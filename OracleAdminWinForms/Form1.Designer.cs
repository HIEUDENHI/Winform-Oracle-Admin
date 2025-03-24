namespace OracleAdminWinForms
{
    partial class Form1
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.btnViewPrivileges = new System.Windows.Forms.Button();
            this.btnGrantPriv = new System.Windows.Forms.Button();
            this.btnGrantRole = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnDropUser = new System.Windows.Forms.Button();
            this.btnAlterUser = new System.Windows.Forms.Button();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnLoadUsers = new System.Windows.Forms.Button();
            this.tabRoles = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDropRole = new System.Windows.Forms.Button();
            this.btnRenameRole = new System.Windows.Forms.Button();
            this.btnCreateRole = new System.Windows.Forms.Button();
            this.btnLoadRoles = new System.Windows.Forms.Button();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.tabControlMain.SuspendLayout();
            this.tabUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.tabRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabUsers);
            this.tabControlMain.Controls.Add(this.tabRoles);
            this.tabControlMain.Location = new System.Drawing.Point(12, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1061, 622);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.btnViewPrivileges);
            this.tabUsers.Controls.Add(this.btnGrantPriv);
            this.tabUsers.Controls.Add(this.btnGrantRole);
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.btnDropUser);
            this.tabUsers.Controls.Add(this.btnAlterUser);
            this.tabUsers.Controls.Add(this.btnCreateUser);
            this.tabUsers.Controls.Add(this.btnLoadUsers);
            this.tabUsers.Location = new System.Drawing.Point(8, 39);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(1045, 575);
            this.tabUsers.TabIndex = 0;
            this.tabUsers.Text = "Quản lý User";
            this.tabUsers.UseVisualStyleBackColor = true;
            this.tabUsers.Click += new System.EventHandler(this.tabUsers_Click);
            // 
            // btnViewPrivileges
            // 
            this.btnViewPrivileges.Location = new System.Drawing.Point(893, 371);
            this.btnViewPrivileges.Name = "btnViewPrivileges";
            this.btnViewPrivileges.Size = new System.Drawing.Size(149, 61);
            this.btnViewPrivileges.TabIndex = 18;
            this.btnViewPrivileges.Text = "Xem quyền";
            this.btnViewPrivileges.UseVisualStyleBackColor = true;
            this.btnViewPrivileges.Click += new System.EventHandler(this.btnViewPrivileges_Click);
            // 
            // btnGrantPriv
            // 
            this.btnGrantPriv.Location = new System.Drawing.Point(893, 257);
            this.btnGrantPriv.Name = "btnGrantPriv";
            this.btnGrantPriv.Size = new System.Drawing.Size(149, 61);
            this.btnGrantPriv.TabIndex = 17;
            this.btnGrantPriv.Text = "Cấp quyền";
            this.btnGrantPriv.UseVisualStyleBackColor = true;
            this.btnGrantPriv.Click += new System.EventHandler(this.btnGrantPriv_Click);
            // 
            // btnGrantRole
            // 
            this.btnGrantRole.Location = new System.Drawing.Point(893, 143);
            this.btnGrantRole.Name = "btnGrantRole";
            this.btnGrantRole.Size = new System.Drawing.Size(149, 61);
            this.btnGrantRole.TabIndex = 16;
            this.btnGrantRole.Text = "Cấp Role";
            this.btnGrantRole.UseVisualStyleBackColor = true;
            this.btnGrantRole.Click += new System.EventHandler(this.btnGrantRole_Click);
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(23, 105);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersWidth = 82;
            this.dgvUsers.RowTemplate.Height = 33;
            this.dgvUsers.Size = new System.Drawing.Size(864, 422);
            this.dgvUsers.TabIndex = 15;
            // 
            // btnDropUser
            // 
            this.btnDropUser.Location = new System.Drawing.Point(739, 24);
            this.btnDropUser.Name = "btnDropUser";
            this.btnDropUser.Size = new System.Drawing.Size(114, 61);
            this.btnDropUser.TabIndex = 14;
            this.btnDropUser.Text = "Xóa user";
            this.btnDropUser.UseVisualStyleBackColor = true;
            this.btnDropUser.Click += new System.EventHandler(this.btnDropUser_Click);
            // 
            // btnAlterUser
            // 
            this.btnAlterUser.Location = new System.Drawing.Point(529, 24);
            this.btnAlterUser.Name = "btnAlterUser";
            this.btnAlterUser.Size = new System.Drawing.Size(114, 61);
            this.btnAlterUser.TabIndex = 13;
            this.btnAlterUser.Text = "Sửa user";
            this.btnAlterUser.UseVisualStyleBackColor = true;
            this.btnAlterUser.Click += new System.EventHandler(this.btnAlterUser_Click);
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.Location = new System.Drawing.Point(277, 24);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(146, 61);
            this.btnCreateUser.TabIndex = 12;
            this.btnCreateUser.Text = "Tạo user mới";
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // btnLoadUsers
            // 
            this.btnLoadUsers.Location = new System.Drawing.Point(23, 24);
            this.btnLoadUsers.Name = "btnLoadUsers";
            this.btnLoadUsers.Size = new System.Drawing.Size(160, 61);
            this.btnLoadUsers.TabIndex = 11;
            this.btnLoadUsers.Text = "Tải danh sách";
            this.btnLoadUsers.UseVisualStyleBackColor = true;
            this.btnLoadUsers.Click += new System.EventHandler(this.btnLoadUsers_Click);
            // 
            // tabRoles
            // 
            this.tabRoles.Controls.Add(this.button2);
            this.tabRoles.Controls.Add(this.button1);
            this.tabRoles.Controls.Add(this.btnDropRole);
            this.tabRoles.Controls.Add(this.btnRenameRole);
            this.tabRoles.Controls.Add(this.btnCreateRole);
            this.tabRoles.Controls.Add(this.btnLoadRoles);
            this.tabRoles.Controls.Add(this.dgvRoles);
            this.tabRoles.Location = new System.Drawing.Point(8, 39);
            this.tabRoles.Name = "tabRoles";
            this.tabRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabRoles.Size = new System.Drawing.Size(1045, 575);
            this.tabRoles.TabIndex = 1;
            this.tabRoles.Text = "Quản lý Role";
            this.tabRoles.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(893, 333);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 61);
            this.button2.TabIndex = 19;
            this.button2.Text = "Xem quyền";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(893, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 61);
            this.button1.TabIndex = 18;
            this.button1.Text = "Cấp quyền";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDropRole
            // 
            this.btnDropRole.Location = new System.Drawing.Point(708, 6);
            this.btnDropRole.Name = "btnDropRole";
            this.btnDropRole.Size = new System.Drawing.Size(160, 61);
            this.btnDropRole.TabIndex = 15;
            this.btnDropRole.Text = "Xóa Role";
            this.btnDropRole.UseVisualStyleBackColor = true;
            this.btnDropRole.Click += new System.EventHandler(this.btnDropRole_Click);
            // 
            // btnRenameRole
            // 
            this.btnRenameRole.Location = new System.Drawing.Point(486, 6);
            this.btnRenameRole.Name = "btnRenameRole";
            this.btnRenameRole.Size = new System.Drawing.Size(160, 61);
            this.btnRenameRole.TabIndex = 14;
            this.btnRenameRole.Text = "Đổi tên Role";
            this.btnRenameRole.UseVisualStyleBackColor = true;
            this.btnRenameRole.Click += new System.EventHandler(this.btnRenameRole_Click);
            // 
            // btnCreateRole
            // 
            this.btnCreateRole.Location = new System.Drawing.Point(250, 6);
            this.btnCreateRole.Name = "btnCreateRole";
            this.btnCreateRole.Size = new System.Drawing.Size(178, 61);
            this.btnCreateRole.TabIndex = 13;
            this.btnCreateRole.Text = "Tạo Role";
            this.btnCreateRole.UseVisualStyleBackColor = true;
            this.btnCreateRole.Click += new System.EventHandler(this.btnCreateRole_Click);
            // 
            // btnLoadRoles
            // 
            this.btnLoadRoles.Location = new System.Drawing.Point(17, 6);
            this.btnLoadRoles.Name = "btnLoadRoles";
            this.btnLoadRoles.Size = new System.Drawing.Size(181, 61);
            this.btnLoadRoles.TabIndex = 12;
            this.btnLoadRoles.Text = "Tải danh sách";
            this.btnLoadRoles.UseVisualStyleBackColor = true;
            this.btnLoadRoles.Click += new System.EventHandler(this.btnLoadRoles_Click);
            // 
            // dgvRoles
            // 
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Location = new System.Drawing.Point(17, 91);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.RowHeadersWidth = 82;
            this.dgvRoles.RowTemplate.Height = 33;
            this.dgvRoles.Size = new System.Drawing.Size(860, 441);
            this.dgvRoles.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 654);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.tabRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabRoles;
        private System.Windows.Forms.Button btnLoadUsers;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnDropUser;
        private System.Windows.Forms.Button btnAlterUser;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Button btnDropRole;
        private System.Windows.Forms.Button btnRenameRole;
        private System.Windows.Forms.Button btnCreateRole;
        private System.Windows.Forms.Button btnLoadRoles;
        private System.Windows.Forms.Button btnGrantRole;
        private System.Windows.Forms.Button btnGrantPriv;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnViewPrivileges;
        private System.Windows.Forms.Button button2;
    }
}

