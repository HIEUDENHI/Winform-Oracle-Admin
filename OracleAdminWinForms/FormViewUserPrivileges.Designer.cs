namespace OracleAdminWinForms
{
    partial class FormViewUserPrivileges
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvPrivileges = new System.Windows.Forms.DataGridView();
            this.btnRevokePrivilege = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPrivileges
            // 
            this.dgvPrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrivileges.Location = new System.Drawing.Point(12, 22);
            this.dgvPrivileges.Name = "dgvPrivileges";
            this.dgvPrivileges.RowHeadersWidth = 82;
            this.dgvPrivileges.RowTemplate.Height = 33;
            this.dgvPrivileges.Size = new System.Drawing.Size(653, 400);
            this.dgvPrivileges.TabIndex = 0;
            this.dgvPrivileges.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnRevokePrivilege
            // 
            this.btnRevokePrivilege.Location = new System.Drawing.Point(671, 138);
            this.btnRevokePrivilege.Name = "btnRevokePrivilege";
            this.btnRevokePrivilege.Size = new System.Drawing.Size(161, 97);
            this.btnRevokePrivilege.TabIndex = 1;
            this.btnRevokePrivilege.Text = "Thu hồi quyền";
            this.btnRevokePrivilege.UseVisualStyleBackColor = true;
            this.btnRevokePrivilege.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormViewUserPrivileges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 450);
            this.Controls.Add(this.btnRevokePrivilege);
            this.Controls.Add(this.dgvPrivileges);
            this.Name = "FormViewUserPrivileges";
            this.Text = "FormViewUserPrivileges";
            this.Load += new System.EventHandler(this.FormViewUserPrivileges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPrivileges;
        private System.Windows.Forms.Button btnRevokePrivilege;
    }
}