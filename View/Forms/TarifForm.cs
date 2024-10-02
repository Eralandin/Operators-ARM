using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursovayaKapitonova.Models.Classes;

namespace KursovayaKapitonova.View.Forms
{
    public partial class TarifForm : Form
    {
        public Tarif tarif;
        public bool IsEditing;
        public string EditedTarifName;
        public TarifForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsEditing != true)
                {
                    List<Tarif> tarifs = JsonFileHelper.LoadTarifs();
                    if (tarifs.FindAll(tarif => tarif.TarifName == TarifNameTextBox.Text).Count == 0)
                    {
                        tarif = new Tarif()
                        {
                            TarifName = TarifNameTextBox.Text,
                            InnerCallPrice = float.Parse(numericUpDown2.Text),
                            OuterCallPrice = float.Parse(numericUpDown1.Text),
                            TarifPrice = float.Parse(numericUpDown3.Text),
                            SmsPrice = float.Parse(numericUpDown4.Text),
                        };
                        tarifs.Add(tarif);
                        JsonFileHelper.SaveTarifs(tarifs, "data.json");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Данное название тарифа занято!");
                    }
                }
                else if (IsEditing == true)
                {
                    List<Tarif> tarifs = JsonFileHelper.LoadTarifs();
                    if (tarifs.FindAll(tar => tar.TarifName == TarifNameTextBox.Text).Count == 1)
                    {
                        int index = tarifs.FindIndex(tar => tar.TarifName == EditedTarifName);
                        tarifs[index].TarifPrice = float.Parse(numericUpDown3.Text);
                        tarifs[index].OuterCallPrice = float.Parse(numericUpDown1.Text);
                        tarifs[index].InnerCallPrice = float.Parse(numericUpDown2.Text);
                        tarifs[index].SmsPrice = float.Parse(numericUpDown4.Text);
                        tarifs[index].TarifName = TarifNameTextBox.Text;
                        JsonFileHelper.SaveTarifs(tarifs, "data.json");
                        this.Close();
                    }
                    else
                    {
                        throw new Exception("Непредвиденная ошибка!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('.') || e.KeyChar.Equals(','))
            {
                e.KeyChar = ((System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture).NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
            }
        }
    }
}
