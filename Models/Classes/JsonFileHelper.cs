using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KursovayaKapitonova.Models.Classes
{
    public static class JsonFileHelper
    {
        private static string FilePath = "data.json";

        public static void SaveData(List<Client> clients, List<Tarif> tarifs, List<Employee> employees)
        {
            var data = new
            {
                Clients = clients,
                Tarifs = tarifs,
                Employees = employees
            };

            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
        public static (List<Client>, List<Tarif>, List<Employee>) LoadData()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                var clients = data.Clients.ToObject<List<Client>>();
                var tarifs = data.Tarifs.ToObject<List<Tarif>>();
                var employees = data.Employees.ToObject<List<Employee>>();

                return (clients, tarifs, employees);
            }
            else
            {
                return (new List<Client>(), new List<Tarif>(), new List<Employee>());
            }
        }
        public static List<Client> LoadClients()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);
                return data.Clients.ToObject<List<Client>>();
            }
            else
            {
                return new List<Client> { };
            }
        }
        public static List<Employee> LoadEmployees()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);
                return data.Employees.ToObject<List<Employee>>();
            }
            else
            {
                return new List<Employee> { };
            }
        }
        public static List<Tarif> LoadTarifs()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);
                return data.Tarifs.ToObject<List<Tarif>>();
            }
            else
            {
                return new List<Tarif>();
            }
        }
        public static void SaveClients(List<Client> clients, string filePath)
        {
            var(loadedClients, loadedTarifs, loadedEmployees) = LoadData();
            loadedClients = clients;
            SaveData(loadedClients, loadedTarifs, loadedEmployees);
        }
        public static void SaveEmployees(List<Employee> employees, string filePath)
        {
            var (loadedClients, loadedTarifs, loadedEmployees) = LoadData();
            loadedEmployees = employees;
            SaveData(loadedClients, loadedTarifs, loadedEmployees);
        }
        public static void SaveTarifs(List<Tarif> tarifs, string filePath)
        {
            var (loadedClients, loadedTarifs, loadedEmployees) = LoadData();
            loadedTarifs = tarifs;
            SaveData(loadedClients, loadedTarifs, loadedEmployees);
        }
        public static void InitializeDataIfFileNotExists()
        {
            if (!File.Exists(FilePath))
            {
                // Создание пустых списков данных
                var emptyClientsList = new List<Client>();
                var emptyEmployeesList = new List<Employee>();
                var emptyTarifsList = new List<Tarif>();

                // Сохранение списков в JSON-файл
                SaveData(emptyClientsList, emptyTarifsList, emptyEmployeesList);
            }
        }
    }
}
