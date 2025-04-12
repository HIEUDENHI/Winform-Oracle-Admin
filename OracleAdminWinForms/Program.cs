using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Mở LoginForm để thực hiện đăng nhập
            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Lấy thông tin đăng nhập từ LoginForm
                    string role = loginForm.RoleSelected.ToUpper();
                    OracleConnection conn = loginForm.UserConnection;

                    // Nếu đăng nhập với vai trò ADMIN thì mở Form1, ngược lại mở FormMain
                    if (role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
                    {
                        Application.Run(new Form1(conn));
                    }
                    else
                    {
                        string username = loginForm.Username.ToUpper();
                        Application.Run(new FormMain(conn, username, role));
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
