using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormTRGDV_XemNhanVien : Form
    {
        private OracleConnection conn;
        private string username; // Mã nhân viên hiện tại
        private string vaitro;   // Vai trò người dùng hiện tại

        public FormTRGDV_XemNhanVien(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper(); // Để so sánh chính xác trong Oracle
        }

        private void FormTRGDV_XemNhanVien_Load(object sender, EventArgs e)
        {
            // Kiểm tra và mở kết nối nếu cần
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
            }

            LoadVaiTro();
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: {vaitro}";

            // Kiểm tra vai trò
            if (vaitro != "TRGĐV")
            {
                MessageBox.Show("Chỉ người dùng có vai trò TRGĐV mới được phép truy cập form này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadNhanVienData();
        }

        private void LoadVaiTro()
        {
            try
            {
                using (var cmd = new OracleCommand("SELECT VAITRO FROM VW_NHANVIEN_NVCB WHERE MANV = :username", conn))
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
        }

        private void LoadNhanVienData()
        {
            string query = @"SELECT * FROM VW_NHANVIEN_TRGDV WHERE MANV != :username";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = query;
            cmd.Parameters.Add(new OracleParameter("username", username));

            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Cập nhật DataGridView trên UI thread
                if (dgvNhanVien.InvokeRequired)
                {
                    dgvNhanVien.Invoke(new Action(() =>
                    {
                        dgvNhanVien.DataSource = dt;
                    }));
                }
                else
                {
                    dgvNhanVien.DataSource = dt;
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có nhân viên nào khác trong cùng đơn vị!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}