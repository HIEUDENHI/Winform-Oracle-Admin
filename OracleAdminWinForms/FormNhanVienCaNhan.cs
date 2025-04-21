using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OracleAdminWinForms
{
    public partial class FormNhanVienCaNhan : Form
    {
        private OracleConnection conn;
        private string username; // Mã nhân viên hiện tại
        private string vaitro;   // Vai trò người dùng hiện tại
        private Timer refreshTimer; // Timer để làm mới giao diện

        public FormNhanVienCaNhan(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper(); // để so sánh chính xác trong Oracle

            // Khởi tạo timer
            refreshTimer = new Timer();
            refreshTimer.Interval = 100; // 100ms
            refreshTimer.Tick += (s, e) =>
            {
                refreshTimer.Stop();
                LoadNhanVienData();
            };
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Form2_Load started");
            LoadVaiTro();
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: {vaitro}";

            if (!vaitro.Contains("NV") && !vaitro.Contains("GV") && vaitro != "TRGĐV")
            {
                MessageBox.Show("Chỉ nhân viên hoặc giảng viên mới được phép truy cập form này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadNhanVienData();
            Debug.WriteLine("Form2_Load completed");
        }

        private void LoadVaiTro()
        {
            Debug.WriteLine("LoadVaiTro started");
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    Debug.WriteLine("Connection opened in LoadVaiTro");
                }

                // Chuyển container sang XEPDB1
                using (var cmdSetContainer = new OracleCommand("ALTER SESSION SET CONTAINER = XEPDB1", conn))
                {
                    cmdSetContainer.ExecuteNonQuery();
                    Debug.WriteLine("Container set to XEPDB1 in LoadVaiTro");
                }

                using (var cmd = new OracleCommand("SELECT VAITRO FROM SYSTEM.VW_NHANVIEN_NVCB WHERE MANV = :username", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", username));
                    object result = cmd.ExecuteScalar();
                    vaitro = result?.ToString() ?? "NVCB";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xác định vai trò người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                vaitro = "NVCB";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    Debug.WriteLine("Connection closed in LoadVaiTro");
                }
            }
            Debug.WriteLine("LoadVaiTro completed");
        }

        private void LoadNhanVienData()
        {
            Debug.WriteLine("LoadNhanVienData started");
            string query = "SELECT MANV, HOTEN, PHAI, NGSINH, ĐT FROM SYSTEM.VW_NHANVIEN_NVCB WHERE MANV = :username";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;
            cmd.Parameters.Add(new OracleParameter("username", username));

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    Debug.WriteLine("Connection opened in LoadNhanVienData");
                }

                // Chuyển container sang XEPDB1
                using (var cmdSetContainer = new OracleCommand("ALTER SESSION SET CONTAINER = XEPDB1", conn))
                {
                    cmdSetContainer.ExecuteNonQuery();
                    Debug.WriteLine("Container set to XEPDB1 in LoadNhanVienData");
                }

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Cập nhật DataGridView và txtSoDienThoai trên UI thread
                if (dgvNhanVien.InvokeRequired || txtSoDienThoai.InvokeRequired)
                {
                    dgvNhanVien.Invoke(new Action(() =>
                    {
                        dgvNhanVien.DataSource = dt;
                        if (dt.Rows.Count > 0)
                        {
                            txtSoDienThoai.Text = dt.Rows[0]["ĐT"]?.ToString() ?? "";
                        }
                    }));
                }
                else
                {
                    dgvNhanVien.DataSource = dt;
                    if (dt.Rows.Count > 0)
                    {
                        txtSoDienThoai.Text = dt.Rows[0]["ĐT"]?.ToString() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    Debug.WriteLine("Connection closed in LoadNhanVienData");
                }
            }
            Debug.WriteLine("LoadNhanVienData completed");
        }

        private async void btnCapNhatSDT_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnCapNhatSDT_Click started");
            string newPhone = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(newPhone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Đảm bảo kết nối đã mở từ khi khởi tạo form
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    Debug.WriteLine("Connection opened in btnCapNhatSDT_Click");
                }

                // Kích hoạt vai trò ROLE_NVCB (nếu cần thiết)
                using (var cmdSetRole = new OracleCommand("SET ROLE ROLE_NVCB", conn))
                {
                    await cmdSetRole.ExecuteNonQueryAsync();
                    Debug.WriteLine("ROLE_NVCB activated");
                }

                // Gọi stored procedure hoặc dùng lệnh SQL trực tiếp
                using (var cmd = new OracleCommand("SYSTEM.UPDATE_NHANVIEN_DT", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_manv", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_dt", OracleDbType.Varchar2).Value = newPhone;
                    cmd.CommandTimeout = 120;
                    await cmd.ExecuteNonQueryAsync();
                    Debug.WriteLine("Stored procedure SYSTEM.UPDATE_NHANVIEN_DT executed successfully");
                }

                // Không thực hiện COMMIT theo yêu cầu của bạn

                // Làm mới giao diện trực tiếp thay vì dùng timer
                MessageBox.Show("Cập nhật số điện thoại thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVienData(); // Gọi trực tiếp thay vì dùng timer
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số điện thoại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Không đóng kết nối ở đây, để đóng khi form đóng
            Debug.WriteLine("btnCapNhatSDT_Click completed");
        }



        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnTaiLai_Click started");
            LoadNhanVienData();
            Debug.WriteLine("btnTaiLai_Click completed");
        }
    }
}