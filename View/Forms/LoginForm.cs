using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursovayaKapitonova.Presenter;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.Models.Classes;

namespace KursovayaKapitonova.View.Forms
{
    public partial class LoginForm : Form, ILogin
    {
        private readonly LoginFormPresenter _presenter;
        private MainForm _mainForm;
        private RegistrationForm _registrationForm;

        public event EventHandler LoginAttempt;
        public LoginForm(List<Employee> employees)
        {
            _presenter = new LoginFormPresenter(this, employees);
            InitializeComponent();
        }
        public string GetLogin()
        {
            return textBox1.Text;
        }
        public string GetPassword()
        {
            return LoginPasswordTextBox.Text;
        }
        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }
        public void LoginSuccess(Employee employee)
        {
            MessageBox.Show("Здравствуйте, " + employee.Fullname + "!");
            this.Visible = false;
            _mainForm = new MainForm(employee);
            _mainForm.ShowDialog();
            this.Dispose();
        }
        public void Registration(List<Employee> employees)
        {
            if (MessageBox.Show("Пользователей не обнаружено. Желаете пройти регистрацию?","Регистрация", MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Visible = false;
                _registrationForm = new RegistrationForm(employees);
                _registrationForm.ShowDialog();
                this.Visible = true;
            }
            else
            {
                return;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPasswordCheckBox.Checked == true)
            {
                LoginPasswordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                LoginPasswordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginAttempt?.Invoke(this, EventArgs.Empty);
        }
    }
}
