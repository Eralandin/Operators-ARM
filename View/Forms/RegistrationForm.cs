using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.Presenter;

namespace KursovayaKapitonova.View.Forms
{
    public partial class RegistrationForm : Form, IRegistration
    {
        private readonly RegistrationPresenter _presenter;
        
        public RegistrationForm(List<Employee> employees)
        {
            _presenter = new RegistrationPresenter(this, employees);
            InitializeComponent();
        }
        public event EventHandler RegistrationAttempt;
        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }
        public string GetRealLogin()
        {
            return textBox2.Text;
        }
        public string GetLogin()
        {
            return textBox1.Text;
        }
        public string GetPassword()
        {
            return PasswordRegistrationTextBox.Text;
        }
        public void RegistrationSuccess(Employee newEmployee)
        {
            MessageBox.Show("Регистрация нового пользователя прошла успешно!");
            this.Dispose();
        }

        private void ShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPasswordCheckBox.Checked == true)
            {
                PasswordRegistrationTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                PasswordRegistrationTextBox.UseSystemPasswordChar = true;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            RegistrationAttempt?.Invoke(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
