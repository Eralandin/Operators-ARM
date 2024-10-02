using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Presenter
{
    public class LoginFormPresenter
    {
        private readonly ILogin _view;
        private List<Employee> _employees;
        public LoginFormPresenter(ILogin view, List<Employee> employees)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _employees = employees;
            _view.LoginAttempt += LoginAttempt;
        }
        private void LoginAttempt(object sender, EventArgs e)
        {
            if (_employees.Count != 0) 
            {
                try
                {
                    if (_view.GetLogin() == "")
                    {
                        throw new Exception("Введите логин!");
                    }
                    if (_view.GetPassword() == "") 
                    {
                        throw new Exception("Введите пароль!");
                    }
                    string password = _view.GetPassword();
                    Employee currentUser = _employees.FirstOrDefault(emp => emp.Login == _view.GetLogin());
                    if (currentUser != null)
                    {
                        if (ValidatePassword(password, currentUser.Salt, currentUser.Password))
                        {
                            _view.LoginSuccess(currentUser);
                        }
                        else
                        {
                            throw new Exception("Введён неправильный пароль! В доступе отказано.");
                        }
                    }
                    else
                    {
                        _view.MessageView("Такого пользователя не существует или введён неверный логин!");
                    }
                }
                catch (Exception ex)
                {
                    _view.MessageView(ex.Message);
                }
            }
            else
            {
                //Регистрация нового пользователя
                _view.Registration(_employees);
            }
        }
        private bool ValidatePassword(string enteredPassword, string salt, string hashedPassword)
        {
            string hashedEnteredPassword = Employee.HashPassword(enteredPassword, salt);
            return hashedEnteredPassword == hashedPassword;
        }
    }
}
