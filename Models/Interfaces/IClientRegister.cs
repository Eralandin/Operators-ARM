using KursovayaKapitonova.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface IClientRegister
    {
        event EventHandler AddClient;
        event EventHandler<CellEventArgs> EditClient;
        string GetDogovorDate();
        string GetPassport();
        string GetTarifName();
        string GetPhoneNumber();
        string GetFullname();
        void MessageView(string message);

    }
}
