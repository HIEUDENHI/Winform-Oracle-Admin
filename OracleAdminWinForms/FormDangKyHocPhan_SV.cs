using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Windows.Forms;
using System;

public partial class FormDangKyHocPhan_SV : Form
{
    private OracleConnection conn;
    private string username;

    public FormDangKyHocPhan_SV(OracleConnection connection, string username)
    {
        InitializeComponent();
        conn = connection;
        this.username = username;
    }

    private void FormDangKyHocPhan_SV_Load(object sender, EventArgs e)
    {
        lblTitle.Text = $"Đăng ký học phần - {username}";
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            string query = "SELECT * FROM DANGKY WHERE MASV = :masv";
            OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.Add("masv", OracleDbType.Varchar2).Value = username;

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvDangKy.DataSource = dt;
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Lỗi tải dữ liệu: " + ex.Message);
        }
    }

    private void btnThem_Click(object sender, EventArgs e)
    {
        try
        {
            string mamm = txtMaMon.Text.Trim().ToUpper();
            string query = "INSERT INTO DANGKY (MASV, MAMM) VALUES (:masv, :mamm)";
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add("masv", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("mamm", OracleDbType.Varchar2).Value = mamm;
                cmd.ExecuteNonQuery();
            }

            LoadData();
            MessageBox.Show("✅ Đã thêm đăng ký!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Lỗi thêm đăng ký: " + ex.Message);
        }
    }

    private void btnXoa_Click(object sender, EventArgs e)
    {
        if (dgvDangKy.CurrentRow == null) return;
        string mamm = dgvDangKy.CurrentRow.Cells["MAMM"].Value.ToString();

        try
        {
            string query = "DELETE FROM DANGKY WHERE MASV = :masv AND MAMM = :mamm";
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add("masv", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("mamm", OracleDbType.Varchar2).Value = mamm;
                cmd.ExecuteNonQuery();
            }

            LoadData();
            MessageBox.Show("✅ Đã xoá đăng ký!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Lỗi xoá: " + ex.Message);
        }
    }

    private void btnCapNhat_Click(object sender, EventArgs e)
    {
        if (dgvDangKy.CurrentRow == null) return;
        string mammCu = dgvDangKy.CurrentRow.Cells["MAMM"].Value.ToString();
        string mammMoi = txtMaMon.Text.Trim().ToUpper();

        try
        {
            string query = "UPDATE DANGKY SET MAMM = :mammMoi WHERE MASV = :masv AND MAMM = :mammCu";
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add("mammMoi", OracleDbType.Varchar2).Value = mammMoi;
                cmd.Parameters.Add("masv", OracleDbType.Varchar2).Value = username;
                cmd.Parameters.Add("mammCu", OracleDbType.Varchar2).Value = mammCu;
                cmd.ExecuteNonQuery();
            }

            LoadData();
            MessageBox.Show("✅ Đã cập nhật đăng ký!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("❌ Lỗi cập nhật: " + ex.Message);
        }
    }

    private void btnTaiLai_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}