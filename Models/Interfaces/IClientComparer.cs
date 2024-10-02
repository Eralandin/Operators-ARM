using KursovayaKapitonova.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursovayaKapitonova.Models.Interfaces
{
    public class ClientComparer : IComparer<Client>
    {
        // Сортировка по номеру договора
        public int Compare(Client x, Client y)
        {
            return x.ContractNumber.CompareTo(y.ContractNumber);
        }

        // Сортировка по ФИО
        public int CompareFullname(Client x, Client y)
        {
            return String.Compare(x.Fullname, y.Fullname);
        }

        // Сортировка по названию тарифа
        public int CompareTarif(Client x, Client y)
        {
            return String.Compare(x.ClientTarif.TarifName, y.ClientTarif.TarifName);
        }

        // Сортировка по номеру абонента
        public int CompareNumber(Client x, Client y)
        {
            return String.Compare(x.ClientNumber, y.ClientNumber);
        }
    }
}
