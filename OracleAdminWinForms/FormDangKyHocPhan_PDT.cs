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

                // Nếu không có dữ liệu, hiển thị thông báo (có thể do VPD chặn)
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị. Có thể bạn đang truy cập ngoài 14 ngày đầu học kỳ hoặc dữ liệu không thỏa mãn điều kiện VPD.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Tạo form nhập liệu để thêm bản ghi mới
            using (var inputForm = new Form())
            {
                inputForm.Text = "Thêm đăng ký học phần";
                inputForm.Size = new System.Drawing.Size(300, 200);
                inputForm.StartPosition = FormStartPosition.CenterParent;

                Label lblMASV = new Label { Text = "Mã sinh viên (MASV):", Location = new System.Drawing.Point(20, 20) };
                TextBox txtMASV = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 150 };

                Label lblMAMM = new Label { Text = "Mã môn mở (MAMM):", Location = new System.Drawing.Point(20, 60) };
                TextBox txtMAMM = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 150 };

                Button btnOK = new Button { Text = "Thêm", Location = new System.Drawing.Point(120, 100), Width = 70 };
                Button btnCancel = new Button { Text = "Hủy", Location = new System.Drawing.Point(200, 100), Width = 70 };

                btnOK.Click += (s, ev) =>
                {
                    string masv = txtMASV.Text.Trim();
                    string mamm = txtMAMM.Text.Trim();

                    if (string.IsNullOrEmpty(masv) || string.IsNullOrEmpty(mamm))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Mã môn mở!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        string query = "INSERT INTO DANGKY (MASV, MAMM, ĐIEMTH, ĐIEMQT, ĐIEMCK, ĐIEMTK) " +
                                       "VALUES (:masv, :mamm, NULL, NULL, NULL, NULL)";
                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("masv", masv));
                            cmd.Parameters.Add(new OracleParameter("mamm", mamm));
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thêm bản ghi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData(); // Tải lại dữ liệu
                                inputForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Không thể thêm bản ghi. Có thể bạn đang thao tác ngoài 14 ngày đầu học kỳ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancel.Click += (s, ev) => inputForm.Close();

                inputForm.Controls.AddRange(new Control[] { lblMASV, txtMASV, lblMAMM, txtMAMM, btnOK, btnCancel });
                inputForm.ShowDialog();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvDangKy.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvDangKy.SelectedRows[0];
            string oldMASV = selectedRow.Cells["MASV"].Value.ToString();
            string oldMAMM = selectedRow.Cells["MAMM"].Value.ToString();

            // Tạo form nhập liệu để cập nhật
            using (var inputForm = new Form())
            {
                inputForm.Text = "Cập nhật đăng ký học phần";
                inputForm.Size = new System.Drawing.Size(300, 200);
                inputForm.StartPosition = FormStartPosition.CenterParent;

                Label lblMASV = new Label { Text = "Mã sinh viên (MASV):", Location = new System.Drawing.Point(20, 20) };
                TextBox txtMASV = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 150, Text = oldMASV };

                Label lblMAMM = new Label { Text = "Mã môn mở (MAMM):", Location = new System.Drawing.Point(20, 60) };
                TextBox txtMAMM = new TextBox { Location = new System.Drawing.Point(120, 60), Width = 150, Text = oldMAMM };

                Button btnOK = new Button { Text = "Cập nhật", Location = new System.Drawing.Point(120, 100), Width = 70 };
                Button btnCancel = new Button { Text = "Hủy", Location = new System.Drawing.Point(200, 100), Width = 70 };

                btnOK.Click += (s, ev) =>
                {
                    string newMASV = txtMASV.Text.Trim();
                    string newMAMM = txtMAMM.Text.Trim();

                    if (string.IsNullOrEmpty(newMASV) || string.IsNullOrEmpty(newMAMM))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Mã môn mở!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        // Chỉ cập nhật MASV và MAMM (tránh cập nhật cột điểm vì bị chặn bởi VPD_DANGKY_DIEM)
                        string query = "UPDATE DANGKY SET MASV = :newMASV, MAMM = :newMAMM " +
                                       "WHERE MASV = :oldMASV AND MAMM = :oldMAMM";
                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("newMASV", newMASV));
                            cmd.Parameters.Add(new OracleParameter("newMAMM", newMAMM));
                            cmd.Parameters.Add(new OracleParameter("oldMASV", oldMASV));
                            cmd.Parameters.Add(new OracleParameter("oldMAMM", oldMAMM));
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật bản ghi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData(); // Tải lại dữ liệu
                                inputForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Không thể cập nhật bản ghi. Có thể bạn đang thao tác ngoài 14 ngày đầu học kỳ hoặc bản ghi không thỏa mãn điều kiện VPD!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancel.Click += (s, ev) => inputForm.Close();

                inputForm.Controls.AddRange(new Control[] { lblMASV, txtMASV, lblMAMM, txtMAMM, btnOK, btnCancel });
                inputForm.ShowDialog();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDangKy.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvDangKy.SelectedRows[0];
            string masv = selectedRow.Cells["MASV"].Value.ToString();
            string mamm = selectedRow.Cells["MAMM"].Value.ToString();

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa bản ghi với MASV = {masv} và MAMM = {mamm}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            try
            {
                string query = "DELETE FROM DANGKY WHERE MASV = :masv AND MAMM = :mamm";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("masv", masv));
                    cmd.Parameters.Add(new OracleParameter("mamm", mamm));
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa bản ghi thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa bản ghi. Có thể bạn đang thao tác ngoài 14 ngày đầu học kỳ hoặc bản ghi không thỏa mãn điều kiện VPD!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa bản ghi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormDangKyHocPhan_PDT_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}