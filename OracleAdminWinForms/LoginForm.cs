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
    public partial class LoginForm: Form
    {
        public string Username { get; private set; }

        public OracleConnection UserConnection { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Lưu username vào thuộc tính công khai
            Username = username;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ username và password.");
                return;
            }

            string connectionString;
            // Nếu tài khoản là SYS thì thêm tham số DBA Privilege
            if (username.Equals("sys", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));DBA Privilege=SYSDBA;";
            }
            else
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";
            }

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open(); // Kiểm tra đăng nhập
                UserConnection = conn;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message);
            }
        }
    }
}
