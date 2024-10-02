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
using KursovayaKapitonova.Presenter;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.View.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;

namespace KursovayaKapitonova
{
    public partial class MainForm : Form, IMain
    {
        private readonly MainFormPresenter _presenter;
        private EmployeesTarifsForm _employeesTarifsForm;

        public event EventHandler LoadData;
        public event EventHandler PDFTarifs;
        public event EventHandler PDFClients;
        public event EventHandler BalanceMinusTarifPrice;
        public event EventHandler<CellEventArgs> DogovorPause;
        public event EventHandler<CellEventArgs> AddBalance;
        public event EventHandler<CellEventArgs> DeleteClient;
        public event EventHandler ReloadLists;
        public event EventHandler<CellEventArgs> PDFDetalization;
        public event EventHandler SortByContract;
        public event EventHandler SortByNumber;
        public event EventHandler SortByTarif;
        public event EventHandler SortByFullname;
        public MainForm(Employee currentUser)
        {
            SuspendLayout();
            InitializeComponent();
            _presenter = new MainFormPresenter(this, currentUser);
            textBox1.Text = DateTime.Now.Date.ToShortDateString();
            textBox4.Text = _presenter.currentUser.Fullname;
            LoadData?.Invoke(this, EventArgs.Empty);
            ShowData(JsonFileHelper.LoadClients());
            radioButton1.Select();
            
            ResumeLayout(true);
        }
        public void MessageView(string message)
        {
            MessageBox.Show(message);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть программу?", "Закрытие программы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            } 
        }
        public void ShowData(List<PhoneRecord> records)
        {
            try
            {
                if (records.Count == 0)
                {
                    dataGridView2.Rows.Clear();
                    dataGridView2.Enabled = false;
                }
                else
                {
                    dataGridView2.Enabled = true;
                    dataGridView2.Rows.Clear();
                    for (int i = 0; i < records.Count; i++)
                    {
                        dataGridView2.Rows.Add(records[i].MomentOfRecord, records[i].ElapsedTime, records[i].ContrAgent, records[i].RecordType, records[i].RecordIOType, records[i].RecordContrAgentType, records[i].BalanceBefore, records[i].BalanceAfter);
                    }
                }
                foreach (DataGridViewColumn column in dataGridView2.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch(Exception ex)
            {
                MessageView(ex.Message);
            }
        }
        public void ShowData(List <Client> clients)
        {
            try
            {
                if (clients.Count == 0)
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Enabled = false;
                    dataGridView2.Enabled = false;
                }
                else
                {
                    dataGridView1.Enabled = true;
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < clients.Count; i++)
                    {
                        dataGridView1.Rows.Add(clients[i].ContractNumber, clients[i].ContractDate, clients[i].ClientNumber, clients[i].Fullname, clients[i].Passport, clients[i].ClientTarif.TarifName, clients[i].Balance, clients[i].IsActive ? "Да" : "Нет");
                    }
                }
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                ReloadLists?.Invoke(this, EventArgs.Empty);
                ShowData(_presenter.clients[_presenter.clients.FindIndex(x => x.ContractNumber == int.Parse(dataGridView1[0, e.RowIndex].Value.ToString()))].phoneRecords);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из программы?", "Выход из программы", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<Employee> employees = JsonFileHelper.LoadEmployees();
                _employeesTarifsForm = new EmployeesTarifsForm(JsonFileHelper.LoadEmployees(), _presenter.currentUser);
                _employeesTarifsForm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void тарифыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _employeesTarifsForm = new EmployeesTarifsForm(JsonFileHelper.LoadTarifs(), _presenter.currentUser);
                _employeesTarifsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void ClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDFClients?.Invoke(this, EventArgs.Empty);
        }

        private void TarifsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PDFTarifs?.Invoke(this, EventArgs.Empty);
        }

        private void ClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClientRegistrationForm _ClientRegistrationForm = new ClientRegistrationForm();
                _ClientRegistrationForm.ShowDialog();
                _presenter.clients = JsonFileHelper.LoadClients();
                ShowData(_presenter.clients);
            }
            catch(Exception ex )
            {
                MessageView(ex.Message);
            }
        }

        private void PriceGetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Parse(textBox1.Text).Day == 1)
                {
                    BalanceMinusTarifPrice?.Invoke(this, EventArgs.Empty);
                    ShowData(JsonFileHelper.LoadClients());
                    MessageView("Вычет абонентской платы проведён успешно!");
                }
                else
                {
                    MessageView("Абонентская плата снимается только первого числа каждого месяца!");
                }
            }
            catch(Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void DogovorPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите приостановить действие договора этого клиента?", "Приостановление действия договора", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cell = new CellEventArgs(selectedCell);
                    cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex].Value.ToString();
                    DogovorPause?.Invoke(this, cell);
                    ShowData(JsonFileHelper.LoadClients());
                    MessageView("Состояние действия договора изменено успешно!");
                }
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void BalanceAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите пополнить баланс этого клиента?", "Пополнение баланса", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cell = new CellEventArgs(selectedCell);
                    cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex ].Value.ToString();
                    cell.balanceAdd = float.Parse(Interaction.InputBox("Введите сумму, на которую необходимо пополнить баланс (в рублях, делитель рублей и копеек - запятая):", "Пополнение баланса", ""));
                    AddBalance?.Invoke(this, cell);
                    MessageView("Пополнение баланса успешно!");
                    ShowData(JsonFileHelper.LoadClients());
                }
                catch (FormatException)
                {
                    MessageView("Введено некорректное значение для пополнения или была совершена отмена операции (делитель рублей и копеек - запятая!)");
                }
                catch (Exception ex)
                {
                    MessageView("Ошибка пополнения баланса: " + ex.Message);
                }
            }
        }

        private void PhoneRecordAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                CellEventArgs cell = new CellEventArgs(selectedCell);
                cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex].Value.ToString();
                PhoneRecordRegisterForm phoneRecordRegister = new PhoneRecordRegisterForm(cell);
                phoneRecordRegister.ShowDialog();
                if (phoneRecordRegister.IsDisposed)
                {
                    throw new OperationCanceledException("Операция отменена!");
                }
                MessageView("Запись успешно добавлена!");
                ShowData(JsonFileHelper.LoadClients());
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void удалениеВыбранногоКлиентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Удаление клиента", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cell = new CellEventArgs(selectedCell);
                    cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex].Value.ToString();

                    DeleteClient?.Invoke(this, cell);
                    MessageView("Удаление клиента прошло успешно!");
                    ShowData(JsonFileHelper.LoadClients());
                }
                catch (Exception ex)
                {
                    MessageView("Ошибка удаления клиента: " + ex.Message);
                }
            }
        }

        private void редактированиеВыбранногоКлиентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите редактировать этого клиента?", "Редактирование клиента", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cell = new CellEventArgs(selectedCell);
                    cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex].Value.ToString();
                    ClientRegistrationForm _ClientRegistrationForm = new ClientRegistrationForm(cell);
                    _ClientRegistrationForm.ShowDialog();
                    ShowData(JsonFileHelper.LoadClients());
                }
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void детализацияСчётаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                try
                {
                    DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                    CellEventArgs cell = new CellEventArgs(selectedCell);
                    cell.contractnumber = dataGridView1[0, cell.Cell.RowIndex].Value.ToString();
                    PDFDetalization?.Invoke(this, cell);
                }
                catch (Exception ex)
                {
                    MessageView(ex.Message);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SortByContract?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SortByNumber?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SortByTarif?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SortByFullname?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageView(ex.Message);
            }
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            radioButton1.Select();
        }
    }
}
