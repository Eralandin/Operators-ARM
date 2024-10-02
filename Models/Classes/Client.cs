using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovayaKapitonova.Models.Classes;

namespace KursovayaKapitonova.Models.Classes
{
    public class Client
    {
        private string fullname;
        private string passport;
        private Tarif clientTarif;
        private float balance;
        private bool isActive;
        private int contractNumber;
        private DateTime contractDate;
        private string clientNumber;
        public List<PhoneRecord> phoneRecords;
        
        public string Fullname
        {
            get { return fullname; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("ФИО абонента не может быть пустым!");
                }
                else
                {
                    fullname = value;
                }
            }
        }
        public int ContractNumber
        {
            get { return contractNumber; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Номер договора не может быть меньше или равен нулю!");
                }
                else
                {
                    contractNumber = value;
                }
            }
        }
        public string Passport
        {
            get { return passport; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Паспортные данные не могут быть пустыми!");
                }
                else
                {
                    passport = value;
                }
            }
        }
        public Tarif ClientTarif
        {
            get { return clientTarif; }
            set 
            { 
                if (value == null)
                    throw new ArgumentException("Тариф не может быть пустым!");
                clientTarif = value; 
            }
        }
        public float Balance
        {
            get { return balance; }
            set 
            {
                balance = value;
            }
        }
        public static float Calculate(PhoneRecordType type, IOType iotype, ContrAgentType contrAgentType, float elapsedseconds, Client client, float balancebefore)
        {
            if(client.isActive == false)
            {
                if (type == PhoneRecordType.SMS && iotype == IOType.Входящая)
                {
                    return client.Balance;
                }
                else if (type == PhoneRecordType.SMS && iotype == IOType.Исходящая)
                {
                    return client.Balance - client.ClientTarif.SmsPrice;
                }
                else if (contrAgentType == ContrAgentType.Внутресетевой && iotype == IOType.Входящая && type == PhoneRecordType.Звонок)
                {
                    return client.Balance;
                }
                else if (contrAgentType == ContrAgentType.Внутресетевой && iotype == IOType.Исходящая && type == PhoneRecordType.Звонок)
                {
                    return client.Balance;
                }
                else if (contrAgentType == ContrAgentType.Внешнесетевой && iotype == IOType.Исходящая && type == PhoneRecordType.Звонок)
                {
                    return client.Balance - client.ClientTarif.OuterCallPrice * elapsedseconds;
                }
                else if (contrAgentType == ContrAgentType.Внешнесетевой && iotype == IOType.Входящая && type == PhoneRecordType.Звонок)
                {
                    return client.Balance - client.ClientTarif.InnerCallPrice * elapsedseconds;
                }
                else
                {
                    throw new Exception("Непредусмотренный тип операции!");
                }
            }
            else
            {
                throw new ArithmeticException("Договор неактивен!");
            }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime ContractDate
        {
            get { return contractDate; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentException("Дата создания договора не может быть пустой!");
                }
                else
                {
                    contractDate = value;
                }
            }
        }
        public string ClientNumber
        {
            get { return clientNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Необходимо задать телефонный номер для клиента!");
                }
                else
                {
                    clientNumber = value;
                }
            }
        }
        public void Activate()
        {
            IsActive = !IsActive;
        }
        public bool CheckPhoneNumber(string checkingPhoneNumber)
        {
            if (JsonFileHelper.LoadClients().Find(x => x.clientNumber == checkingPhoneNumber) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
