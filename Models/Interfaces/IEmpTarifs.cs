using KursovayaKapitonova.Models.Classes;
using KursovayaKapitonova.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface IEmpTarifs
    {
        event EventHandler Add;
        event EventHandler<CellEventArgs> Delete;
        event EventHandler<string> CheckValidate;
        event EventHandler<Tarif> AddTarif;
        event EventHandler<CellEventArgs> DeleteTarif;
        event EventHandler<CellEventArgs> EditTarif;

        void MessageView(string message);
        void AddEmpForm(List<Employee> employees);
        void EditTarifToForm(Tarif tarif);
    }
}
