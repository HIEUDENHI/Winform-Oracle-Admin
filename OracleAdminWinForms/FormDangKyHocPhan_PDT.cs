using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormDangKyHocPhan_PDT : Form
    {
        private OracleConnection conn;
        private string username;

        public FormDangKyHocPhan_PDT(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;

            lblTitle.Text = $"Đăng ký học phần - NV PĐT: {username}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM DANGKY";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvDangKy.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu đăng ký: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("👉 Tính năng thêm đang chờ phát triển");
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("👉 Tính năng cập nhật đang chờ phát triển");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            MessageBox.Show("👉 Tính năng xóa đang chờ phát triển");
        }

        private void FormDangKyHocPhan_PDT_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
