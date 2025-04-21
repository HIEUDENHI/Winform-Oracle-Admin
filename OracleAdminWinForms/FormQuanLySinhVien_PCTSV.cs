using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormQuanLySinhVien_PCTSV : Form
    {
        private OracleConnection conn;
        private string username;

        public FormQuanLySinhVien_PCTSV(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void FormQuanLySinhVien_PCTSV_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Quản lý sinh viên - {username}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT MASV, HOTEN, PHAI, NGSINH, ĐCHI, ĐT, KHOA, TINHTRANG FROM SINHVIEN";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Thêm một dòng trống cuối cùng để người dùng có thể nhập mới
                dt.Rows.Add(dt.NewRow());

                dgvSinhVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();  // Mở kết nối nếu chưa mở
                    Debug.WriteLine("Connection opened in btnThem_Click");
                }

                // Kiểm tra xem MASV có trống không
                string masv = dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["MASV"].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(masv))
                {
                    MessageBox.Show("Nhập thông tin sinh viên vào dòng trống trước khi nhấn 'Thêm'.");
                    return;
                }

                string checkQuery = "SELECT COUNT(*) FROM SINHVIEN WHERE MASV = :masv";
                using (var checkCmd = new OracleCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.Add(":masv", masv);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("❗ Mã sinh viên đã tồn tại.");
                        return;
                    }
                }

                string sql = @"INSERT INTO SINHVIEN (MASV, HOTEN, PHAI, NGSINH, ĐCHI, ĐT, KHOA)
                       VALUES (:MASV, :HOTEN, :PHAI, :NGSINH, :DCHI, :DT, :KHOA)";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":MASV", masv);
                    cmd.Parameters.Add(":HOTEN", dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["HOTEN"].Value?.ToString());
                    cmd.Parameters.Add(":PHAI", dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["PHAI"].Value?.ToString());
                    cmd.Parameters.Add(":NGSINH", Convert.ToDateTime(dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["NGSINH"].Value));
                    cmd.Parameters.Add(":DCHI", dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["ĐCHI"].Value?.ToString());
                    cmd.Parameters.Add(":DT", dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["ĐT"].Value?.ToString());
                    cmd.Parameters.Add(":KHOA", dgvSinhVien.Rows[dgvSinhVien.Rows.Count - 1].Cells["KHOA"].Value?.ToString());

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("✔️ Thêm sinh viên thành công!");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sinh viên: " + ex.Message);
            }
        }


        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.CurrentRow == null || dgvSinhVien.CurrentRow.IsNewRow) return;

                DataGridViewRow row = dgvSinhVien.CurrentRow;
                string masv = row.Cells["MASV"].Value?.ToString()?.Trim();

                if (string.IsNullOrEmpty(masv))
                {
                    MessageBox.Show("Không thể cập nhật vì MASV trống.");
                    return;
                }

                string sql = @"UPDATE SINHVIEN
                               SET HOTEN = :HOTEN,
                                   PHAI = :PHAI,
                                   NGSINH = :NGSINH,
                                   ĐCHI = :DCHI,
                                   ĐT = :DT,
                                   KHOA = :KHOA
                             WHERE MASV = :MASV";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(":HOTEN", row.Cells["HOTEN"].Value?.ToString());
                    cmd.Parameters.Add(":PHAI", row.Cells["PHAI"].Value?.ToString());
                    cmd.Parameters.Add(":NGSINH", Convert.ToDateTime(row.Cells["NGSINH"].Value));
                    cmd.Parameters.Add(":DCHI", row.Cells["ĐCHI"].Value?.ToString());
                    cmd.Parameters.Add(":DT", row.Cells["ĐT"].Value?.ToString());
                    cmd.Parameters.Add(":KHOA", row.Cells["KHOA"].Value?.ToString());
                    cmd.Parameters.Add(":MASV", masv);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("✔️ Cập nhật thành công!");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật sinh viên: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.CurrentRow == null || dgvSinhVien.CurrentRow.IsNewRow) return;

                DataGridViewRow row = dgvSinhVien.CurrentRow;
                string masv = row.Cells["MASV"].Value?.ToString()?.Trim();

                if (string.IsNullOrEmpty(masv))
                {
                    MessageBox.Show("Không thể xóa sinh viên vì mã trống.");
                    return;
                }

                DialogResult confirm = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {masv}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes) return;

                using (var cmd = new OracleCommand("DELETE FROM SINHVIEN WHERE MASV = :MASV", conn))
                {
                    cmd.Parameters.Add(":MASV", masv);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("🗑️ Đã xóa sinh viên.");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sinh viên: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}