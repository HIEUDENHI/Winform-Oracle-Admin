namespace OracleAdminWinForms
{
    partial class FormDangKyHocPhan_PDT
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvDangKy;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnTaiLai;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvDangKy = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnTaiLai = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDangKy)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Đăng ký học phần - NV PĐT: [username]";

            // dgvDangKy
            this.dgvDangKy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDangKy.Location = new System.Drawing.Point(20, 50);
            this.dgvDangKy.Name = "dgvDangKy";
            this.dgvDangKy.RowHeadersWidth = 51;
            this.dgvDangKy.RowTemplate.Height = 24;
            this.dgvDangKy.Size = new System.Drawing.Size(900, 350);
            this.dgvDangKy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // btnTaiLai
            this.btnTaiLai.Location = new System.Drawing.Point(20, 420);
            this.btnTaiLai.Name = "btnTaiLai";
            this.btnTaiLai.Size = new System.Drawing.Size(100, 30);
            this.btnTaiLai.Text = "Tải lại";
            this.btnTaiLai.UseVisualStyleBackColor = true;
            this.btnTaiLai.Click += new System.EventHandler(this.btnTaiLai_Click);

            // btnThem
            this.btnThem.Location = new System.Drawing.Point(140, 420);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(100, 30);
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            // btnCapNhat
            this.btnCapNhat.Location = new System.Drawing.Point(260, 420);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(100, 30);
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);

            // btnXoa
            this.btnXoa.Location = new System.Drawing.Point(380, 420);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 30);
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            // FormDangKyHocPhan_PDT
            this.ClientSize = new System.Drawing.Size(950, 480);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvDangKy);
            this.Controls.Add(this.btnTaiLai);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.btnCapNhat);
            this.Controls.Add(this.btnXoa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormDangKyHocPhan_PDT";
            this.Text = "Đăng ký học phần - PĐT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormDangKyHocPhan_PDT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDangKy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}