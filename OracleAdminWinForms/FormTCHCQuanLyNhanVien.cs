using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormTCHCQuanLyNhanVien : Form
    {
        private OracleConnection conn;
        private string username;

        public FormTCHCQuanLyNhanVien(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormTCHCQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: NV TCHC";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM NHANVIEN";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvNhanVien.DataSource = dt;
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

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                try
                {
                    var row = dgvNhanVien.CurrentRow;

                    string query = @"UPDATE NHANVIEN 
                                     SET HOTEN = :hoten,
                                         PHAI = :phai,
                                         NGSINH = :ngsinh,
                                         LUONG = :luong,
                                         PHUCAP = :phucap,
                                         ĐT = :dt,
                                         VAITRO = :vaitro,
                                         MAĐV = :madv
                                     WHERE MANV = :manv";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":hoten", row.Cells["HOTEN"].Value);
                        cmd.Parameters.Add(":phai", row.Cells["PHAI"].Value);
                        cmd.Parameters.Add(":ngsinh", row.Cells["NGSINH"].Value);
                        cmd.Parameters.Add(":luong", row.Cells["LUONG"].Value);
                        cmd.Parameters.Add(":phucap", row.Cells["PHUCAP"].Value);
                        cmd.Parameters.Add(":dt", row.Cells["ĐT"].Value);
                        cmd.Parameters.Add(":vaitro", row.Cells["VAITRO"].Value);
                        cmd.Parameters.Add(":madv", row.Cells["MAĐV"].Value);
                        cmd.Parameters.Add(":manv", row.Cells["MANV"].Value);

                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show($"Đã cập nhật {result} dòng.");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"INSERT INTO NHANVIEN 
                                (MANV, HOTEN, PHAI, NGSINH, LUONG, PHUCAP, ĐT, VAITRO, MAĐV)
                                 VALUES (:manv, :hoten, :phai, :ngsinh, :luong, :phucap, :dt, :vaitro, :madv)";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    var row = dgvNhanVien.CurrentRow;
                    cmd.Parameters.Add(":manv", row.Cells["MANV"].Value);
                    cmd.Parameters.Add(":hoten", row.Cells["HOTEN"].Value);
                    cmd.Parameters.Add(":phai", row.Cells["PHAI"].Value);
                    cmd.Parameters.Add(":ngsinh", row.Cells["NGSINH"].Value);
                    cmd.Parameters.Add(":luong", row.Cells["LUONG"].Value);
                    cmd.Parameters.Add(":phucap", row.Cells["PHUCAP"].Value);
                    cmd.Parameters.Add(":dt", row.Cells["ĐT"].Value);
                    cmd.Parameters.Add(":vaitro", row.Cells["VAITRO"].Value);
                    cmd.Parameters.Add(":madv", row.Cells["MAĐV"].Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm nhân viên thành công!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                var manv = dgvNhanVien.CurrentRow.Cells["MANV"].Value.ToString();
                var result = MessageBox.Show($"Xác nhận xoá nhân viên '{manv}'?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (var cmd = new OracleCommand("DELETE FROM NHANVIEN WHERE MANV = :manv", conn))
                        {
                            cmd.Parameters.Add(":manv", manv);
                            int rows = cmd.ExecuteNonQuery();
                            MessageBox.Show($"Đã xoá {rows} dòng.");
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xoá: " + ex.Message);
                    }
                }
            }
        }
    }
}
