using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAnCuoiKiNhom5_SimpleNote.Controllers;
using DoAnCuoiKiNhom5_SimpleNote.Models;

namespace DoAnCuoiKiNhom5_SimpleNote.Views
{
    public partial class frmSignIn : Form
    {
        public frmSignIn()
        {
            InitializeComponent();
            this.ptBArrow.Visible = false;
            this.btnSignUp.Visible = false;
            this.lblClick.Visible = false;
        }


        private void frmSignIn_Load(object sender, EventArgs e)
        {
            this.txtUsername.Focus();

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void tsmCheckSpelling_Click(object sender, EventArgs e)
        {
        }

        private void frmSignIn_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void lbl_SignUp_MouseHover(object sender, EventArgs e)
        {
            this.lbl_SignUp.Cursor = Cursors.Hand;
            this.lblClick.Visible = true;
        }

        private void lblClick_Click(object sender, EventArgs e)
        {
            this.lblClick.Cursor = Cursors.Hand;
            this.ptBArrow.Visible = true;
            this.btnSignUp.Visible = true;
        }

        private void lblClick_MouseHover(object sender, EventArgs e)
        {
            this.lblClick.Cursor = Cursors.Hand;
        }
        private void frmSignIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát phần mềm?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (UserControllers.getUser(this.txtUsername.Text.Trim()) != null && UserControllers.checkPassword(this.txtUsername.Text.Trim(), this.txtPassword.Text.Trim()) == true)
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK);
                this.Hide();
                frmNote f2 = new frmNote(this.txtUsername.Text.Trim());
                f2.ShowDialog();
                this.Show();
                this.txtUsername.Clear();
                this.txtPassword.Clear();

            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.TabIndex = 1;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            frmSignUp f = new frmSignUp();
            f.ShowDialog();
            lblClick.Visible = false;
            ptBArrow.Visible = false;
            btnSignUp.Visible = false;
        }
    }
}
