using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovayaKapitonova
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            JsonFileHelper.InitializeDataIfFileNotExists();
            List<Employee> employees = JsonFileHelper.LoadEmployees();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm(employees));
        }
    }
}
