using KursovayaKapitonova.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursovayaKapitonova.Models.Classes;

namespace KursovayaKapitonova.Models.Interfaces
{
    public interface IMain
    {
        event EventHandler LoadData;
        event EventHandler PDFTarifs;
        event EventHandler PDFClients;
        event EventHandler BalanceMinusTarifPrice;
        event EventHandler<CellEventArgs> DogovorPause;
        event EventHandler<CellEventArgs> AddBalance;
        event EventHandler<CellEventArgs> DeleteClient;
        event EventHandler ReloadLists;
        event EventHandler<CellEventArgs> PDFDetalization;
        event EventHandler SortByContract;
        event EventHandler SortByNumber;
        event EventHandler SortByTarif;
        event EventHandler SortByFullname;

        void MessageView(string message);
        void ShowData(List<Client> clients);
    }
}
