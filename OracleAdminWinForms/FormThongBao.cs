using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormThongBao : Form
    {
        private OracleConnection conn;
        private string username;

        public FormThongBao(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormThongBao_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Thông báo dành cho: {username}";
            LoadThongBaoData();
        }

        private void LoadThongBaoData()
        {
            try
            {
                string query = "SELECT MATB, NOIDUNG, NGAYGUI FROM ADMIN_OLS.THONGBAO ";
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvThongBao.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông báo: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadThongBaoData();
        }
    }
}
