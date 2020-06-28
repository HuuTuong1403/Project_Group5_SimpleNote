using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DoAnCuoiKiNhom5_SimpleNote.Models;
using DoAnCuoiKiNhom5_SimpleNote.Controllers;

namespace DoAnCuoiKiNhom5_SimpleNote.Views
{
    public partial class frmSignUp : Form
    {
        string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        string sdt = @"^[-+]?[0-9]*\.?[0-9]+$";
        public frmSignUp()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex(match);
            Regex regex = new Regex(sdt);
            if (this.txtUsername.Text.Trim().Length < 8)
            {
                this.errP1.SetError(this.txtUsername, "Mời bạn nhập tên đăng nhập đủ 8 ký tự");
                return;
            }
            else if (UserControllers.getUser(this.txtUsername.Text.Trim()) != null)
            {
                this.errP1.SetError(txtUsername, "Username đã tồn tại");
                return;
            }
            else
                this.errP1.Clear();
            if (this.txtPassword.Text.Trim().Length <= 0)
            {
                this.errP1.SetError(this.txtPassword, "Mời bạn nhập mật khẩu");
                return;
            }
            if (this.txtEmail.Text.Trim().Length <= 0)
            {
                this.errP1.SetError(this.txtEmail, "Mời bạn nhập email");
                return;
            }
            if (this.txtPhone.Text.Trim().Length < 10)
            {
                this.errP1.SetError(this.txtPhone, "Mời bạn nhập số điện thoại đủ 10 số");
                return;
            }
            if (reg.IsMatch(this.txtEmail.Text))
            {
                this.errP1.Clear();
            }
            else
            {
                this.errP1.SetError(this.txtEmail, "Email được nhập không hợp lệ");
                return;
            }
            if(regex.IsMatch(this.txtPhone.Text))
            {
                this.errP1.Clear();
            }
            else
            {
                this.errP1.SetError(this.txtPhone, "Số điện thoại không hợp lệ");
                return;
            }
            User user = new User();
            user.Username = this.txtUsername.Text.Trim();
            user.Password = this.txtPassword.Text.Trim();
            user.Phone = this.txtPhone.Text.Trim();
            user.Email = this.txtEmail.Text.Trim();

            if (UserControllers.AddUser(user) == false)
            {
                MessageBox.Show("Đăng ký không thành công");
                return;
            }
            else
            {
                MessageBox.Show("Đăng ký thành công");
                this.Close();
            }
            this.txtUsername.Clear();
            this.txtPassword.Clear();
            this.txtPhone.Clear();
            this.txtEmail.Clear();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
