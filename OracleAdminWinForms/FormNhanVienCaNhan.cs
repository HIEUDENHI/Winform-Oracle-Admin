using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormNhanVienCaNhan : Form
    {
        private OracleConnection conn;
        private string username; // Mã nhân viên hiện tại
        private string vaitro;   // Vai trò người dùng hiện tại

        public FormNhanVienCaNhan(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper(); // để so sánh chính xác trong Oracle
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadVaiTro();
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: {vaitro}";
            LoadNhanVienData();
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
            catch
            {
                MessageBox.Show("Không thể xác định vai trò người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                vaitro = "NVCB";
            }
        }

        private void LoadNhanVienData()
        {
            string query = "";
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;

            if (vaitro == "NVCB")
            {
                query = "SELECT MANV, HOTEN, PHAI, NGSINH, ĐT FROM NHANVIEN WHERE MANV = :username";
                cmd.CommandText = query;
                cmd.Parameters.Add(new OracleParameter("username", username));
            }
            else if (vaitro == "TRGĐV")
            {
                query = @"SELECT MANV, HOTEN, PHAI, NGSINH, ĐT, VAITRO 
                          FROM NHANVIEN 
                          WHERE MAĐV = (
                              SELECT MAĐV FROM NHANVIEN WHERE MANV = :username
                          )";
                cmd.CommandText = query;
                cmd.Parameters.Add(new OracleParameter("username", username));
            }
            else if (vaitro == "NV TCHC")
            {
                query = "SELECT * FROM NHANVIEN";
                cmd.CommandText = query;
            }
            else
            {
                // Các vai trò khác kế thừa quyền của NVCB
                query = "SELECT MANV, HOTEN, PHAI, NGSINH, ĐT FROM NHANVIEN WHERE MANV = :username";
                cmd.CommandText = query;
                cmd.Parameters.Add(new OracleParameter("username", username));
            }

            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvNhanVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu nhân viên: " + ex.Message);
            }
        }

        private void btnCapNhatSDT_Click(object sender, EventArgs e)
        {
            string newPhone = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(newPhone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại mới.");
                return;
            }

            if (vaitro != "NVCB" && vaitro != "GV" && vaitro != "TRGĐV") // Những người khác không có quyền này
            {
                MessageBox.Show("Chỉ người dùng có vai trò NVCB hoặc kế thừa mới được phép cập nhật SDT.");
                return;
            }

            try
            {
                using (var cmd = new OracleCommand("UPDATE NHANVIEN SET ĐT = :sdt WHERE MANV = :username", conn))
                {
                    cmd.Parameters.Add("sdt", newPhone);
                    cmd.Parameters.Add("username", username);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật số điện thoại thành công!");
                        LoadNhanVienData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số điện thoại: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadNhanVienData();
        }
    }
}