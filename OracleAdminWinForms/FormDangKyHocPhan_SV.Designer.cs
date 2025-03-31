using System.Drawing;
using System.Windows.Forms;
using System;

partial class FormDangKyHocPhan_SV
{
    private System.ComponentModel.IContainer components = null;
    private DataGridView dgvDangKy;
    private Button btnThem;
    private Button btnXoa;
    private Button btnTaiLai;
    private Label lblTitle;
    private TextBox txtMaMon;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.dgvDangKy = new DataGridView();
        this.btnThem = new Button();
        this.btnXoa = new Button();
        this.btnTaiLai = new Button();
        this.lblTitle = new Label();
        this.txtMaMon = new TextBox();

        this.SuspendLayout();

        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        this.lblTitle.Location = new Point(20, 20);
        this.lblTitle.Text = "Đăng ký học phần";

        this.dgvDangKy.Location = new Point(20, 60);
        this.dgvDangKy.Size = new Size(740, 300);
        this.dgvDangKy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        this.txtMaMon.Location = new Point(20, 370);
        this.txtMaMon.Size = new Size(200, 27);
        this.txtMaMon.Text = "Nhập mã môn...";
        this.txtMaMon.ForeColor = Color.Gray;
        this.txtMaMon.GotFocus += (s, e) =>
        {
            if (txtMaMon.Text == "Nhập mã môn...")
            {
                txtMaMon.Text = "";
                txtMaMon.ForeColor = Color.Black;
            }
        };
        this.txtMaMon.LostFocus += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtMaMon.Text))
            {
                txtMaMon.Text = "Nhập mã môn...";
                txtMaMon.ForeColor = Color.Gray;
            }
        };

        this.btnThem.Text = "Thêm";
        this.btnThem.Location = new Point(240, 370);
        this.btnThem.Click += new EventHandler(this.btnThem_Click);

        this.btnXoa.Text = "Xoá";
        this.btnXoa.Location = new Point(340, 370);
        this.btnXoa.Click += new EventHandler(this.btnXoa_Click);

        this.btnTaiLai.Text = "Tải lại";
        this.btnTaiLai.Location = new Point(440, 370);
        this.btnTaiLai.Click += new EventHandler(this.btnTaiLai_Click);

        this.ClientSize = new Size(800, 420);
        this.Controls.AddRange(new Control[] {
            lblTitle, dgvDangKy, txtMaMon, btnThem, btnXoa, btnTaiLai
        });

        this.Text = "Đăng ký học phần - Sinh viên";
        this.Load += new EventHandler(this.FormDangKyHocPhan_SV_Load);

        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
