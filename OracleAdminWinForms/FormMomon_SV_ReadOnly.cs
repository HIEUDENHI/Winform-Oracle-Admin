using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormMomon_SV_ReadOnly : Form
    {
        private OracleConnection conn;
        private string username;

        public FormMomon_SV_ReadOnly(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void FormMomon_SV_ReadOnly_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Môn học của khoa - SV: {username}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM VW_MOMON_SV";
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
