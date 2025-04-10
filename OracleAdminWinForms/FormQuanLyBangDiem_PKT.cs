using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormQuanLyBangDiem_PKT : Form
    {
        private OracleConnection conn;
        private string username;

        public FormQuanLyBangDiem_PKT(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;

            lblTitle.Text = $"Quản lý điểm - {username}";
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
                dgvBangDiem.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng điểm: " + ex.Message);
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Duyệt qua các dòng trong DataGridView để cập nhật điểm
                foreach (DataGridViewRow row in dgvBangDiem.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Kiểm tra giá trị MASV và MAMM
                    string masv = row.Cells["MASV"].Value?.ToString();
                    string mamm = row.Cells["MAMM"].Value?.ToString();

                    // Kiểm tra nếu MASV hoặc MAMM là null hoặc rỗng
                    if (string.IsNullOrEmpty(masv) || string.IsNullOrEmpty(mamm))
                    {
                        MessageBox.Show("MASV hoặc MAMM không hợp lệ.");
                        return;
                    }

                    // Kiểm tra điểm (dùng tên đúng của cột)
                    object diemth = row.Cells["ĐIEMTH"].Value;
                    object diemqt = row.Cells["ĐIEMQT"].Value;
                    object diemck = row.Cells["ĐIEMCK"].Value;
                    object diemtk = row.Cells["ĐIEMTK"].Value;

                    // Kiểm tra xem các giá trị điểm có hợp lệ không
                    if (diemth == DBNull.Value || diemqt == DBNull.Value || diemck == DBNull.Value || diemtk == DBNull.Value)
                    {
                        MessageBox.Show("Điểm phải được nhập đầy đủ.");
                        return;
                    }

                    // Chuyển đổi điểm sang kiểu số để kiểm tra tính hợp lệ
                    decimal diemTHValue, diemQTValue, diemCKValue, diemTKValue;
                    if (!decimal.TryParse(diemth.ToString(), out diemTHValue) ||
                        !decimal.TryParse(diemqt.ToString(), out diemQTValue) ||
                        !decimal.TryParse(diemck.ToString(), out diemCKValue) ||
                        !decimal.TryParse(diemtk.ToString(), out diemTKValue))
                    {
                        MessageBox.Show("Điểm phải là giá trị hợp lệ.");
                        return;
                    }

                    // Cập nhật điểm vào cơ sở dữ liệu
                    string updateQuery = "UPDATE DANGKY SET ĐIEMTH = :diemth, ĐIEMQT = :diemqt, ĐIEMCK = :diemck, ĐIEMTK = :diemtk " +
                                         "WHERE MASV = :masv AND MAMM = :mamm";
                    using (OracleCommand cmd = new OracleCommand(updateQuery, conn))
                    {
                        cmd.Parameters.Add("diemth", OracleDbType.Decimal).Value = diemTHValue;
                        cmd.Parameters.Add("diemqt", OracleDbType.Decimal).Value = diemQTValue;
                        cmd.Parameters.Add("diemck", OracleDbType.Decimal).Value = diemCKValue;
                        cmd.Parameters.Add("diemtk", OracleDbType.Decimal).Value = diemTKValue;
                        cmd.Parameters.Add("masv", OracleDbType.Varchar2).Value = masv;
                        cmd.Parameters.Add("mamm", OracleDbType.Varchar2).Value = mamm;

                        cmd.ExecuteNonQuery();
                    }
                }

                // Sau khi cập nhật xong, tải lại dữ liệu
                LoadData();
                MessageBox.Show("Bảng điểm đã được cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật bảng điểm: " + ex.Message);
            }
        }



        private void FormQuanLyBangDiem_PKT_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
