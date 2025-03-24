namespace OracleAdminWinForms
{
    partial class FormViewRolePrivileges
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
            this.dgvRolePrivileges = new System.Windows.Forms.DataGridView();
            this.btnRevokePrivilege = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRolePrivileges)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRolePrivileges
            // 
            this.dgvRolePrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRolePrivileges.Location = new System.Drawing.Point(27, 26);
            this.dgvRolePrivileges.Name = "dgvRolePrivileges";
            this.dgvRolePrivileges.RowHeadersWidth = 82;
            this.dgvRolePrivileges.RowTemplate.Height = 33;
            this.dgvRolePrivileges.Size = new System.Drawing.Size(626, 412);
            this.dgvRolePrivileges.TabIndex = 0;
            // 
            // btnRevokePrivilege
            // 
            this.btnRevokePrivilege.Location = new System.Drawing.Point(659, 172);
            this.btnRevokePrivilege.Name = "btnRevokePrivilege";
            this.btnRevokePrivilege.Size = new System.Drawing.Size(129, 74);
            this.btnRevokePrivilege.TabIndex = 1;
            this.btnRevokePrivilege.Text = "Thu hồi quyền";
            this.btnRevokePrivilege.UseVisualStyleBackColor = true;
            this.btnRevokePrivilege.Click += new System.EventHandler(this.btnRevokePrivilege_Click);
            // 
            // FormViewRolePrivileges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRevokePrivilege);
            this.Controls.Add(this.dgvRolePrivileges);
            this.Name = "FormViewRolePrivileges";
            this.Text = "FormViewRolePrivileges";
            this.Load += new System.EventHandler(this.FormViewRolePrivileges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRolePrivileges)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvRolePrivileges;
        private System.Windows.Forms.Button btnRevokePrivilege;
    }
}