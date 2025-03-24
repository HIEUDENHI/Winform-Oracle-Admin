using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormGrantPrivilege : Form
    {
        private OracleConnection conn;
        private string username;

        public FormGrantPrivilege(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;

            // Load các loại đối tượng (TABLE, VIEW, PROCEDURE, FUNCTION)
            LoadObjectTypes();

            // Load dữ liệu cho các ComboBox: Schema, Privilege,...
            LoadComboBoxData();
        }

        #region Load dữ liệu ban đầu

        // Load các loại đối tượng vào ComboBox cbObjectType
        private void LoadObjectTypes()
        {
            cbObjectType.Items.Clear();
            cbObjectType.Items.Add("TABLE");
            cbObjectType.Items.Add("VIEW");
            cbObjectType.Items.Add("PROCEDURE");
            cbObjectType.Items.Add("FUNCTION");

            // Nếu có thể, chọn mặc định loại đối tượng đầu tiên
            if (cbObjectType.Items.Count > 0)
                cbObjectType.SelectedIndex = 0;
        }

        // Load dữ liệu cho ComboBox Schema và thiết lập Privilege ban đầu
        private void LoadComboBoxData()
        {
            try
            {
                // Lấy danh sách schema qua stored procedure sp_GetSchemas
                using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_GetSchemas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    List<string> schemas = new List<string>();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schemas.Add(reader.GetString(0)); // Lấy tên schema
                        }
                    }
                    cbSchema.DataSource = schemas;
                }

                // Chọn mặc định schema đầu tiên (nếu có)
                if (cbSchema.Items.Count > 0)
                    cbSchema.SelectedIndex = 0;

                // Load danh sách đối tượng dựa trên schema và object type đã chọn
                RefreshObjectNames();

                // Load danh sách privilege phù hợp theo object type đã chọn
                LoadPrivilegesForObjectType();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message);
            }
        }

        #endregion

        #region Load đối tượng & privilege

        // Load danh sách đối tượng theo schema và object type hiện tại
        private void RefreshObjectNames()
        {
            if (cbSchema.SelectedItem == null || cbObjectType.SelectedItem == null)
                return;

            string schema = cbSchema.SelectedItem.ToString();
            string objectType = cbObjectType.SelectedItem.ToString();

            try
            {
                string query = $"SELECT OBJECT_NAME FROM ALL_OBJECTS " +
                               $"WHERE OBJECT_TYPE = '{objectType}' " +
                               $"AND OWNER = '{schema.ToUpper()}' " +
                               $"ORDER BY OBJECT_NAME";

                List<string> objects = new List<string>();
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objects.Add(reader.GetString(0));
                        }
                    }
                }
                cbObjectName.DataSource = objects;
                // Nếu có đối tượng, chọn mặc định đối tượng đầu tiên
                if (cbObjectName.Items.Count > 0)
                    cbObjectName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đối tượng: " + ex.Message);
            }
        }

        // Load danh sách privilege phù hợp theo loại đối tượng (TABLE/VIEW: SELECT, INSERT, UPDATE, DELETE; PROCEDURE/FUNCTION: EXECUTE)
        private void LoadPrivilegesForObjectType()
        {
            if (cbObjectType.SelectedItem == null)
                return;

            string objectType = cbObjectType.SelectedItem.ToString();
            List<string> privileges = new List<string>();

            if (objectType == "TABLE" || objectType == "VIEW")
            {
                privileges.Add("SELECT");
                privileges.Add("INSERT");
                privileges.Add("UPDATE");
                privileges.Add("DELETE");
            }
            else if (objectType == "PROCEDURE" || objectType == "FUNCTION")
            {
                privileges.Add("EXECUTE");
            }
            cbObjectPrivilege.DataSource = privileges;
            if (cbObjectPrivilege.Items.Count > 0)
                cbObjectPrivilege.SelectedIndex = 0;
        }

        // Load danh sách cột cho đối tượng TABLE/VIEW theo schema và tên đối tượng
        private void LoadColumns(string objectName)
        {
            try
            {
                string schema = cbSchema.SelectedItem.ToString();

                cbColumns.Items.Clear();

                string query = $"SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS " +
                               $"WHERE TABLE_NAME = '{objectName.ToUpper()}' " +
                               $"AND OWNER = '{schema.ToUpper()}' " +
                               $"ORDER BY COLUMN_ID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbColumns.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách cột: " + ex.Message);
            }
        }

        // Lấy danh sách cột đã được chọn trong CheckedListBox cbColumns
        private string GetSelectedColumns()
        {
            List<string> selectedColumns = new List<string>();
            foreach (var item in cbColumns.CheckedItems)
            {
                selectedColumns.Add(item.ToString());
            }
            return string.Join(",", selectedColumns);
        }

        #endregion

        #region Sự kiện Control

        // Khi chọn Schema (cbSchema)
        private void cbSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshObjectNames();
            UpdateColumnsVisibility();
        }

        // Khi chọn Object Type (cbObjectType)
        private void cbObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPrivilegesForObjectType();
            RefreshObjectNames();
            // Clear và ẩn danh sách cột
            cbColumns.Items.Clear();
            cbColumns.Visible = false;
        }

        // Khi chọn Object Name (cbObjectName)
        private void cbObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateColumnsVisibility();
        }

        // Khi chọn Privilege (cbObjectPrivilege)
        private void cbObjectPrivilege_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateColumnsVisibility();
        }

        // Kiểm tra nếu privilege là SELECT hoặc UPDATE trên TABLE/VIEW thì load danh sách cột
        private void UpdateColumnsVisibility()
        {
            if (cbObjectPrivilege.SelectedItem == null ||
                cbObjectType.SelectedItem == null ||
                cbObjectName.SelectedItem == null)
                return;

            string privilege = cbObjectPrivilege.SelectedItem.ToString();
            string objectType = cbObjectType.SelectedItem.ToString();

            if ((privilege == "SELECT" || privilege == "UPDATE") &&
                (objectType == "TABLE" || objectType == "VIEW"))
            {
                string objectName = cbObjectName.SelectedItem.ToString();
                LoadColumns(objectName);
                cbColumns.Visible = true;
            }
            else
            {
                cbColumns.Items.Clear();
                cbColumns.Visible = false;
            }
        }

        // Khi nhấn nút OK để cấp quyền
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string schema = cbSchema.SelectedItem.ToString();
                string objectPrivilege = cbObjectPrivilege.SelectedItem.ToString();
                string objectName = cbObjectName.SelectedItem.ToString();
                string columnList = GetSelectedColumns();
                string withGrantOption = chkWithGrantOption.Checked ? "YES" : "NO";

                // Gọi stored procedure cấp quyền
                GrantObjectPrivilege(objectPrivilege, objectName, columnList, withGrantOption);

                MessageBox.Show("Cấp quyền thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cấp quyền: " + ex.Message);
            }
        }

        // Nút Cancel: đóng form
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Event handler cho cbColumns (nếu cần xử lý bổ sung khi chọn cột)
        private void cbColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hiện tại không cần xử lý thêm, để trống
        }

        // Nếu bạn muốn xử lý sự kiện Load của form (nếu không, có thể bỏ qua)
        private void FormGrantPrivilege_Load(object sender, EventArgs e)
        {
            // Có thể gọi LoadComboBoxData() nếu chưa gọi trong constructor
            // LoadComboBoxData();
        }

        #endregion

        #region Gọi Stored Procedure cấp quyền

        // Gọi stored procedure để cấp quyền cho user
        private void GrantObjectPrivilege(string privilege, string objectName, string columnList, string withGrantOption)
        {
            string schema = cbSchema.SelectedItem.ToString();

            using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_GrantObjectPrivilegeToUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("p_privilege", OracleDbType.Varchar2).Value = privilege;
                cmd.Parameters.Add("p_schema", OracleDbType.Varchar2).Value = schema;
                cmd.Parameters.Add("p_object", OracleDbType.Varchar2).Value = objectName;
                cmd.Parameters.Add("p_column_list", OracleDbType.Varchar2).Value = columnList;
                cmd.Parameters.Add("p_with_grant_option", OracleDbType.Varchar2).Value = withGrantOption;

                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
