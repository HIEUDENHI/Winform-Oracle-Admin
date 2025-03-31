namespace OracleAdminWinForms
{
    partial class FormNhanVienCaNhan
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Button btnCapNhatSDT;
        private System.Windows.Forms.Button btnTaiLai;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.dgvNhanVien = new System.Windows.Forms.DataGridView();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.btnCapNhatSDT = new System.Windows.Forms.Button();
            this.btnTaiLai = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.SuspendLayout();

            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.Location = new System.Drawing.Point(20, 15);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(200, 23);
            this.lblUserInfo.TabIndex = 0;
            this.lblUserInfo.Text = "Đăng nhập: [username]";

            // 
            // dgvNhanVien
            // 
            this.dgvNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNhanVien.Location = new System.Drawing.Point(20, 50);
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.RowHeadersWidth = 51;
            this.dgvNhanVien.RowTemplate.Height = 24;
            this.dgvNhanVien.Size = new System.Drawing.Size(740, 300);
            this.dgvNhanVien.TabIndex = 1;
            this.dgvNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Location = new System.Drawing.Point(160, 370);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(200, 22);
            this.txtSoDienThoai.TabIndex = 2;

            // 
            // btnCapNhatSDT
            // 
            this.btnCapNhatSDT.Location = new System.Drawing.Point(380, 368);
            this.btnCapNhatSDT.Name = "btnCapNhatSDT";
            this.btnCapNhatSDT.Size = new System.Drawing.Size(120, 28);
            this.btnCapNhatSDT.TabIndex = 3;
            this.btnCapNhatSDT.Text = "Cập nhật SDT";
            this.btnCapNhatSDT.UseVisualStyleBackColor = true;
            this.btnCapNhatSDT.Click += new System.EventHandler(this.btnCapNhatSDT_Click);

            // 
            // btnTaiLai
            // 
            this.btnTaiLai.Location = new System.Drawing.Point(520, 368);
            this.btnTaiLai.Name = "btnTaiLai";
            this.btnTaiLai.Size = new System.Drawing.Size(100, 28);
            this.btnTaiLai.TabIndex = 4;
            this.btnTaiLai.Text = "Tải lại";
            this.btnTaiLai.UseVisualStyleBackColor = true;
            this.btnTaiLai.Click += new System.EventHandler(this.btnTaiLai_Click);

            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.dgvNhanVien);
            this.Controls.Add(this.txtSoDienThoai);
            this.Controls.Add(this.btnCapNhatSDT);
            this.Controls.Add(this.btnTaiLai);
            this.Name = "Form2";
            this.Text = "Quản lý nhân viên";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
