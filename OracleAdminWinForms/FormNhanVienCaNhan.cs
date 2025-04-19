using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

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
            LoadNhanVienData();
            Debug.WriteLine("Form2_Load completed");
        }

        private void LoadVaiTro()
        {
            Debug.WriteLine("LoadVaiTro started");
            try
            {
                using (var cmd = new OracleCommand("SELECT VAITRO FROM NHANVIEN WHERE UPPER(MANV) = :username", conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", username));
                    object result = cmd.ExecuteScalar();
                    vaitro = result?.ToString() ?? "NVCB";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xác định vai trò người dùng: " + ex.Message);
                vaitro = "NVCB";
            }
            Debug.WriteLine("LoadVaiTro completed");
        }

        private void LoadNhanVienData()
        {
            Debug.WriteLine("LoadNhanVienData started");
            string query = "SELECT MANV, HOTEN, PHAI, NGSINH, ĐT FROM NHANVIEN WHERE MANV = :username";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;
            cmd.Parameters.Add(new OracleParameter("username", username));

            try
            {
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
                MessageBox.Show("Lỗi tải dữ liệu nhân viên: " + ex.Message);
            }
            Debug.WriteLine("LoadNhanVienData completed");
        }

        private void btnCapNhatSDT_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("btnCapNhatSDT_Click started");
            string newPhone = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(newPhone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại mới.");
                return;
            }

            try
            {
                // Kiểm tra trạng thái kết nối
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    Debug.WriteLine("Connection reopened");
                }

                // Kích hoạt ROLE_NVCB trong session
                using (var cmdSetRole = new OracleCommand("SET ROLE ROLE_NVCB", conn))
                {
                    cmdSetRole.ExecuteNonQuery();
                    Debug.WriteLine("ROLE_NVCB activated");
                }

                // Chỉ cập nhật số điện thoại của user hiện tại
                using (var cmd = new OracleCommand("UPDATE NHANVIEN SET ĐT = :sdt WHERE MANV = :username", conn))
                {
                    cmd.Parameters.Add("sdt", newPhone);
                    cmd.Parameters.Add("username", username);
                    cmd.CommandTimeout = 30; // Đặt timeout 30 giây
                    int rows = cmd.ExecuteNonQuery();
                    Debug.WriteLine($"Rows updated: {rows}");
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật số điện thoại thành công!");
                        // Sử dụng timer để làm mới giao diện
                        refreshTimer.Start();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số điện thoại: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    Debug.WriteLine("Connection closed");
                }
            }
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