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

            string connectionString;
            if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=15211))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));DBA Privilege=SYSDBA;";
            }
            else
            {
                connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=15211))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));";
            }

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();

                string vaiTroFromDb = null;

                if (selectedRole == "SV")
                {
                    // ✅ Check vai trò sinh viên
                    using (var cmd = new OracleCommand("SELECT 'SV' FROM SYSTEM.SINHVIEN WHERE MASV = :id", conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Varchar2).Value = username;
                        object result = cmd.ExecuteScalar();
                        vaiTroFromDb = result?.ToString();
                    }
                }
                else
                {
                    // ✅ Check vai trò nhân viên
                    using (var cmd = new OracleCommand("SELECT VAITRO FROM SYSTEM.NHANVIEN WHERE MANV = :id", conn))
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

                if (!vaiTroFromDb.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Bạn chọn vai trò [{selectedRole}], nhưng hệ thống ghi nhận là [{vaiTroFromDb}].");
                    conn.Close();
                    return;
                }

                // ✅ Đăng nhập thành công
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
