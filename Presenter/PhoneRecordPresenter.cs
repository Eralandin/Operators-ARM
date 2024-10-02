using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.Models.Interfaces;
using Org.BouncyCastle.Crypto.Tls;

namespace KursovayaKapitonova.Presenter
{
    public class PhoneRecordPresenter
    {
        public List<Client> clients;
        private int contractNumber;
        private readonly IPhoneRecord _view;
        public PhoneRecordPresenter(IPhoneRecord view, int contractnumber)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            contractNumber = contractnumber;
            clients = JsonFileHelper.LoadClients();
            _view.AddRecord += AddRecord;
        }
        private void AddRecord(object sender, EventArgs e)
        {
            if (_view.GetContrAgent() == "")
            {
                throw new Exception("Введите значение контрагента!");
            }
            ContrAgentType type;
            if (clients.FindAll(cl => cl.ClientNumber == _view.GetContrAgent()).Count > 0)
            {
                _view.MessageView("Номер контрагента найден в базе! Применяется тип 'Внутресетевой'");
                type = ContrAgentType.Внутресетевой;
            }
            else
            {
                type = _view.GetContrAgentType();
            }
            PhoneRecord record = new PhoneRecord()
            {
                RecordType = _view.GetPhoneRecordType(),
                RecordIOType = _view.GetIOType(),
                RecordContrAgentType = type,
                ElapsedTime = _view.GetElapsedTime(),
                MomentOfRecord = _view.GetMomentOfRecord(),
                ContrAgent = _view.GetContrAgent(),
                BalanceBefore = clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)].Balance,
                BalanceAfter = Client.Calculate(_view.GetPhoneRecordType(),_view.GetIOType(), _view.GetContrAgentType(),_view.GetElapsedTime(), clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)], clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)].Balance)
            };
            if (clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)].ClientNumber == record.ContrAgent)
            {
                throw new Exception("Невозможно зарегистрировать данную операцию, так как операция направлена на номер пользователя!");
            }
            clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)].Balance = record.BalanceAfter;
            clients[clients.FindIndex(cl => cl.ContractNumber == contractNumber)].phoneRecords.Add(record);
            JsonFileHelper.SaveClients(clients, "data.json");
        }
    }
}
