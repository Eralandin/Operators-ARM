using KursovayaKapitonova.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovayaKapitonova.Models.Classes;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using KursovayaKapitonova.View.Forms;
using Org.BouncyCastle.Crypto.Tls;

namespace KursovayaKapitonova.Presenter
{
    public class MainFormPresenter
    {
        public List<Client> clients;
        public List<Tarif> tarifs;
        private readonly IMain _view;
        public Employee currentUser;
        private ClientComparer clientComparer;
        public MainFormPresenter(IMain view, Employee currentUser)
        {
            clientComparer = new ClientComparer();
            this.currentUser = currentUser;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _view.LoadData += LoadDataFromFile;
            _view.PDFTarifs += TarifsToPDF;
            _view.PDFClients += ClientsToPDF;
            _view.BalanceMinusTarifPrice += BalanceMinusTarifPrice;
            _view.DogovorPause += DogovorPause;
            _view.AddBalance += AddBalance;
            _view.DeleteClient += DeleteClient;
            _view.ReloadLists += ReloadLists;
            _view.PDFDetalization += PDFDetalization;
            _view.SortByContract += ContractSorting;
            _view.SortByFullname += FullnameSorting;
            _view.SortByNumber += NumberSorting;
            _view.SortByTarif += TarifSorting;
        }
        private void ContractSorting(object sender, EventArgs e)
        {
            List<Client> sortedClients = JsonFileHelper.LoadClients();
            sortedClients.Sort(clientComparer.Compare);
            _view.ShowData(sortedClients);
        }
        private void FullnameSorting(object sender, EventArgs e)
        {
            List<Client> sortedClients = JsonFileHelper.LoadClients();
            sortedClients.Sort(clientComparer.CompareFullname);
            _view.ShowData(sortedClients);
        }
        private void NumberSorting(object sender, EventArgs e)
        {
            List<Client> sortedClients = JsonFileHelper.LoadClients();
            sortedClients.Sort(clientComparer.CompareNumber);
            _view.ShowData(sortedClients);
        }
        private void TarifSorting(object sender, EventArgs e)
        {
            List<Client> sortedClients = JsonFileHelper.LoadClients();
            sortedClients.Sort(clientComparer.CompareTarif);
            _view.ShowData(sortedClients);
        }
        private void PDFDetalization(object sender, CellEventArgs e)
        {
            clients = JsonFileHelper.LoadClients();
            Client currentClient = clients.FirstOrDefault(cl => cl.ContractNumber == int.Parse(e.contractnumber));
            string filePath = "Детализация счета №" + currentClient.ContractNumber + ".pdf";
            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);

            try
            {
                BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font12 = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font font24 = new iTextSharp.text.Font(baseFont, 24, iTextSharp.text.Font.NORMAL);

                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                // Добавление заголовка
                Paragraph header = new Paragraph("Детализация счета по договору №" + currentClient.ContractNumber + ":\n\n", font24);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                if (currentClient.phoneRecords.Count > 0)
                {
                    // Создание таблицы
                    PdfPTable table = new PdfPTable(7); // 7 столбцов для даты, длительности, вида услуги, типа вызова, типа контрагента, баланса до списания и баланса после списания
                    table.WidthPercentage = 100;

                    // Добавление заголовков столбцов
                    string[] headers = { "Дата", "Длит. эфира, с", "Вид услуги", "Тип вызова", "Тип контрагента", "Баланс до списания, руб", "Баланс после списания, руб" };
                    foreach (string headerText in headers)
                    {
                        PdfPCell headerCell = new PdfPCell(new Phrase(headerText, font12));
                        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(headerCell);
                    }

                    // Добавление содержимого
                    foreach (var record in currentClient.phoneRecords)
                    {
                        table.AddCell(new Phrase(record.MomentOfRecord.ToString(), font12)); // Дата
                        table.AddCell(new Phrase(record.ElapsedTime.ToString(), font12)); // Длительность
                        table.AddCell(new Phrase(record.RecordType.ToString(), font12)); // Вид услуги
                        table.AddCell(new Phrase(record.RecordIOType.ToString(), font12)); // Тип вызова
                        table.AddCell(new Phrase(record.RecordContrAgentType.ToString(), font12)); // Тип контрагента
                        table.AddCell(new Phrase(record.BalanceBefore.ToString(), font12)); // Баланс до списания
                        table.AddCell(new Phrase(record.BalanceAfter.ToString(), font12)); // Баланс после списания
                    }

                    // Добавление таблицы в документ
                    document.Add(table);
                }
                else
                {
                    Paragraph paragraph = new Paragraph("Пусто. Нет зарегистрированных операций.", font12);
                    document.Add(paragraph);
                }

            }
            catch (DocumentException ex)
            {
                _view.MessageView("Ошибка создания документа: " + ex.Message);
            }
            catch (IOException ex)
            {
                _view.MessageView("Ошибка ввода/вывода: " + ex.Message);
            }
            finally
            {
                document.Close();
                _view.MessageView("Файл PDF успешно создан и выгружен в рабочую директорию программы!");
            }
        }

        private void ReloadLists(object sender, EventArgs e)
        {
            clients = JsonFileHelper.LoadClients();
            tarifs = JsonFileHelper.LoadTarifs();
        }
        private void LoadDataFromFile(object sender, EventArgs e)
        {
            try
            {
                clients = JsonFileHelper.LoadData().Item1;
                tarifs = JsonFileHelper.LoadData().Item2;
            }
            catch (Exception ex) 
            {
                _view.MessageView(ex.Message);
            }
        }
        private void DeleteClient(object sender, CellEventArgs ce)
        {
            clients = JsonFileHelper.LoadClients();
            clients.RemoveAt(clients.FindIndex(cl => cl.ContractNumber == int.Parse(ce.contractnumber)));
            JsonFileHelper.SaveClients(clients, "data.json");
        }
        private void AddBalance(object sender, CellEventArgs ce)
        {
            if(ce.balanceAdd > 0)
            {
                if (clients[clients.FindIndex(cl => cl.ContractNumber == int.Parse(ce.contractnumber))].IsActive == false)
                {
                    clients = JsonFileHelper.LoadClients();
                    clients[clients.FindIndex(cl => cl.ContractNumber == int.Parse(ce.contractnumber))].Balance += ce.balanceAdd;
                    JsonFileHelper.SaveClients(clients, "data.json");
                }
                else
                {
                    throw new Exception("Договор данного клиента неактивен!");
                }
            }
            else
            {
                throw new Exception("Сумма пополнения не может быть меньше или равной нулю!");
            }
        }
        private void DogovorPause(object sender, CellEventArgs e)
        {
            clients = JsonFileHelper.LoadClients();
            clients[clients.FindIndex(cl => cl.ContractNumber == int.Parse(e.contractnumber))].Activate();
            JsonFileHelper.SaveClients(clients, "data.json");
        }
        private void BalanceMinusTarifPrice(object sender, EventArgs e)
        {
            clients = JsonFileHelper.LoadClients();
            foreach (var client in clients)
            {
                if (client.IsActive == false)
                {
                    client.Balance -= client.ClientTarif.TarifPrice;
                }
            }
            JsonFileHelper.SaveClients(clients, "data.json");
        }
        private void TarifsToPDF(object sender, EventArgs e)
        {
            List<Tarif> tarifs = JsonFileHelper.LoadTarifs();
            string filePath = "Тарифы.pdf";
            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
            try
            {
                BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                // Добавление заголовка
                Paragraph header = new Paragraph("Список тарифов:\n", font);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                if (tarifs.Count > 0 )
                {
                    // Добавление содержимого
                    foreach (var tarif in tarifs)
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Add(new Chunk($"Название тарифа: {tarif.TarifName}\n", font));
                        paragraph.Add(new Chunk($"Стоимость тарифа: {tarif.TarifPrice} руб.\n", font));
                        paragraph.Add(new Chunk($"Стоимость звонков внутри сети: {tarif.InnerCallPrice} руб. в секунду\n", font));
                        paragraph.Add(new Chunk($"Стоимость звонков вне сети: {tarif.OuterCallPrice} руб. в секунду\n", font));
                        paragraph.Add(new Chunk($"Стоимость SMS: {tarif.SmsPrice} за одно SMS\n\n", font));

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph();
                    paragraph.Add(new Chunk($"Пусто. Нет зарегистрированных тарифов."));
                    document.Add(paragraph);
                }
                
            }
            catch (DocumentException ex)
            {
                _view.MessageView("Ошибка создания документа: " + ex.Message);
            }
            catch (IOException ex)
            {
                _view.MessageView("Ошибка ввода/вывода: " + ex.Message);
            }
            finally
            {
                document.Close();
                _view.MessageView("Файл PDF успешно создан и выгружен в рабочую директорию программы!");
            }
        }
        private void ClientsToPDF(object sender, EventArgs e)
        {
            List<Client> clients = JsonFileHelper.LoadClients();
            // Путь к файлу PDF
            string filePath = "Клиенты.pdf";
            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
            try
            {
                // Создание файла PDF и запись в него данных
                BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                // Открытие документа для записи
                document.Open();
                
                Paragraph header = new Paragraph();
                header.Alignment = Element.ALIGN_CENTER;
                header.Add(new Phrase("Список клиентов:\n", font));
                document.Add(header);

                if (clients.Count > 0)
                {
                    // Добавление содержимого в документ
                    foreach (var client in clients)
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Font = font;
                        paragraph.Add(new Phrase("ФИО: ", font));
                        paragraph.Add(new Phrase(client.Fullname, font));
                        paragraph.Add(new Phrase("\nПаспорт: ", font));
                        paragraph.Add(new Phrase(client.Passport, font));
                        paragraph.Add(new Phrase("\nТариф: ", font));
                        paragraph.Add(new Phrase(client.ClientTarif.TarifName, font));
                        paragraph.Add(new Phrase("\nБаланс: ", font));
                        paragraph.Add(new Phrase(client.Balance.ToString() + " руб.", font));
                        paragraph.Add(new Phrase("\nАктивен: ", font));
                        paragraph.Add(new Phrase(client.IsActive ? "Да" : "Нет", font));
                        paragraph.Add(new Phrase("\nНомер договора: ", font));
                        paragraph.Add(new Phrase(client.ContractNumber.ToString(), font));
                        paragraph.Add(new Phrase("\nДата договора: ", font));
                        paragraph.Add(new Phrase(client.ContractDate.ToShortDateString(), font));
                        paragraph.Add(new Phrase("\nТелефонный номер: ", font));
                        paragraph.Add(new Phrase(client.ClientNumber, font));
                        paragraph.Add(new Phrase("\n\n", font));

                        document.Add(paragraph);
                    }
                }
                else
                {
                    Paragraph paragraph = new Paragraph("Пусто. \nНет зарегистрированных клиентов.", font);
                    document.Add(paragraph);
                }
                
            }
            catch (DocumentException ex)
            {
                _view.MessageView("Ошибка создания документа: " + ex.Message);
            }
            catch (IOException ex)
            {
                _view.MessageView("Ошибка ввода/вывода: " + ex.Message);
            }
            finally
            {
                document.Close();
                _view.MessageView("Файл PDF успешно создан и выгружен в рабочую директорию программы!");
            }
        }
    }
}
