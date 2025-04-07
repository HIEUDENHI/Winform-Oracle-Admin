using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormMomon_PDT_FULLCRUD : Form
    {
        private OracleConnection conn;
        private string username;

        public FormMomon_PDT_FULLCRUD(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void FormMomon_PDT_FULLCRUD_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: NV PĐT";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM VW_MOMON_PDT";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvMomon.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dgvMomon.CurrentRow;
                string query = @"INSERT INTO MOMON (MAMM, MAHP, MAGV, HK, NAM)
                                 VALUES (:mamm, :mahp, :magv, :hk, :nam)";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":mamm", row.Cells["MAMM"].Value);
                    cmd.Parameters.Add(":mahp", row.Cells["MAHP"].Value);
                    cmd.Parameters.Add(":magv", row.Cells["MAGV"].Value);
                    cmd.Parameters.Add(":hk", row.Cells["HK"].Value);
                    cmd.Parameters.Add(":nam", row.Cells["NAM"].Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã thêm thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dgvMomon.CurrentRow;
                string query = @"UPDATE MOMON 
                                 SET MAHP = :mahp, MAGV = :magv, HK = :hk, NAM = :nam
                                 WHERE MAMM = :mamm";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":mahp", row.Cells["MAHP"].Value);
                    cmd.Parameters.Add(":magv", row.Cells["MAGV"].Value);
                    cmd.Parameters.Add(":hk", row.Cells["HK"].Value);
                    cmd.Parameters.Add(":nam", row.Cells["NAM"].Value);
                    cmd.Parameters.Add(":mamm", row.Cells["MAMM"].Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đã cập nhật thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var mamm = dgvMomon.CurrentRow.Cells["MAMM"].Value.ToString();
                DialogResult result = MessageBox.Show($"Xác nhận xoá dòng MAMM = {mamm}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM MOMON WHERE MAMM = :mamm", conn))
                    {
                        cmd.Parameters.Add(":mamm", mamm);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã xoá thành công!");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá: " + ex.Message);
            }
        }
    }
}
