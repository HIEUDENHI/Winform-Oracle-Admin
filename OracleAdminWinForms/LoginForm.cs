using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class LoginForm : Form
    {
        public string Username { get; private set; }
        public string RoleSelected { get; private set; }
        public OracleConnection UserConnection { get; private set; }
        public string OlsLabel { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.AddRange(new string[]
            {
                "Admin",
                "NVCB",
                "GV",
                "TRGĐV",
                "NV PKT",
                "NV TCHC",
                "NV PĐT",
                "NV PCTSV",
                "SV",
                "OLS_USER"
            });

            cmbRole.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string selectedRole = cmbRole.SelectedItem?.ToString();

            Username = username;
            RoleSelected = selectedRole;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn vai trò.");
                return;
            }

            // Nếu người dùng chọn vai trò Admin
            if (selectedRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                string connectionString;
                if (username.Equals("sys", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = "User Id=sys;Password=" + password + ";Data Source=localhost:1521/XEPDB1;DBA Privilege=SYSDBA";
                }
                else
                {
                    connectionString = "User Id=" + username + ";Password=" + password + ";Data Source=localhost:1521/XEPDB1";
                }

                try
                {
                    OracleConnection conn = new OracleConnection(connectionString);
                    conn.Open();
                    UserConnection = conn;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đăng nhập thất bại: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
                return;
            }
            // Xử lý cho vai trò OLS_USER
            else if (selectedRole.Equals("OLS_USER", StringComparison.OrdinalIgnoreCase))
            {
                string connectionString;
                if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = "User Id=SYS;Password=" + password + ";Data Source=localhost:1521/XEPDB1;DBA Privilege=SYSDBA";
                }
                else
                {
                    connectionString = "User Id=" + username + ";Password=" + password + ";Data Source=localhost:1521/XEPDB1";
                }

                try
                {
                    OracleConnection conn = new OracleConnection(connectionString);
                    conn.Open();
                    MessageBox.Show("Đăng nhập thành công với vai trò OLS_USER.");
                    UserConnection = conn;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đăng nhập thất bại: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
                return;
            }
            else
            {
                // Đối với các vai trò còn lại: NVCB, GV, TRGĐV, NV PKT, NV TCHC, NV PĐT, NV PCTSV, SV
                username = username.ToUpper();
                string connectionString;
                if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = "User Id=SYS;Password=" + password + ";Data Source=localhost:1521/XEPDB1;DBA Privilege=SYSDBA";
                }
                else
                {
                    connectionString = "User Id=" + username + ";Password=" + password + ";Data Source=localhost:1521/XEPDB1";
                }

                try
                {
                    OracleConnection conn = new OracleConnection(connectionString);
                    conn.Open();

                    string vaiTroFromDb = null;

                    if (selectedRole.Equals("SV", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var cmd = new OracleCommand("SELECT 'SV' FROM VW_SINHVIEN_SV WHERE MASV = :id", conn))
                        {
                            cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                            var result = cmd.ExecuteScalar();
                            vaiTroFromDb = result?.ToString();
                        }
                    }
                    else
                    {
                        using (var cmd = new OracleCommand("SELECT VAITRO FROM VW_NHANVIEN_NVCB WHERE MANV = :id", conn))
                        {
                            cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                            var result = cmd.ExecuteScalar();
                            vaiTroFromDb = result?.ToString();
                        }
                    }

                    if (vaiTroFromDb == null)
                    {
                        MessageBox.Show("Không tìm thấy vai trò trong hệ thống.");
                        conn.Close();
                        return;
                    }

                    if (!vaiTroFromDb.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Bạn chọn vai trò [{selectedRole}], nhưng hệ thống ghi nhận là [{vaiTroFromDb}].");
                        conn.Close();
                        return;
                    }

                    UserConnection = conn;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đăng nhập thất bại: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
            }
        }

        private string GetOlsLabel(OracleConnection conn, string username)
        {
            try
            {
                using (var cmd = new OracleCommand("SELECT SA_USER_ADMIN.GET_USER_LABELS('THONGBAO_POLICY', :username) FROM DUAL", conn))
                {
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "Unknown";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể lấy nhãn OLS: " + ex.Message);
                return "Unknown";
            }
        }
    }
}