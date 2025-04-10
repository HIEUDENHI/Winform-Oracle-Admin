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
        public string OlsLabel { get; private set; } // Optional: Store the OLS label for later use

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.AddRange(new string[]
            {
                "NVCB",
                "GV",
                "TRGĐV",
                "NV PKT",
                "NV TCHC",
                "NV PĐT",
                "NV PCTSV",
                "SV",
                "OLS_USER" // Add a new role for OLS users
            });

            cmbRole.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim().ToUpper();
            string password = txtPassword.Text;
            string selectedRole = cmbRole.SelectedItem?.ToString();

            Username = username;
            RoleSelected = selectedRole;

            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn vai trò.");
                return;
            }

            // Xây dựng chuỗi kết nối
            string connectionString;
            if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1.lan)));DBA Privilege=SYSDBA;";
            }
            else
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1.lan)));";
            }

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();

                // Bỏ qua việc lấy nhãn OLS cho vai trò OLS_USER
                if (selectedRole == "OLS_USER")
                {
                    MessageBox.Show($"Đăng nhập thành công với vai trò OLS_USER.");

                    UserConnection = conn;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }

                // Kiểm tra vai trò trong cơ sở dữ liệu
                string vaiTroFromDb = null;

                if (selectedRole == "SV")
                {
                    // Kiểm tra vai trò sinh viên
                    using (var cmd = new OracleCommand("SELECT 'SV' FROM SYSTEM.SINHVIEN WHERE MASV = :id", conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                        object result = cmd.ExecuteScalar();
                        vaiTroFromDb = result?.ToString();
                    }
                }
                else
                {
                    // Kiểm tra vai trò nhân viên
                    using (var cmd = new OracleCommand("SELECT VAITRO FROM SYSTEM.NHANVIEN WHERE MANV = :id", conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                        object result = cmd.ExecuteScalar();
                        vaiTroFromDb = result?.ToString();
                    }
                }

                // Kiểm tra vai trò có tồn tại không
                if (vaiTroFromDb == null)
                {
                    MessageBox.Show("Không tìm thấy vai trò trong hệ thống.");
                    conn.Close();
                    return;
                }

                // So sánh vai trò người dùng chọn với vai trò trong cơ sở dữ liệu
                if (!vaiTroFromDb.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Bạn chọn vai trò [{selectedRole}], nhưng hệ thống ghi nhận là [{vaiTroFromDb}].");
                    conn.Close();
                    return;
                }

                // Đăng nhập thành công
                UserConnection = conn;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
            }
        }

        // Optional: Method to fetch the OLS label for the user
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