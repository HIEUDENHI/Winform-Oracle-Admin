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
    public partial class Form1: Form
    {
        private OracleConnection conn;

        public Form1(OracleConnection connection)
        {
            InitializeComponent();
            conn = connection;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Có thể kiểm tra lại kết nối tại đây hoặc thực hiện các thao tác khởi tạo
            if (conn != null && conn.State == ConnectionState.Open)
            {
                MessageBox.Show("Kết nối Oracle đã được xác thực qua Login!");
            }


        }

        private void btnLoadUsers_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_USER_MANAGEMENT.sp_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Thủ tục có 1 tham số OUT (ref cursor):
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        dgvUsers.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách User: " + ex.Message);
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            // Mở FormCreateUser, truyền kết nối
            using (FormCreateUser frm = new FormCreateUser(conn))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Sau khi tạo user thành công -> refresh danh sách
                    btnLoadUsers_Click(null, null);
                }
            }
        }

        private void btnAlterUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                // Lấy username ở cột "USERNAME" (chú ý cột này phải trùng tên cột trả về)
                string username = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();

                // Mở form đổi pass
                using (FormAlterUser frm = new FormAlterUser(conn, username))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh list
                        btnLoadUsers_Click(null, null);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 user để sửa.");
            }
        }

        private void btnDropUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow != null)
            {
                string username = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();

                DialogResult dr = MessageBox.Show(
                    $"Bạn có chắc chắn xóa user '{username}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        using (OracleCommand cmd = new OracleCommand("PKG_USER_MANAGEMENT.sp_DropUser", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show($"User '{username}' đã bị xóa.");
                        btnLoadUsers_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa user: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn 1 user để xóa.");
            }
        }

        private void btnLoadRoles_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand("PKG_ROLE_MANAGEMENT.sp_GetAllRoles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số OUT kiểu RefCursor
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dgvRoles.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách role: " + ex.Message);
            }
        }

        private void btnCreateRole_Click(object sender, EventArgs e)
        {
            using (FormCreateRole frm = new FormCreateRole(conn))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    btnLoadRoles_Click(null, null); // Refresh danh sách role
                }
            }
        }

        private void btnRenameRole_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow != null)
            {
                // Giả sử cột tên role có tên "ROLE"
                string oldRole = dgvRoles.CurrentRow.Cells["ROLE"].Value.ToString();
                using (FormRenameRole frm = new FormRenameRole(conn, oldRole))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        btnLoadRoles_Click(null, null); // Refresh danh sách role
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn role cần đổi tên!");
            }
        }

        private void btnDropRole_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow != null)
            {
                string roleName = dgvRoles.CurrentRow.Cells["ROLE"].Value.ToString();
                DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa role '{roleName}'?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        using (OracleCommand cmd = new OracleCommand("PKG_ROLE_MANAGEMENT.sp_DropRole", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("p_role_name", OracleDbType.Varchar2).Value = roleName;
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Đã xóa role thành công!");
                        btnLoadRoles_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa role: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn role để xóa!");
            }
        }

        private void btnGrantRole_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn 1 user để cấp role!");
                return;
            }

            string username = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();
            using (FormGrantRoleToUser frm = new FormGrantRoleToUser(conn, username))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Sau khi cấp role, có thể refresh quyền của user hoặc chỉ thông báo
                }
            }
        }

        private void btnGrantPriv_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có user được chọn trong DataGridView hoặc bất kỳ thông tin cần thiết.
            if (dgvUsers.CurrentRow != null)
            {
                // Lấy username từ DataGridView, giả sử cột chứa tên người dùng là "USERNAME"
                string username = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();

                // Mở form cấp quyền và truyền thông tin (kết nối và username)
                using (FormGrantPrivilege frm = new FormGrantPrivilege(conn, username))
                {
                    frm.ShowDialog();  // Hiển thị form cấp quyền
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một user để cấp quyền!");
            }
        }

        private void btnViewPrivileges_Click(object sender, EventArgs e)
        {

            if (dgvUsers.CurrentRow != null)
            {
                string selectedUser = dgvUsers.CurrentRow.Cells["USERNAME"].Value.ToString();
                FormViewUserPrivileges frm = new FormViewUserPrivileges(conn, selectedUser);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Hãy chọn một user để xem quyền!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow != null)
            {
                string selectedRole = dgvRoles.CurrentRow.Cells["ROLE"].Value.ToString();
                FormViewRolePrivileges frm = new FormViewRolePrivileges(conn, selectedRole);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Hãy chọn một role để xem quyền!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow != null)
            {
                string roleName = dgvRoles.CurrentRow.Cells["ROLE"].Value.ToString();
                using (FormGrantRolePrivilege frm = new FormGrantRolePrivilege(conn, roleName))
                {
                    frm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một role để cấp quyền!");
            }
        }

        private void tabUsers_Click(object sender, EventArgs e)
        {

        }
    }
}
