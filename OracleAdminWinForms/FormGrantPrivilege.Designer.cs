namespace OracleAdminWinForms
{
    partial class FormGrantPrivilege
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
            this.cbSchema = new System.Windows.Forms.ComboBox();
            this.cbObjectName = new System.Windows.Forms.ComboBox();
            this.cbObjectPrivilege = new System.Windows.Forms.ComboBox();
            this.chkWithGrantOption = new System.Windows.Forms.CheckBox();
            this.cbColumns = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbSchema
            // 
            this.cbSchema.FormattingEnabled = true;
            this.cbSchema.Location = new System.Drawing.Point(239, 72);
            this.cbSchema.Name = "cbSchema";
            this.cbSchema.Size = new System.Drawing.Size(275, 33);
            this.cbSchema.TabIndex = 0;
            this.cbSchema.SelectedIndexChanged += new System.EventHandler(this.cbSchema_SelectedIndexChanged);
            // 
            // cbObjectName
            // 
            this.cbObjectName.FormattingEnabled = true;
            this.cbObjectName.Location = new System.Drawing.Point(239, 243);
            this.cbObjectName.Name = "cbObjectName";
            this.cbObjectName.Size = new System.Drawing.Size(275, 33);
            this.cbObjectName.TabIndex = 1;
            this.cbObjectName.SelectedIndexChanged += new System.EventHandler(this.cbObjectName_SelectedIndexChanged);
            // 
            // cbObjectPrivilege
            // 
            this.cbObjectPrivilege.FormattingEnabled = true;
            this.cbObjectPrivilege.Location = new System.Drawing.Point(239, 319);
            this.cbObjectPrivilege.Name = "cbObjectPrivilege";
            this.cbObjectPrivilege.Size = new System.Drawing.Size(275, 33);
            this.cbObjectPrivilege.TabIndex = 2;
            this.cbObjectPrivilege.SelectedIndexChanged += new System.EventHandler(this.cbObjectPrivilege_SelectedIndexChanged);
            // 
            // chkWithGrantOption
            // 
            this.chkWithGrantOption.AutoSize = true;
            this.chkWithGrantOption.Location = new System.Drawing.Point(239, 398);
            this.chkWithGrantOption.Name = "chkWithGrantOption";
            this.chkWithGrantOption.Size = new System.Drawing.Size(150, 29);
            this.chkWithGrantOption.TabIndex = 3;
            this.chkWithGrantOption.Text = "checkBox1";
            this.chkWithGrantOption.UseVisualStyleBackColor = true;
            // 
            // cbColumns
            // 
            this.cbColumns.FormattingEnabled = true;
            this.cbColumns.Location = new System.Drawing.Point(682, 94);
            this.cbColumns.Name = "cbColumns";
            this.cbColumns.Size = new System.Drawing.Size(312, 172);
            this.cbColumns.TabIndex = 4;
            this.cbColumns.SelectedIndexChanged += new System.EventHandler(this.cbColumns_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(682, 446);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(121, 67);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "button1";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(860, 446);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 67);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "button2";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbObjectType
            // 
            this.cbObjectType.FormattingEnabled = true;
            this.cbObjectType.Location = new System.Drawing.Point(239, 154);
            this.cbObjectType.Name = "cbObjectType";
            this.cbObjectType.Size = new System.Drawing.Size(275, 33);
            this.cbObjectType.TabIndex = 7;
            this.cbObjectType.SelectedIndexChanged += new System.EventHandler(this.cbObjectType_SelectedIndexChanged);
            // 
            // FormGrantPrivilege
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 578);
            this.Controls.Add(this.cbObjectType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbColumns);
            this.Controls.Add(this.chkWithGrantOption);
            this.Controls.Add(this.cbObjectPrivilege);
            this.Controls.Add(this.cbObjectName);
            this.Controls.Add(this.cbSchema);
            this.Name = "FormGrantPrivilege";
            this.Text = "FormGrantPrivilege";
            this.Load += new System.EventHandler(this.FormGrantPrivilege_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSchema;
        private System.Windows.Forms.ComboBox cbObjectName;
        private System.Windows.Forms.ComboBox cbObjectPrivilege;
        private System.Windows.Forms.CheckBox chkWithGrantOption;
        private System.Windows.Forms.CheckedListBox cbColumns;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbObjectType;
    }
}