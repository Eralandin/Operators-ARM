using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface ILogin
    {
        event EventHandler LoginAttempt;

        string GetPassword();
        string GetLogin();

        void MessageView(string message);
        void LoginSuccess(Employee currentUser);
        void Registration(List<Employee> employees);
    }
}
