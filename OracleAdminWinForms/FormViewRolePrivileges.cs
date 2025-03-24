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

namespace OracleAdminWinForms
{
    public partial class FormViewRolePrivileges: Form
    {
        private OracleConnection _conn;
        private string _roleName;
        public FormViewRolePrivileges(OracleConnection conn, string roleName)
        {
            InitializeComponent();
            _conn = conn;
            _roleName = roleName;
        }

        private void FormViewRolePrivileges_Load(object sender, EventArgs e)
        {
            LoadRoleObjectPrivileges();

        }
        private void LoadRoleObjectPrivileges()
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_GetRoleObjectPrivileges", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_rolename", OracleDbType.Varchar2).Value = _roleName;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    dgvRolePrivileges.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy quyền của role: " + ex.Message);
            }
        }

        private void btnRevokePrivilege_Click(object sender, EventArgs e)
        {
            if (dgvRolePrivileges.CurrentRow != null)
            {
                string schema = dgvRolePrivileges.CurrentRow.Cells["OWNER"].Value.ToString();
                string objectName = dgvRolePrivileges.CurrentRow.Cells["OBJECT_NAME"].Value.ToString();
                string privilege = dgvRolePrivileges.CurrentRow.Cells["PRIVILEGE"].Value.ToString();

                using (OracleCommand cmd = new OracleCommand("PKG_OBJECT_MANAGEMENT.sp_RevokeObjectPrivilegeFromRole", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_rolename", OracleDbType.Varchar2).Value = _roleName;
                    cmd.Parameters.Add("p_privilege", OracleDbType.Varchar2).Value = privilege;
                    cmd.Parameters.Add("p_schema", OracleDbType.Varchar2).Value = schema;
                    cmd.Parameters.Add("p_object", OracleDbType.Varchar2).Value = objectName;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thu hồi quyền thành công!");

                        // Refresh lại danh sách
                        LoadRoleObjectPrivileges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thu hồi quyền: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một quyền để thu hồi!");
            }
        }
    }
}
