using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Classes
{
    public class Employee
    {
        private string salt;
        private string password;
        private string fullname;
        private string login;
        public string Fullname
        {
            get { return fullname; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("ФИО сотрудника не может быть пустым!");
                }
                else
                {
                    fullname = value;
                }
            }
        }
        public string Login
        {
            get { return login; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Логин сотрудника не может быть пустым!");
                }
                else if (!Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
                {
                    throw new ArgumentException("Логин сотрудника должен содержать только символы латиницы и цифры!");
                }
                else
                {
                    login = value;
                }
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Пароль сотрудника не может быть пустым!");
                }
                else
                {
                    password = value;
                }
            }
        }
        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }
        public static string GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
        public static string HashPassword(string password, string salt)
        {
            if (password != "")
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] saltBytes = Convert.FromBase64String(salt);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                    byte[] hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                    return Convert.ToBase64String(hashedPasswordBytes);
                }
            }
            else
            {
                throw new Exception("Введите пароль!");
            }
        }
    }
}
