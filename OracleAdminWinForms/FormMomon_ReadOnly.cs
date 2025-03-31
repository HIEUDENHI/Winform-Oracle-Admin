using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormMomon_ReadOnly : Form
    {
        private OracleConnection conn;
        private string username;
        private string role;

        public FormMomon_ReadOnly(OracleConnection connection, string user, string role)
        {
            InitializeComponent();
            conn = connection;
            username = user;
            this.role = role;
        }

        private void FormMomon_ReadOnly_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Môn học giảng dạy - {username}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string viewName = "";

                // Sử dụng switch-case truyền thống do C# 7.3 không hỗ trợ switch expression
                switch (role)
                {
                    case "GV":
                        viewName = "VW_MOMON_GV";
                        break;
                    case "TRGĐV":
                        viewName = "VW_MOMON_TRGDV";
                        break;
                    case "SV":
                        viewName = "VW_MOMON_SV";
                        break;
                    default:
                        throw new Exception("Vai trò không hợp lệ hoặc không được hỗ trợ.");
                }

                string query = $"SELECT * FROM {viewName}";
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
