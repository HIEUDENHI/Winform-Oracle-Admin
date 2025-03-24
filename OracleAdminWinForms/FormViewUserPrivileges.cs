using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OracleAdminWinForms
{
    public partial class FormViewUserPrivileges: Form
    {
        private OracleConnection _conn;
        private string _username;
        public FormViewUserPrivileges(OracleConnection conn, string username)
        {
            InitializeComponent();
            _conn = conn;
            _username = username;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormViewUserPrivileges_Load(object sender, EventArgs e)
        {
            LoadUserObjectPrivileges();

        }
        private void LoadUserObjectPrivileges()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_GetUserObjectPrivileges", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = _username;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    dgvPrivileges.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy quyền: " + ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvPrivileges.CurrentRow != null)
            {
                // Lấy thông tin từ DataGridView
                string schema = dgvPrivileges.CurrentRow.Cells["OWNER"].Value.ToString();
                string objectName = dgvPrivileges.CurrentRow.Cells["OBJECT_NAME"].Value.ToString();
                string privilege = dgvPrivileges.CurrentRow.Cells["PRIVILEGE"].Value.ToString();

                // Gọi stored procedure sp_RevokeObjectPrivilegeFromUser
                using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_RevokeObjectPrivilegeFromUser", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = _username;
                    cmd.Parameters.Add("p_privilege", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("p_schema", OracleDbType.Varchar2).Value = schema;
                    cmd.Parameters.Add("p_object", OracleDbType.Varchar2).Value = objectName;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thu hồi quyền thành công!");

                        // Gọi lại hàm load để refresh danh sách quyền
                        LoadUserObjectPrivileges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thu hồi quyền: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một quyền trong danh sách để thu hồi.");
            }
        }
    }
}
