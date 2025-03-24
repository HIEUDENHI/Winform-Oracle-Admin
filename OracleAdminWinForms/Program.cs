using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OracleAdminWinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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
                    // Nếu đăng nhập thành công, mở form chính và truyền OracleConnection
                    Application.Run(new Form1(loginForm.UserConnection));
                }
                else
                {
                    // Nếu đăng nhập thất bại hoặc người dùng đóng form, thoát ứng dụng
                    Application.Exit();
                }
            }
        }
    }
}
