using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleAdminWinForms
{
    public partial class FormMain : Form
    {
        private OracleConnection conn;
        private string username;
        private string role;

        public FormMain(OracleConnection connection, string username, string role)
        {
            InitializeComponent();
            this.conn = connection;
            this.username = username;
            this.role = role;

            lblUserInfo.Text = $"Đăng nhập: {username} | Vai trò: {role}";
            LoadSidebarByRole(role);
        }

        private void LoadSidebarByRole(string role)
        {
            sidebarPanel.Controls.Clear();

            if (role == "NVCB" || role == "GV" || role == "TRGĐV" || role == "NV PKT")
            {
                AddSidebarButton("Thông tin cá nhân", () =>
                {
                    LoadForm(new FormNhanVienCaNhan(conn, username));
                });
            }

            if (role == "NV TCHC")
            {
                AddSidebarButton("Quản lý nhân viên", () =>
                {
                    LoadForm(new FormTCHCQuanLyNhanVien(conn, username));
                });
            }

            if (role == "NV PĐT")
            {
                AddSidebarButton("Thông tin cá nhân", () =>
                {
                    LoadForm(new FormNhanVienCaNhan(conn, username));
                });
                AddSidebarButton("Quản lý môn học", () =>
                {
                    LoadForm(new FormMomon_PDT_FULLCRUD(conn, username));
                });
                AddSidebarButton("Tình trạng học vụ", () =>
                {
                    LoadForm(new FormTinhTrangHocVu(conn, username));
                });
                AddSidebarButton("Đăng ký học phần", () =>
                {
                    LoadForm(new FormDangKyHocPhan_PDT(conn, username));
                });
            }

            if (role == "GV" || role == "TRGĐV")
            {
                AddSidebarButton("Thông tin giảng dạy", () =>
                {
                    LoadForm(new FormMomon_ReadOnly(conn, username, role));
                });
                AddSidebarButton("Sinh viên khoa tôi", () =>
                {
                    LoadForm(new FormSinhVien_GV_ReadOnly(conn, username));
                });
                AddSidebarButton("Bảng điểm lớp dạy", () =>
                {
                    LoadForm(new FormBangDiem_GiangVien(conn));
                });
            }



            if (role == "SV")
            {
                AddSidebarButton("Thông tin cá nhân", () =>
                {
                    LoadForm(new FormSinhVien_CaNhan(conn, username));
                });

                AddSidebarButton("Lịch học theo khoa", () =>
                {
                    LoadForm(new FormMomon_SV_ReadOnly(conn, username));
                });

                AddSidebarButton("Đăng ký học phần", () =>
                {
                    LoadForm(new FormDangKyHocPhan_SV(conn, username));
                });
            }
            if (role == "NV PCTSV")
            {
                AddSidebarButton("Thông tin cá nhân", () =>
                {
                    LoadForm(new FormNhanVienCaNhan(conn, username));
                });
                AddSidebarButton("Quản lý sinh viên", () =>
                {
                    LoadForm(new FormQuanLySinhVien_PCTSV(conn, username));
                });
            }
            if (role == "NV PKT")
            {
                AddSidebarButton("Quản lý bảng điểm", () =>
                {
                    LoadForm(new FormQuanLyBangDiem_PKT(conn, username));
                });
            }

            AddSidebarButton("Đăng xuất", () =>
            {
                this.Hide();
                using (LoginForm loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        string selectedRole = loginForm.RoleSelected.ToUpper();
                        string newUser = loginForm.Username.ToUpper();
                        OracleConnection newConn = loginForm.UserConnection;

                        FormMain newMain = new FormMain(newConn, newUser, selectedRole);
                        newMain.ShowDialog();
                    }
                }
                this.Close();
            });
        }



        private void AddSidebarButton(string title, Action action)
        {
            Button btn = new Button();
            btn.Text = title;
            btn.Dock = DockStyle.Top;
            btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(30, 30, 60);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);

            btn.Click += (s, e) => action();

            sidebarPanel.Controls.Add(btn);
            sidebarPanel.Controls.SetChildIndex(btn, 0); // Show newest on top
        }

        private void LoadForm(Form form)
        {
            mainPanel.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(form);
            form.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
