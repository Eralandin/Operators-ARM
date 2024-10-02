using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.Models.Interfaces;
using KursovayaKapitonova.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovayaKapitonova.Presenter
{
    public class EmpTarifsPresenter
    {
        private Employee currentUser;
        public List<Employee> employees;
        public List<Tarif> tarifs;
        private readonly IEmpTarifs _view;

        public EmpTarifsPresenter(IEmpTarifs view, List<Employee> employeesIn, Employee currentUser)
        {
            employees = employeesIn;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _view.Add += AddEmp;
            _view.Delete += DeleteEmp;
            _view.CheckValidate += CheckValidate;
            this.currentUser = currentUser;
        }
        public EmpTarifsPresenter(IEmpTarifs view, List<Tarif> tarifsIn, Employee currentUser)
        {
            employees = JsonFileHelper.LoadEmployees();
            tarifs = tarifsIn;
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _view.AddTarif += AddTarif;
            _view.EditTarif += EditTarif;
            _view.DeleteTarif += DeleteTarif;
            _view.CheckValidate += CheckValidate;
            this.currentUser = currentUser;
        }
        private void AddTarif(object sender, Tarif tarif)
        {

        }
        private void EditTarif(object sender, CellEventArgs e)
        {
            try
            {
                tarifs = JsonFileHelper.LoadTarifs();
                if (tarifs.Count > 0)
                {
                    Tarif editingTarif = tarifs[tarifs.FindIndex(tarif => tarif.TarifName == e.tarifname)];
                    _view.EditTarifToForm(editingTarif);
                }
            }
            catch (Exception ex)
            {
                _view.MessageView(ex.Message);
            }
        }
        private void DeleteTarif(object sender, CellEventArgs e)
        {
            try
            {
                tarifs = JsonFileHelper.LoadTarifs();
                if (tarifs.Count > 0)
                {
                    tarifs.RemoveAt(tarifs.FindIndex(tarif => tarif.TarifName == e.tarifname));
                    JsonFileHelper.SaveTarifs(tarifs, "data.json");
                    tarifs = JsonFileHelper.LoadTarifs();
                }
            }
            catch (Exception ex)
            {
                _view.MessageView(ex.Message);
            }
        }
        private void AddEmp(object sender, EventArgs e)
        {
            try
            {
                _view.AddEmpForm(employees);
            }
            catch (Exception ex)
            {
                _view.MessageView(ex.Message);
            }
        }
        private void DeleteEmp(object sender, CellEventArgs e)
        {
            employees = JsonFileHelper.LoadEmployees();
            if (employees.Count > 1)
            {
                if (employees[employees.FindIndex(emp => emp.Fullname == e.fullname)].Fullname != currentUser.Fullname)
                {
                    employees.RemoveAt(employees.FindIndex(emp => emp.Fullname == e.fullname));
                    JsonFileHelper.SaveEmployees(employees, "data.json");
                    employees = JsonFileHelper.LoadEmployees();
                }
                else
                {
                    throw new InvalidOperationException("Невозможно удалить текущего пользователя!");
                }
            }
            else
            {
                throw new Exception("Невозможно удалить последнего пользователя!");
            }
        }
        private void CheckValidate(object sender, string e)
        {
            employees = JsonFileHelper.LoadEmployees();
            if (employees.Count > 0)
            {
                if (employees[employees.FindIndex(emp => emp.Fullname == currentUser.Fullname)].Password == Employee.HashPassword(e, employees[employees.FindIndex(emp => emp.Fullname == currentUser.Fullname)].Salt))
                {

                }
                else
                {
                    throw new Exception("Введён неправильный пароль! В доступе отказано.");
                }
            }
            else
            {
                throw new Exception("Неопознанная ошибка! Пользователей не обнаружено!");
            }
            
        }
    }
}
