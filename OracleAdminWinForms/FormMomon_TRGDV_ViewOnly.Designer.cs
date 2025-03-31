namespace OracleAdminWinForms
{
    partial class FormMomon_TRGDV_ViewOnly
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvMomon;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvMomon = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMomon
            // 
            this.dgvMomon.AllowUserToAddRows = false;
            this.dgvMomon.AllowUserToDeleteRows = false;
            this.dgvMomon.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMomon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMomon.Location = new System.Drawing.Point(12, 50);
            this.dgvMomon.Name = "dgvMomon";
            this.dgvMomon.ReadOnly = true;
            this.dgvMomon.RowHeadersWidth = 51;
            this.dgvMomon.RowTemplate.Height = 24;
            this.dgvMomon.Size = new System.Drawing.Size(760, 350);
            this.dgvMomon.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(255, 28);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Môn giảng dạy của đơn vị";
            // 
            // FormMomon_TRGDV_ViewOnly
            // 
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvMomon);
            this.Name = "FormMomon_TRGDV_ViewOnly";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xem môn giảng dạy - Trưởng đơn vị";
            this.Load += new System.EventHandler(this.FormMomon_TRGDV_ViewOnly_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
