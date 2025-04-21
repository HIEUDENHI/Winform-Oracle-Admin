using System;
using System.Data;
using System.Diagnostics;
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
                // Kiểm tra kết nối
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();  // Mở kết nối nếu chưa mở
                    Debug.WriteLine("Connection opened in btnCapNhat_Click");
                }

                // Kiểm tra xem có dòng nào được chọn không
                if (dgvBangDiem.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một dòng để cập nhật điểm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool hasUpdated = false; // Biến để kiểm tra xem có dòng nào được cập nhật không

                // Duyệt qua các dòng được chọn trong DataGridView
                foreach (DataGridViewRow row in dgvBangDiem.SelectedRows)
                {
                    string masv = row.Cells["MASV"].Value?.ToString();
                    string mamm = row.Cells["MAMM"].Value?.ToString();

                    if (string.IsNullOrEmpty(masv) || string.IsNullOrEmpty(mamm))
                    {
                        MessageBox.Show($"Dòng với MASV = {masv} và MAMM = {mamm} không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Lấy giá trị điểm từ các cột
                    string diemthStr = row.Cells["ĐIEMTH"].Value?.ToString();
                    string diemqtStr = row.Cells["ĐIEMQT"].Value?.ToString();
                    string diemckStr = row.Cells["ĐIEMCK"].Value?.ToString();
                    string diemtkStr = row.Cells["ĐIEMTK"].Value?.ToString();

                    if (string.IsNullOrEmpty(diemthStr) || string.IsNullOrEmpty(diemqtStr) ||
                        string.IsNullOrEmpty(diemckStr) || string.IsNullOrEmpty(diemtkStr))
                    {
                        MessageBox.Show($"Dòng với MASV = {masv} và MAMM = {mamm}: Điểm phải được nhập đầy đủ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(diemthStr, out decimal diemTHValue) ||
                        !decimal.TryParse(diemqtStr, out decimal diemQTValue) ||
                        !decimal.TryParse(diemckStr, out decimal diemCKValue) ||
                        !decimal.TryParse(diemtkStr, out decimal diemTKValue))
                    {
                        MessageBox.Show($"Dòng với MASV = {masv} và MAMM = {mamm}: Điểm phải là giá trị hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            hasUpdated = true;
                        }
                    }
                }

                LoadData();

                if (hasUpdated)
                {
                    MessageBox.Show("Bảng điểm đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có dòng nào được cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật bảng điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FormQuanLyBangDiem_PKT_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}