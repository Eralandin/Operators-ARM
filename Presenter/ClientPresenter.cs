using KursovayaKapitonova.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.View.Forms;
using System.Net;

namespace KursovayaKapitonova.Presenter
{
    public class ClientPresenter
    {
        public List<Client> clients;
        public List<Tarif> tarifs;
        private readonly IClientRegister _view;
        public ClientPresenter(IClientRegister view) 
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            clients = JsonFileHelper.LoadClients();
            tarifs = JsonFileHelper.LoadTarifs();
            if (tarifs.Count == 0)
            {
                throw new ArgumentException("Список тарифов пуст! Для начала работы необходимо создать тариф(-ы)!");
            }
            _view.AddClient += AddClient;
            _view.EditClient += EditClient;
        }
        private void EditClient(object sender, CellEventArgs e)
        {
            string phoneNumber = _view.GetPhoneNumber();
            Client editingClient = clients[clients.FindIndex(client => client.ContractNumber == int.Parse(e.contractnumber))];
            if (clients.FindAll(client => client.ClientNumber == phoneNumber).Count > 1)
            {
                throw new Exception("Данный номер телефона уже зарегистрирован. Внесите в соответствующую строку другой номер телефона.");
            }
            if (clients.FindAll(client => client.Fullname == _view.GetFullname()).Count > 1)
            {
                throw new Exception("Данное ФИО уже зарегистрировано. Внесите в соответствующую строку другое ФИО.");
            }
            if (clients.FindAll(client => client.Passport == _view.GetPassport()).Count > 1)
            {
                throw new Exception("Данные паспортные данные уже зарегистрированы. Внесите в соответствующую строку другие данные.");
            }
            editingClient.ClientNumber = _view.GetPhoneNumber();
            editingClient.Fullname = _view.GetFullname();
            editingClient.Passport = _view.GetPassport();
            editingClient.ClientTarif = tarifs[tarifs.FindIndex(tar => tar.TarifName == _view.GetTarifName())];
            clients[clients.FindIndex(client => client.ContractNumber == int.Parse(e.contractnumber))] = editingClient;
            JsonFileHelper.SaveClients(clients, "data.json");
            _view.MessageView("Клиент, " + editingClient.Fullname + ", изменён успешно!");
        }
        private void AddClient(object sender, EventArgs e) {
            if (_view.GetTarifName() == "")
            {
                throw new Exception("Выберите тариф!");
            }
            Random random = new Random();
            int contractNumber = random.Next(1, clients.Count+100);
            while (clients.FindAll(client => client.ContractNumber == contractNumber).Count != 0)
            {
                contractNumber = random.Next(1, clients.Count+100);
            }
            string phoneNumber = _view.GetPhoneNumber();
            if (clients.FindAll(client => client.ClientNumber == phoneNumber).Count != 0)
            {
                throw new Exception("Данный номер телефона уже зарегистрирован. Внесите в соответствующую строку другой номер телефона.");
            }
            if (clients.FindAll(client => client.Fullname == _view.GetFullname()).Count != 0)
            {
                throw new Exception("Данное ФИО уже зарегистрировано. Внесите в соответствующую строку другое ФИО.");
            }
            if (clients.FindAll(client => client.Passport == _view.GetPassport()).Count != 0)
            {
                throw new Exception("Данные паспортные данные уже зарегистрированы. Внесите в соответствующую строку другие данные.");
            }
            Client newclient = new Client()
            {
                Fullname = _view.GetFullname(),
                Passport = _view.GetPassport(),
                ContractDate = DateTime.Parse(_view.GetDogovorDate()),
                ClientTarif = tarifs[tarifs.FindIndex(tar => tar.TarifName == _view.GetTarifName())],
                Balance = 0,
                ContractNumber = contractNumber,
                ClientNumber = phoneNumber,
                phoneRecords = new List<PhoneRecord>()
            };
            newclient.Balance -= newclient.ClientTarif.TarifPrice;
            clients.Add(newclient);
            JsonFileHelper.SaveClients(clients, "data.json");
            _view.MessageView("Новый клиент, " + newclient.Fullname +", добавлен успешно!");
        }
    }
}
