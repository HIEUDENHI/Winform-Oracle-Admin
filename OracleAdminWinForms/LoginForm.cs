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
        public string OlsLabel { get; private set; } // Optional: Lưu nhãn OLS nếu cần

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.AddRange(new string[]
            {
                "Admin",      // Thêm vai trò Admin
                "NVCB",
                "GV",
                "TRGĐV",
                "NV PKT",
                "NV TCHC",
                "NV PĐT",
                "NV PCTSV",
                "SV",
                "OLS_USER"    // Vai trò OLS_USER
            });

            cmbRole.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string selectedRole = cmbRole.SelectedItem?.ToString();

            // Lưu thông tin vào thuộc tính của form
            Username = username;
            RoleSelected = selectedRole;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn vai trò.");
                return;
            }

            // Nếu người dùng chọn vai trò Admin thì dùng đoạn mã đăng nhập sau:
            if (selectedRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
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
                return;
            }
            // Xử lý cho vai trò OLS_USER
            else if (selectedRole.Equals("OLS_USER", StringComparison.OrdinalIgnoreCase))
            {
                string connectionString;
                if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));DBA Privilege=SYSDBA;";
                }
                else
                {
                    connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";
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
                // Chuyển đổi username sang chữ hoa để so sánh
                username = username.ToUpper();

                string connectionString;
                if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));DBA Privilege=SYSDBA;";
                }
                else
                {
                    connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";
                }

                try
                {
                    OracleConnection conn = new OracleConnection(connectionString);
                    conn.Open();

                    string vaiTroFromDb = null;

                    if (selectedRole.Equals("SV", StringComparison.OrdinalIgnoreCase))
                    {
                        // Kiểm tra vai trò sinh viên
                        using (var cmd = new OracleCommand("SELECT 'SV' FROM SYSTEM.SINHVIEN WHERE MASV = :id", conn))
                        {
                            cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                            var result = cmd.ExecuteScalar();
                            vaiTroFromDb = result?.ToString();
                        }
                    }
                    else
                    {
                        // Kiểm tra vai trò nhân viên
                        using (var cmd = new OracleCommand("SELECT VAITRO FROM SYSTEM.NHANVIEN WHERE MANV = :id", conn))
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

                    // So sánh vai trò người dùng chọn với vai trò ghi nhận trong cơ sở dữ liệu
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

        // Optional: Phương thức lấy nhãn OLS cho người dùng (nếu cần)
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
