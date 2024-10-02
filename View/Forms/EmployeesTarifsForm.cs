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
    public partial class EmployeesTarifsForm : Form, IEmpTarifs
    {
        private readonly EmpTarifsPresenter _presenter;
        private bool IsTarifForm;
        public EmployeesTarifsForm(List<Employee> employees, Employee currentUser)
        {
            InitializeComponent();
            _presenter = new EmpTarifsPresenter(this, employees, currentUser);
            ShowData(employees);
            button1.Enabled = false;
            button2.Enabled = false;
            LoginButton.Enabled = false;
            LoginButton.Visible = false;
            IsTarifForm = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public EmployeesTarifsForm(List<Tarif> tarifs, Employee currentUser)
        {
            InitializeComponent();
            label2.Text = "Тарифы";
            _presenter = new EmpTarifsPresenter(this, tarifs, currentUser);
            ShowData(tarifs);
            button1.Enabled = false;
            button2.Enabled = false;
            LoginButton.Enabled = false;
            IsTarifForm = true;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public event EventHandler Add;
        public event EventHandler<CellEventArgs> Delete;
        public event EventHandler<string> CheckValidate;
        public event EventHandler<Tarif> AddTarif;
        public event EventHandler<CellEventArgs> DeleteTarif;
        public event EventHandler<CellEventArgs> EditTarif;
        public void EditTarifToForm(Tarif tarif)
        {
            TarifForm tarifForm = new TarifForm();
            tarifForm.IsEditing = true;
            tarifForm.TarifNameTextBox.Text = tarif.TarifName;
            tarifForm.EditedTarifName = tarif.TarifName;
            tarifForm.numericUpDown1.Value = decimal.Parse(tarif.OuterCallPrice.ToString());
            tarifForm.numericUpDown2.Value = decimal.Parse(tarif.InnerCallPrice.ToString());
            tarifForm.numericUpDown3.Value = decimal.Parse(tarif.TarifPrice.ToString());
            tarifForm.numericUpDown4.Value = decimal.Parse(tarif.SmsPrice.ToString());
            tarifForm.TarifNameTextBox.Enabled = false;
            tarifForm.ShowDialog();
        }

        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }
        public void AddEmpForm(List<Employee> employees)
        {
            try
            {
                RegistrationForm _registerForm = new RegistrationForm(employees);
                _registerForm.ShowDialog();
                employees = JsonFileHelper.LoadEmployees();
                ShowData(employees);
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void ShowData(List<Employee> employees) 
        {
            if (employees.Count != 0)
            {
                dataGridView1.DataSource = employees;
                dataGridView1.Columns[0].HeaderText = "ФИО Пользователя";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[1].HeaderText = "Логин пользователя";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
            }
            else
            {
                dataGridView1.Rows.Clear();
            }
        }
        private void ShowData(List<Tarif> tarifs)
        {
            if (tarifs.Count != 0)
            {
                dataGridView1.DataSource = tarifs;
                dataGridView1.Columns[1].HeaderText = "Стоимость тарифа";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[1].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[2].HeaderText = "Стоимость операции внутри сети";
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[3].HeaderText = "Стоимость операции вне сети";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[4].HeaderText = "Стоимость SMS";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
                dataGridView1.Columns[0].HeaderText = "Название тарифа";
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.RosyBrown };
            }
            else
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
            }
        }
        //Добавить
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsTarifForm != true)
            {
                Add?.Invoke(this, EventArgs.Empty);
            }
            else if (IsTarifForm == true)
            {
                try
                {
                    TarifForm tarifForm = new TarifForm();
                    tarifForm.ShowDialog();
                    if (tarifForm.IsDisposed == false)
                    {
                        AddTarif?.Invoke(this, tarifForm.tarif);
                        MessageView("Тариф " + tarifForm.tarif.TarifName + " успешно добавлен!");
                        tarifForm.Dispose();
                    }
                    ShowData(JsonFileHelper.LoadTarifs());
                }
                catch (Exception ex)
                {
                    MessageView(ex.Message);
                }
                
            }
        }
        //Редактировать
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0 && IsTarifForm == true)
            {
                if (MessageBox.Show("Вы уверены, что хотите редактировать этот тариф?", "Редактирование тарифа", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cellEventArgs = new CellEventArgs(selectedCell);
                    cellEventArgs.tarifname = dataGridView1[0, selectedCell.RowIndex].Value.ToString();
                    EditTarif?.Invoke(this, cellEventArgs);
                    ShowData(JsonFileHelper.LoadTarifs());
                }
            }
        }
        //Удалить
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0 && IsTarifForm != true)
            {
                try
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Удаление пользователя", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                        CellEventArgs cell = new CellEventArgs(selectedCell);
                        cell.fullname = selectedCell.Value.ToString();
                        Delete?.Invoke(this, cell);
                        ShowData(_presenter.employees);
                    }
                }
                catch (Exception ex)
                {
                    MessageView(ex.Message);
                }
            }
            else if (dataGridView1.SelectedCells.Count > 0 && IsTarifForm == true)
            {
                try
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить этот тариф?", "Удаление тарифа", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                        CellEventArgs cell = new CellEventArgs(selectedCell);
                        cell.tarifname = dataGridView1[0, selectedCell.RowIndex].Value.ToString();
                        DeleteTarif?.Invoke(this, cell);
                        ShowData(_presenter.tarifs);
                    }
                }
                catch (Exception ex)
                {
                    MessageView(ex.Message);
                }
            }
        }
        //Проверка пароля пользователя
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                ValidatePasswordForm validate = new ValidatePasswordForm();
                validate.ShowDialog();
                CheckValidate?.Invoke(this, validate.GetOutEnteredPassword());
                validate.Dispose();
                button1.Enabled = true;
                button2.Enabled = true;
                LoginButton.Enabled = true;
                button3.Enabled = false;
                button3.Visible = false;
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }
    }
    public class CellEventArgs : EventArgs
    {
        public string fullname;
        public string tarifname;
        public string contractnumber;
        public float balanceAdd;
        public DataGridViewCell Cell { get; set; }

        public CellEventArgs(DataGridViewCell cell)
        {
            Cell = cell;
        }
    }
}
