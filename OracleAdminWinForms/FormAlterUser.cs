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
    public partial class FormAlterUser: Form
    {
        private OracleConnection conn;
        private string username;
        public FormAlterUser(OracleConnection connection, string user)
        {
            InitializeComponent();
            this.conn = connection;
            this.username = user;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!");
                return;
            }

            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_USER_MANAGEMENT.sp_AlterUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_new_password", OracleDbType.Varchar2).Value = newPassword;

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đổi mật khẩu thành công!");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FormAlterUser_Load(object sender, EventArgs e)
        {
            lblUsernameValue.Text = username;

        }
    }
}
