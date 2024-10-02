using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovayaKapitonova.Presenter
{
    public class RegistrationPresenter
    {
        private readonly IRegistration _view;
        private List<Employee> _employees;
        public RegistrationPresenter(IRegistration view, List<Employee> employees)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _employees = employees;
            _view.RegistrationAttempt += RegistrationAttempt;
        }
        private void RegistrationAttempt(object sender, EventArgs e)
        {
            try
            {
                string login = _view.GetLogin();
                string password = _view.GetPassword();
                string realLogin = _view.GetRealLogin();
                bool BadSymbolsInside = false;
                for (int i = 0; i < _view.GetPassword().Length; i++)
                {
                    if (!((password.ToCharArray()[i] > 'A' && password.ToCharArray()[i] < 'Z') ||
                        (password.ToCharArray()[i] > 'a' && password.ToCharArray()[i] < 'z') ||
                        (password.ToCharArray()[i] > '0' && password.ToCharArray()[i] < '9')) ||
                        (password.Length < 4 && password.Length > 20))
                    {
                        BadSymbolsInside = true;
                    }
                }
                if (BadSymbolsInside)
                {
                    throw new ArgumentException("Вы используете недопустимые символы \nДля логина можно использовать только латинские буквы и цифры\nПароль должен быть длиннее 4 символов и не больше 20 символов!");
                }
                else if (password.Length == 0)
                {
                    throw new ArgumentException("Введите пароль!");
                }
                else if (_employees.Any(emp => emp.Login == realLogin))
                {
                    throw new ArgumentException("Данный логин занят!");
                }
                else if(_employees.Any(emp => emp.Fullname == login))
                {
                    throw new ArgumentException("Данное ФИО занято!");
                }
                
                string salt = Employee.GenerateSalt();
                string hashedPassword = Employee.HashPassword(password, salt);
                _employees.Add(new Employee() { Login = realLogin, Fullname = login, Salt = salt, Password = hashedPassword });
                JsonFileHelper.SaveEmployees(_employees, "data.json");
                _view.RegistrationSuccess(_employees[0]);
            }
            catch (Exception ex)
            {
                _view.MessageView(ex.Message);
            }
        }
    }
}
