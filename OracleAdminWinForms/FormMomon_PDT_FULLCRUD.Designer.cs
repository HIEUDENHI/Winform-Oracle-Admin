namespace OracleAdminWinForms
{
    partial class FormMomon_PDT_FULLCRUD
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.dgvMomon = new System.Windows.Forms.DataGridView();
            this.labelMAMM = new System.Windows.Forms.Label();
            this.txtMAMM = new System.Windows.Forms.TextBox();
            this.labelMAHP = new System.Windows.Forms.Label();
            this.txtMAHP_Them = new System.Windows.Forms.TextBox();
            this.labelMAGV = new System.Windows.Forms.Label();
            this.txtMAGV_Them = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.labelMAHP_Update = new System.Windows.Forms.Label();
            this.txtMAHP_Update = new System.Windows.Forms.TextBox();
            this.labelMAGV_Update = new System.Windows.Forms.Label();
            this.txtMAGV_Update = new System.Windows.Forms.TextBox();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnTaiLai = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Location = new System.Drawing.Point(12, 9);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(100, 13);
            this.lblUserInfo.TabIndex = 0;
            this.lblUserInfo.Text = "Đăng nhập: | Vai trò:";
            // 
            // dgvMomon
            // 
            this.dgvMomon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMomon.Location = new System.Drawing.Point(12, 40);
            this.dgvMomon.Name = "dgvMomon";
            this.dgvMomon.Size = new System.Drawing.Size(760, 150);
            this.dgvMomon.TabIndex = 1;
            this.dgvMomon.SelectionChanged += new System.EventHandler(this.dgvMomon_SelectionChanged);
            // 
            // labelMAMM
            // 
            this.labelMAMM.AutoSize = true;
            this.labelMAMM.Location = new System.Drawing.Point(12, 200);
            this.labelMAMM.Name = "labelMAMM";
            this.labelMAMM.Size = new System.Drawing.Size(41, 13);
            this.labelMAMM.TabIndex = 2;
            this.labelMAMM.Text = "MAMM:";
            // 
            // txtMAMM
            // 
            this.txtMAMM.Location = new System.Drawing.Point(59, 197);
            this.txtMAMM.Name = "txtMAMM";
            this.txtMAMM.Size = new System.Drawing.Size(100, 20);
            this.txtMAMM.TabIndex = 3;
            // 
            // labelMAHP
            // 
            this.labelMAHP.AutoSize = true;
            this.labelMAHP.Location = new System.Drawing.Point(165, 200);
            this.labelMAHP.Name = "labelMAHP";
            this.labelMAHP.Size = new System.Drawing.Size(38, 13);
            this.labelMAHP.TabIndex = 4;
            this.labelMAHP.Text = "MAHP:";
            // 
            // txtMAHP_Them
            // 
            this.txtMAHP_Them.Location = new System.Drawing.Point(209, 197);
            this.txtMAHP_Them.Name = "txtMAHP_Them";
            this.txtMAHP_Them.Size = new System.Drawing.Size(100, 20);
            this.txtMAHP_Them.TabIndex = 5;
            // 
            // labelMAGV
            // 
            this.labelMAGV.AutoSize = true;
            this.labelMAGV.Location = new System.Drawing.Point(315, 200);
            this.labelMAGV.Name = "labelMAGV";
            this.labelMAGV.Size = new System.Drawing.Size(38, 13);
            this.labelMAGV.TabIndex = 6;
            this.labelMAGV.Text = "MAGV:";
            // 
            // txtMAGV_Them
            // 
            this.txtMAGV_Them.Location = new System.Drawing.Point(359, 197);
            this.txtMAGV_Them.Name = "txtMAGV_Them";
            this.txtMAGV_Them.Size = new System.Drawing.Size(100, 20);
            this.txtMAGV_Them.TabIndex = 7;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(465, 197);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 23);
            this.btnThem.TabIndex = 8;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // labelMAHP_Update
            // 
            this.labelMAHP_Update.AutoSize = true;
            this.labelMAHP_Update.Location = new System.Drawing.Point(165, 230);
            this.labelMAHP_Update.Name = "labelMAHP_Update";
            this.labelMAHP_Update.Size = new System.Drawing.Size(38, 13);
            this.labelMAHP_Update.TabIndex = 9;
            this.labelMAHP_Update.Text = "MAHP:";
            // 
            // txtMAHP_Update
            // 
            this.txtMAHP_Update.Location = new System.Drawing.Point(209, 227);
            this.txtMAHP_Update.Name = "txtMAHP_Update";
            this.txtMAHP_Update.Size = new System.Drawing.Size(100, 20);
            this.txtMAHP_Update.TabIndex = 10;
            // 
            // labelMAGV_Update
            // 
            this.labelMAGV_Update.AutoSize = true;
            this.labelMAGV_Update.Location = new System.Drawing.Point(315, 230);
            this.labelMAGV_Update.Name = "labelMAGV_Update";
            this.labelMAGV_Update.Size = new System.Drawing.Size(38, 13);
            this.labelMAGV_Update.TabIndex = 11;
            this.labelMAGV_Update.Text = "MAGV:";
            // 
            // txtMAGV_Update
            // 
            this.txtMAGV_Update.Location = new System.Drawing.Point(359, 227);
            this.txtMAGV_Update.Name = "txtMAGV_Update";
            this.txtMAGV_Update.Size = new System.Drawing.Size(100, 20);
            this.txtMAGV_Update.TabIndex = 12;
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Location = new System.Drawing.Point(465, 227);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(75, 23);
            this.btnCapNhat.TabIndex = 13;
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(12, 257);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 23);
            this.btnXoa.TabIndex = 14;
            this.btnXoa.Text = "Xoá";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnTaiLai
            // 
            this.btnTaiLai.Location = new System.Drawing.Point(100, 257);
            this.btnTaiLai.Name = "btnTaiLai";
            this.btnTaiLai.Size = new System.Drawing.Size(75, 23);
            this.btnTaiLai.TabIndex = 15;
            this.btnTaiLai.Text = "Tải lại";
            this.btnTaiLai.UseVisualStyleBackColor = true;
            this.btnTaiLai.Click += new System.EventHandler(this.btnTaiLai_Click);
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(697, 257);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(75, 23);
            this.btnDong.TabIndex = 16;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FormMomon_PDT_FULLCRUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 291);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.btnTaiLai);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnCapNhat);
            this.Controls.Add(this.txtMAGV_Update);
            this.Controls.Add(this.labelMAGV_Update);
            this.Controls.Add(this.txtMAHP_Update);
            this.Controls.Add(this.labelMAHP_Update);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.txtMAGV_Them);
            this.Controls.Add(this.labelMAGV);
            this.Controls.Add(this.txtMAHP_Them);
            this.Controls.Add(this.labelMAHP);
            this.Controls.Add(this.txtMAMM);
            this.Controls.Add(this.labelMAMM);
            this.Controls.Add(this.dgvMomon);
            this.Controls.Add(this.lblUserInfo);
            this.Name = "FormMomon_PDT_FULLCRUD";
            this.Text = "Quản Lý Môn Mở (NV PĐT)";
            this.Load += new System.EventHandler(this.FormMomon_PDT_FULLCRUD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.DataGridView dgvMomon;
        private System.Windows.Forms.Label labelMAMM;
        private System.Windows.Forms.TextBox txtMAMM;
        private System.Windows.Forms.Label labelMAHP;
        private System.Windows.Forms.TextBox txtMAHP_Them;
        private System.Windows.Forms.Label labelMAGV;
        private System.Windows.Forms.TextBox txtMAGV_Them;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Label labelMAHP_Update;
        private System.Windows.Forms.TextBox txtMAHP_Update;
        private System.Windows.Forms.Label labelMAGV_Update;
        private System.Windows.Forms.TextBox txtMAGV_Update;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnTaiLai;
        private System.Windows.Forms.Button btnDong;
    }
}