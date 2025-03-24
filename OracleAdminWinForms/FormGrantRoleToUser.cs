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
    public partial class FormGrantRoleToUser: Form
    {
        private OracleConnection conn;
        private string username;
        public FormGrantRoleToUser(OracleConnection connection, string user)
        {
            InitializeComponent();
            conn = connection;
            username = user;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboRoles.SelectedItem == null)
            {
                MessageBox.Show("Please select a role to grant.");
                return;
            }

            string selectedRole = cboRoles.SelectedItem.ToString();

            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_USER_ROLES.sp_GrantRoleToUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_role", OracleDbType.Varchar2).Value = selectedRole;
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Role '{selectedRole}' granted to user '{username}' successfully.");
                this.DialogResult = DialogResult.OK; // Close the form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error granting role: " + ex.Message);
            }

        }

        private void FormGrantRoleToUser_Load(object sender, EventArgs e)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("SELECT ROLE FROM DBA_ROLES", conn))
                {
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Assuming you're using ComboBox
                        cboRoles.Items.Add(reader["ROLE"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles: " + ex.Message);
            }


        }
    }
}
