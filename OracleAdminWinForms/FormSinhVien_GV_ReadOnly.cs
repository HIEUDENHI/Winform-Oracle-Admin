using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormSinhVien_GV_ReadOnly : Form
    {
        private OracleConnection conn;
        private string username;
        private string vaitro;

        public FormSinhVien_GV_ReadOnly(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormSinhVien_GV_ReadOnly_Load(object sender, EventArgs e)
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

            // Xác định vai trò
            LoadVaiTro();
            if (vaitro != "GV")
            {
                MessageBox.Show("Chỉ người dùng có vai trò GV mới được phép truy cập form này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblTitle.Text = $"Danh sách sinh viên thuộc khoa của giảng viên: {username}";
            LoadSinhVienData();
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

        private void LoadSinhVienData()
        {
            try
            {
                string query = @"
                    SELECT * FROM SINHVIEN
                ";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadSinhVienData();
        }
    }
}