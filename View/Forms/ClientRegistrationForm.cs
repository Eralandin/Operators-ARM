using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.Presenter;
using Org.BouncyCastle.Bcpg;
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
    public partial class ClientRegistrationForm : Form, IClientRegister
    {
        private readonly ClientPresenter _presenter;
        private CellEventArgs _cell;
        public ClientRegistrationForm()
        {
            InitializeComponent();
            _presenter = new ClientPresenter(this);
            dateTimePicker1.Value = DateTime.Now;
            foreach (var tarif in _presenter.tarifs)
            {
                comboBox1.Items.Add(tarif.TarifName);
            }
            ValidateButton.Click += ValidateButton_Click;
        }
        public ClientRegistrationForm(CellEventArgs cell)
        {
            InitializeComponent();
            _cell = cell;
            _presenter = new ClientPresenter(this);

            Client currentClient = _presenter.clients.FindAll(client => client.ContractNumber == int.Parse(_cell.contractnumber))[0];
            dateTimePicker1.Value = currentClient.ContractDate;
            textBox1.Text = currentClient.Fullname;
            textBox2.Text = currentClient.Passport;
            textBox4.Text = currentClient.ClientNumber;
            foreach (var tarif in _presenter.tarifs)
            {
                comboBox1.Items.Add(tarif.TarifName);
            }
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(currentClient.ClientTarif.TarifName.ToString());
            ValidateButton.Click += EditButton_Click;
        }
        public event EventHandler AddClient;
        public event EventHandler<CellEventArgs> EditClient;
        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                EditClient?.Invoke(this, _cell);
                this.Dispose();
            }
            catch(Exception ex)
            {
                MessageView(ex.Message);
            }
        }
        private void ValidateButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddClient?.Invoke(this, EventArgs.Empty);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }
        public string GetDogovorDate()
        {
            return dateTimePicker1.Value.ToString();
        }
        public string GetPassport()
        {
            return textBox2.Text;
        }
        public string GetTarifName()
        {
            return comboBox1.Text;
        }
        public string GetPhoneNumber()
        {
            return textBox4.Text;
        }
        public string GetFullname()
        {
            return textBox1.Text;
        }
    }
}
