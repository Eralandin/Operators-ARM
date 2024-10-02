using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.Presenter;

namespace KursovayaKapitonova.View.Forms
{
    public partial class PhoneRecordRegisterForm : Form, IPhoneRecord
    {
        public int contractNumber;
        private readonly PhoneRecordPresenter _presenter;
        public PhoneRecordRegisterForm(CellEventArgs cell)
        {
            InitializeComponent();
            contractNumber = int.Parse(cell.contractnumber);
            _presenter = new PhoneRecordPresenter(this, contractNumber);
            textBox1.Text = DateTime.Now.ToString();
            Array typesArray = Enum.GetValues(typeof(PhoneRecordType));
            foreach (var type in typesArray)
            {
                comboBox1.Items.Add(type);
            }
            typesArray = Enum.GetValues(typeof(IOType));
            foreach (var type in typesArray)
            {
                comboBox2.Items.Add(type);
            }
            typesArray = Enum.GetValues(typeof(ContrAgentType));
            foreach (var type in typesArray)
            {
                comboBox3.Items.Add(type);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }
        public event EventHandler AddRecord;
        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((PhoneRecordType)comboBox1.SelectedItem == PhoneRecordType.SMS)
            {
                numericUpDown1.Enabled = false;
            }
            else
            {
                numericUpDown1.Enabled = true;
            }
        }
        public PhoneRecordType GetPhoneRecordType()
        {
            return (PhoneRecordType)comboBox1.SelectedItem;
        }
        public IOType GetIOType()
        {
            return (IOType)comboBox2.SelectedItem;
        }
        public ContrAgentType GetContrAgentType()
        {
            return (ContrAgentType)comboBox3.SelectedItem;
        }
        public float GetElapsedTime()
        {
            return float.Parse(numericUpDown1.Value.ToString());
        }
        public string GetMomentOfRecord()
        {
            return textBox1.Text;
        }
        public int GetContractNumber()
        {
            return contractNumber;
        }
        public string GetContrAgent()
        {
            return textBox2.Text;
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            try
            {
                AddRecord?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }
    }
}
