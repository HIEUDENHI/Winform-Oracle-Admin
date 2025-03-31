using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormQuanLyBangDiem_PKT : Form
    {
        private OracleConnection conn;
        private string username;

        public FormQuanLyBangDiem_PKT(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;

            lblTitle.Text = $"Quản lý điểm - {username}";
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
                dgvBangDiem.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng điểm: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("👉 Tính năng cập nhật điểm đang chờ phát triển");
        }

        private void FormQuanLyBangDiem_PKT_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
