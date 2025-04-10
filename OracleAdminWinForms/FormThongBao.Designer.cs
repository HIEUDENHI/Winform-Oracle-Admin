namespace OracleAdminWinForms
{
    partial class FormThongBao
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvThongBao;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnTaiLai;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvThongBao = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTaiLai = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBao)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvThongBao
            // 
            this.dgvThongBao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongBao.Location = new System.Drawing.Point(12, 60);
            this.dgvThongBao.Name = "dgvThongBao";
            this.dgvThongBao.RowHeadersWidth = 51;
            this.dgvThongBao.RowTemplate.Height = 24;
            this.dgvThongBao.Size = new System.Drawing.Size(760, 360);
            this.dgvThongBao.TabIndex = 0;

            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(500, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Thông báo dành cho:";

            // 
            // btnTaiLai
            // 
            this.btnTaiLai.Location = new System.Drawing.Point(660, 20);
            this.btnTaiLai.Name = "btnTaiLai";
            this.btnTaiLai.Size = new System.Drawing.Size(100, 30);
            this.btnTaiLai.TabIndex = 2;
            this.btnTaiLai.Text = "Tải lại";
            this.btnTaiLai.UseVisualStyleBackColor = true;
            this.btnTaiLai.Click += new System.EventHandler(this.btnTaiLai_Click);

            // 
            // FormThongBao
            // 
            this.ClientSize = new System.Drawing.Size(784, 450);
            this.Controls.Add(this.btnTaiLai);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvThongBao);
            this.Name = "FormThongBao";
            this.Text = "Thông báo";
            this.Load += new System.EventHandler(this.FormThongBao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongBao)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
