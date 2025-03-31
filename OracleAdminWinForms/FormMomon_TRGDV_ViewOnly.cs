using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormMomon_TRGDV_ViewOnly : Form
    {
        private OracleConnection conn;
        private string username;

        public FormMomon_TRGDV_ViewOnly(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void FormMomon_TRGDV_ViewOnly_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Xem môn giảng dạy - Trưởng đơn vị: {username}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM MOMON";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvMomon.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }
    }
}
