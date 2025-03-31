namespace OracleAdminWinForms
{
    partial class FormMomon_ReadOnly
    {
        private System.Windows.Forms.DataGridView dgvMomon;
        private System.Windows.Forms.Label lblTitle;

        private void InitializeComponent()
        {
            this.dgvMomon = new System.Windows.Forms.DataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 28);
            this.lblTitle.Text = "Môn học giảng dạy";
            // 
            // dgvMomon
            // 
            this.dgvMomon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMomon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMomon.Location = new System.Drawing.Point(20, 60);
            this.dgvMomon.Name = "dgvMomon";
            this.dgvMomon.RowTemplate.Height = 24;
            this.dgvMomon.Size = new System.Drawing.Size(750, 400);
            this.dgvMomon.TabIndex = 0;
            // 
            // FormMomon_ReadOnly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvMomon);
            this.Name = "FormMomon_ReadOnly";
            this.Text = "Xem môn giảng dạy";
            this.Load += new System.EventHandler(this.FormMomon_ReadOnly_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMomon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
