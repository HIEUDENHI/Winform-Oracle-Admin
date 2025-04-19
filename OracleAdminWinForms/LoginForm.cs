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
                "SV" // ✅ đã thêm sinh viên vào danh sách
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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn vai trò.");
                return;
            }

            string connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";
            if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                connectionString += "DBA Privilege=SYSDBA;";
            }

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();

                string vaiTroFromDb = null;

                if (selectedRole == "SV")
                {
                    using (var cmd = new OracleCommand("SELECT 'SV' FROM SINHVIEN WHERE MASV = :id", conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                        object result = cmd.ExecuteScalar();
                        vaiTroFromDb = result?.ToString();
                    }
                }
                else
                {
                    using (var cmd = new OracleCommand("SELECT VAITRO FROM NHANVIEN WHERE MANV = :id", conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                        object result = cmd.ExecuteScalar();
                        vaiTroFromDb = result?.ToString();
                    }
                }

                if (vaiTroFromDb == null)
                {
                    MessageBox.Show("Không tìm thấy vai trò trong hệ thống.");
                    conn.Close();
                    return;
                }

                // Chuẩn hóa vai trò từ DB để so sánh với selectedRole
                string normalizedRoleFromDb = vaiTroFromDb.ToUpper().Trim();
                if (normalizedRoleFromDb == "SV" && selectedRole == "SV")
                {
                    // Sinh viên hợp lệ
                }
                else if (normalizedRoleFromDb == selectedRole ||
                         (normalizedRoleFromDb == "NV PĐT" && selectedRole == "NV PĐT") ||
                         (normalizedRoleFromDb == "NV PKT" && selectedRole == "NV PKT") ||
                         (normalizedRoleFromDb == "NV PCTSV" && selectedRole == "NV PCTSV") ||
                         (normalizedRoleFromDb == "NV TCHC" && selectedRole == "NV TCHC"))
                {
                    // Nhân viên hợp lệ
                }
                else
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
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message);
            }
        }
    }
}
