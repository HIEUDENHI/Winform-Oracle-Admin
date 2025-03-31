using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormSinhVien_GV_ReadOnly : Form
    {
        private OracleConnection conn;
        private string username;

        public FormSinhVien_GV_ReadOnly(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user.ToUpper();
        }

        private void FormSinhVien_GV_ReadOnly_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Danh sách sinh viên thuộc khoa của giảng viên: {username}";
            LoadSinhVienData();
        }

        private void LoadSinhVienData()
        {
            try
            {
                string query = @"
                    SELECT MASV, HOTEN, PHAI, NGSINH, ĐCHI, ĐT, KHOA, TINHTRANG
                    FROM SINHVIEN
                    WHERE KHOA = (
                        SELECT MAĐV FROM NHANVIEN WHERE MANV = :username
                    )
                ";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;

                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadSinhVienData();
        }
    }
}
