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
    public partial class FormCreateUser: Form
    {
        private OracleConnection conn;

        public FormCreateUser(OracleConnection connection)
        {
            InitializeComponent();
            this.conn = connection;

        }

        private void FormCreateUser_Load(object sender, EventArgs e)
        {

        }
        

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Username và Password.");
                return;
            }

            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_USER_MANAGEMENT.sp_CreateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thủ tục: sp_CreateUser(p_username IN VARCHAR2, p_password IN VARCHAR2)
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = password;

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Tạo user thành công!");
                this.DialogResult = DialogResult.OK; // Để FormCreateUser đóng và báo về cha
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo user: " + ex.Message);
            }

        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
