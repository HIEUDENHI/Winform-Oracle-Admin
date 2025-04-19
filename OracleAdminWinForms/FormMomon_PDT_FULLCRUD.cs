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
        private string vaitro;

        public FormMomon_PDT_FULLCRUD(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormMomon_PDT_FULLCRUD_Load(object sender, EventArgs e)
        {
            LoadVaiTro();
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: {vaitro}";

            if (vaitro != "NV PĐT")
            {
                MessageBox.Show("Chỉ người dùng có vai trò NV PĐT mới được phép truy cập form này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadData();
        }

        private void LoadVaiTro()
        {
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
                MessageBox.Show("Không thể xác định vai trò người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                vaitro = "NVCB";
            }
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM VW_MOMON_PDT";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dgvMomon.InvokeRequired)
                {
                    dgvMomon.Invoke(new Action(() =>
                    {
                        dgvMomon.DataSource = dt;
                    }));
                }
                else
                {
                    dgvMomon.DataSource = dt;
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có môn mở nào thuộc học kỳ hiện tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string mamm = txtMAMM.Text.Trim();
            string mahp = txtMAHP_Them.Text.Trim();
            string magv = txtMAGV_Them.Text.Trim();
            int hk = DateTime.Now.Month >= 1 && DateTime.Now.Month <= 4 ? 2 :
                     DateTime.Now.Month >= 5 && DateTime.Now.Month <= 8 ? 3 : 1;
            int nam = DateTime.Now.Year;

            if (string.IsNullOrEmpty(mamm) || string.IsNullOrEmpty(mahp) || string.IsNullOrEmpty(magv))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin MAMM, MAHP, và MAGV!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = @"INSERT INTO VW_MOMON_PDT (MAMM, MAHP, MAGV, HK, NAM)
                                 VALUES (:mamm, :mahp, :magv, :hk, :nam)";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":mamm", mamm);
                    cmd.Parameters.Add(":mahp", mahp);
                    cmd.Parameters.Add(":magv", magv);
                    cmd.Parameters.Add(":hk", hk);
                    cmd.Parameters.Add(":nam", nam);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Đã thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtMAMM.Clear();
                        txtMAHP_Them.Clear();
                        txtMAGV_Them.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm môn mở. Kiểm tra lại dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvMomon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một môn mở để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string mamm = dgvMomon.SelectedRows[0].Cells["MAMM"].Value.ToString();
            string mahp = txtMAHP_Update.Text.Trim();
            string magv = txtMAGV_Update.Text.Trim();

            if (string.IsNullOrEmpty(mahp) || string.IsNullOrEmpty(magv))
            {
                MessageBox.Show("Vui lòng nhập MAHP và MAGV để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string query = @"UPDATE VW_MOMON_PDT 
                                 SET MAHP = :mahp, MAGV = :magv
                                 WHERE MAMM = :mamm";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":mahp", mahp);
                    cmd.Parameters.Add(":magv", magv);
                    cmd.Parameters.Add(":mamm", mamm);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Đã cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        txtMAHP_Update.Clear();
                        txtMAGV_Update.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy môn mở để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMomon.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một môn mở để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string mamm = dgvMomon.SelectedRows[0].Cells["MAMM"].Value.ToString();
            DialogResult result = MessageBox.Show($"Xác nhận xoá dòng MAMM = {mamm}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM VW_MOMON_PDT WHERE MAMM = :mamm", conn))
                    {
                        cmd.Parameters.Add(":mamm", mamm);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Đã xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy môn mở để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xoá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvMomon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMomon.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvMomon.SelectedRows[0];
                txtMAHP_Update.Text = selectedRow.Cells["MAHP"].Value?.ToString() ?? "";
                txtMAGV_Update.Text = selectedRow.Cells["MAGV"].Value?.ToString() ?? "";
            }
            else
            {
                txtMAHP_Update.Clear();
                txtMAGV_Update.Clear();
            }
        }
    }
}