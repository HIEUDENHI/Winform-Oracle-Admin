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

            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Lấy thông tin đăng nhập từ LoginForm
                    string role = loginForm.RoleSelected.ToUpper();
                    string username = loginForm.Username.ToUpper();
                    OracleConnection conn = loginForm.UserConnection;

                    // Mở FormMain duy nhất, tự động điều chỉnh giao diện theo role
                    Application.Run(new FormMain(conn, username, role));
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}
