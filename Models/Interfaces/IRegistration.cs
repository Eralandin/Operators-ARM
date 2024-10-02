using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface IRegistration
    {
        event EventHandler RegistrationAttempt;
        void RegistrationSuccess(Employee employee);
        void MessageView(string message);
        string GetPassword();
        string GetLogin();
        string GetRealLogin();
    }
}
