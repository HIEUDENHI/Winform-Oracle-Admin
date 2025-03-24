using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormRenameRole: Form
    {
        private OracleConnection conn;
        private string oldRole;
        public FormRenameRole(OracleConnection connection, string role)
        {
            InitializeComponent();
            this.conn = connection;
            this.oldRole = role;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newRole = txtNewRole.Text.Trim();
            if (string.IsNullOrEmpty(newRole))
            {
                MessageBox.Show("Vui lòng nhập tên Role mới.");
                return;
            }

            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_ROLE_MANAGEMENT.sp_RenameRole", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_old_role", OracleDbType.Varchar2).Value = oldRole;
                    cmd.Parameters.Add("p_new_role", OracleDbType.Varchar2).Value = newRole;
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đổi tên role thành công!");
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi tên role: " + ex.Message);
            }
        }

        private void FormRenameRole_Load(object sender, EventArgs e)
        {
            lblOldRole.Text = "Role hiện tại: " + oldRole;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}
