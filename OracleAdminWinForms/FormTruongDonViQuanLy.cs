using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormTruongDonViQuanLy : Form
    {
        private OracleConnection conn;
        private string username;

        public FormTruongDonViQuanLy(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormTruongDonViQuanLy_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: TRGĐV";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM VW_NHANVIEN_TRGDV";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvNhanVien.DataSource = dt;

                // Ẩn cột LUONG và PHUCAP nếu có (phòng trường hợp view chưa filter sẵn)
                if (dt.Columns.Contains("LUONG"))
                    dgvNhanVien.Columns["LUONG"].Visible = false;
                if (dt.Columns.Contains("PHUCAP"))
                    dgvNhanVien.Columns["PHUCAP"].Visible = false;
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

        private void btnCapNhatSDT_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                try
                {
                    string selectedManld = dgvNhanVien.CurrentRow.Cells["MANLD"].Value.ToString();
                    if (!selectedManld.Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Bạn chỉ được cập nhật số điện thoại của chính mình.");
                        return;
                    }

                    string newPhone = txtSoDienThoai.Text.Trim();
                    if (string.IsNullOrEmpty(newPhone))
                    {
                        MessageBox.Show("Vui lòng nhập số điện thoại mới.");
                        return;
                    }

                    string query = "UPDATE NHANVIEN SET ĐT = :sdt WHERE MANLD = :manld";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(":sdt", newPhone);
                        cmd.Parameters.Add(":manld", username);
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show("Đã cập nhật " + result + " dòng.");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message);
                }
            }
        }
    }
}
