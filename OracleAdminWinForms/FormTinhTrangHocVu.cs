using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormTinhTrangHocVu : Form
    {
        private OracleConnection conn;
        private string username;

        private DataTable dtSinhVien;

        public FormTinhTrangHocVu(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void FormTinhTrangHocVu_Load(object sender, EventArgs e)
        {
            LoadSinhVienData();
        }

        private void LoadSinhVienData()
        {
            try
            {
                string query = "SELECT MASV, HOTEN, PHAI, NGSINH, ĐCHI, ĐT, KHOA, TINHTRANG FROM SINHVIEN";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                dtSinhVien = new DataTable();
                adapter.Fill(dtSinhVien);
                dgvSinhVien.DataSource = dtSinhVien;

                // Chỉ cho phép chỉnh sửa cột TINHTRANG
                foreach (DataGridViewColumn col in dgvSinhVien.Columns)
                {
                    col.ReadOnly = col.Name != "TINHTRANG";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter("SELECT MASV, TINHTRANG FROM SINHVIEN", conn);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);

                // Cập nhật cột TINHTRANG
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.Update(dtSinhVien);

                MessageBox.Show("Đã cập nhật tình trạng học vụ thành công!", "Thông báo");
                LoadSinhVienData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadSinhVienData();
        }
    }
}
