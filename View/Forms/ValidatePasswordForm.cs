using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovayaKapitonova.View.Forms
{
    public partial class ValidatePasswordForm : Form
    {
        string enteredPassword;
        public ValidatePasswordForm()
        {
            InitializeComponent();
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            GetEnteredPassword(PasswordValidateTextBox.Text);
            this.Close();
        }
        private void GetEnteredPassword(string ent)
        {
            if (!string.IsNullOrEmpty(ent)) 
            {
                enteredPassword = ent;
            }
            else
            {
                throw new ArgumentNullException("Введите и подтвердите пароль нажатием кнопки!");
            }
        }
        public string GetOutEnteredPassword()
        {
            return enteredPassword;
        }
    }
}
