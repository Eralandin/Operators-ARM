using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Classes
{
    public enum PhoneRecordType
    {
        SMS,
        Звонок
    }
    public enum IOType
    {
        Входящая,
        Исходящая
    }
    public enum ContrAgentType
    {
        Внутресетевой,
        Внешнесетевой
    }
    public class PhoneRecord
    {
        private float elapsedTime;
        private DateTime momentOfRecord;
        private string contrAgent;
        private float balanceBefore;
        private float balanceAfter;
        private PhoneRecordType recordType;
        private IOType recordIOType;
        private ContrAgentType recordContrAgentType;

        public float BalanceBefore
        {
            get { return balanceBefore; }
            set
            {
                if (value <= 0)
                {
                    throw new OperationCanceledException("Операция не может быть зарегестрирована! Недостаточно средств!");
                }
                else
                {
                    balanceBefore = value;
                };
            }
        }
        public float BalanceAfter
        {
            get { return balanceAfter; }
            set { balanceAfter = value; } 
        }
        public PhoneRecordType RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }

        public IOType RecordIOType
        {
            get { return recordIOType; }
            set { recordIOType = value; }
        }

        public ContrAgentType RecordContrAgentType
        {
            get { return recordContrAgentType; }
            set { recordContrAgentType = value; }
        }
        public float ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentNullException("Прошедшее время операции не может быть меньше нуля!");
                }
                else if(recordType == PhoneRecordType.SMS)
                {
                    elapsedTime = 0;
                }
                else
                {
                    elapsedTime = value;
                }
            }
        }
        public string MomentOfRecord
        {
            get { return momentOfRecord.ToString(); }
            set 
            { 
                if (!DateTime.TryParse(value, out DateTime result))
                {
                    throw new ArgumentException("Неверное представление момента записи!");
                }
                else
                {
                    momentOfRecord = DateTime.Parse(value);
                }
            }
        }
        public string ContrAgent
        {
            get { return contrAgent; }
            set {
                if (value == "")
                {
                    throw new ArgumentNullException("Контрагент не может быть пустым значением! Введите значение контрагента!");
                }
                else
                {
                    contrAgent = value;
                }
            }
        }
    }
}
