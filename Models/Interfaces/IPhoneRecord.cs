using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface IPhoneRecord
    {
        event EventHandler AddRecord;
        void MessageView(string message);
        int GetContractNumber();
        string GetMomentOfRecord();
        float GetElapsedTime();
        ContrAgentType GetContrAgentType();
        IOType GetIOType();
        PhoneRecordType GetPhoneRecordType();
        string GetContrAgent();
    }
}
