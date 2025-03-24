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
    public partial class FormCreateRole: Form
    {
        private OracleConnection conn;

        public FormCreateRole(OracleConnection connection)
        {
            InitializeComponent();
            this.conn = connection;

        }

        private void FormCreateRole_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string roleName = txtRoleName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Vui lòng nhập tên Role.");
                return;
            }

            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_ROLE_MANAGEMENT.sp_CreateRole", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_role_name", OracleDbType.Varchar2).Value = roleName;
                    // Nếu password rỗng thì truyền null (tạo role không có password)
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(password) ? null : password;
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Tạo role thành công!");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo role: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
