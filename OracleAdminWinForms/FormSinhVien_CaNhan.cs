using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormSinhVien_CaNhan : Form
    {
        private OracleConnection conn;
        private string username;

        public FormSinhVien_CaNhan(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        // 💥 FIXED: Tên method trùng khớp với Designer
        private void FormSinhVienCaNhan_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Thông tin cá nhân - Sinh viên: {username}";
            LoadSinhVienData();
        }

        private void LoadSinhVienData()
        {
            try
            {
                string query = "SELECT MASV, HOTEN, PHAI, NGSINH, ĐCHI, ĐT, KHOA, TINHTRANG FROM SINHVIEN WHERE MASV = :username";
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username));
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvSinhVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để cập nhật.");
                    return;
                }

                string newDC = dgvSinhVien.Rows[0].Cells["ĐCHI"].Value?.ToString();
                string newDT = dgvSinhVien.Rows[0].Cells["ĐT"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(newDC) || string.IsNullOrWhiteSpace(newDT))
                {
                    MessageBox.Show("Địa chỉ và số điện thoại không được để trống.");
                    return;
                }

                // 💡 Gọi stored procedure ADMIN.UPDATE_SINHVIEN_INFO
                OracleCommand cmd = new OracleCommand("ADMIN.UPDATE_SINHVIEN_INFO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_masv", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("p_dc", OracleDbType.Varchar2).Value = newDC;
                cmd.Parameters.Add("p_dt", OracleDbType.Varchar2).Value = newDT;

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("✔️ Cập nhật thành công!");
                    LoadSinhVienData();
                }
                else
                {
                    MessageBox.Show("✔️ Cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadSinhVienData();
        }
    }
}
