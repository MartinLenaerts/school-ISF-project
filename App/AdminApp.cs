using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class AdminApp
    {
        public AdminApp(Storage storage)
        {
            Storage = storage;
        }

        public Admin Admin { get; set; }

        public Storage Storage { get; set; }

        public bool Start()
        {
            AskAdminCredentials:
            if (!AskAdminCredentials()) // Ask Credentials
            {
                CustomConsole.PrintError("\n Username ou password incorrect");
                goto AskAdminCredentials;
            }

            Console.Clear();
            Admin = new Admin();
            BeginAdmin:
            PrintWelcomeMessage();
            var key = Console.ReadLine();
            switch (key.ToLower())
            {
                case "1": // Create Client
                    createClient:
                    if (!CreateClient())
                    {
                        CustomConsole.PrintError("Une erreur s'est produite");
                        goto createClient;
                    }

                    goto BeginAdmin;
                case "2": // Manage Client
                    if (!ManageClient())
                    {
                        CustomConsole.PrintError("Une erreur s'est produite");
                        goto BeginAdmin;
                    }

                    CustomConsole.PrintSuccess("Client modifié ! ");
                    goto BeginAdmin;
                case "3": // Verify user transactions
                    VerifyTransactions:
                    if (!GetTransactions())
                    {
                        CustomConsole.PrintError("Une erreur s'est produite");
                        goto VerifyTransactions;
                    }

                    goto BeginAdmin;
                case "4": // View all clients
                    if (!ViewAllClients()) CustomConsole.PrintError("Une erreur s'est produite");

                    goto BeginAdmin;
                case "5": // View all clients
                    if (!ViewMessages()) CustomConsole.PrintError("Une erreur s'est produite");

                    goto BeginAdmin;
                case "d": // Disconnect
                    Admin = null;
                    CustomConsole.PrintSuccess("Vous avez bien été déconnecté \n");
                    return false;
                case "q": // Quit
                    return true;
                default:
                    goto BeginAdmin;
            }
        }


        public void PrintWelcomeMessage()
        {
            Console.WriteLine("\n");
            CustomConsole.PrintStyleInfo("Welcome Admin");
            CustomConsole.PrintInfo("Enter : ");
            var choices = new List<Choice>
            {
                new() {Key = "1", Message = "to create a client"},
                new() {Key = "2", Message = "to manage client"},
                new() {Key = "3", Message = "to verify user transactions"},
                new() {Key = "4", Message = "to view a list of all clients"},
                new() {Key = "5", Message = "to view messages"},
                new() {Key = "D", Message = "to disconnect"},
                new() {Key = "Q", Message = "to quit"}
            };
            CustomConsole.PrintAllChoices(choices);
        }


        public bool AskAdminCredentials()
        {
            Console.Write("Please enter your username : ");
            var username = Console.ReadLine();

            Console.Write("Please enter your password : ");
            var password = CustomConsole.EnterPassword();

            Console.WriteLine("");
            return username == Admin.username && password == Admin.password;
        }


        public bool CreateClient()
        {
            if (Admin is null) return false;
            CustomConsole.Print("Please enter the new client information : ");

            Console.Write("LastName : ");
            var lastname = Console.ReadLine();

            Console.Write("FirstName : ");
            var firstname = Console.ReadLine();
            if (lastname == "" || firstname == "") return false;

            var pin = new Random().Next(1000, 10000);

            Client newClient = new Client {Lastname = lastname, Firstname = firstname, Pin = pin};
            bool res = Storage.DataAccess.CreateClient(newClient);

            if (res)
            {
                Console.Clear();
                CustomConsole.PrintSuccess("Client has been created ! ");
                CustomConsole.PrintStyleInfo("Please give this pin to the new client n° " +
                                             Storage.DataAccess.getLastId() + " : " + newClient.Pin);
            }

            return res;
        }

        public bool ManageClient()
        {
            if (Admin is null) return false;
            CustomConsole.Print("Please enter a client guid : ", false);
            var guid = Console.ReadLine();
            if (guid == "") return false;
            try
            {
                var client = Storage.DataAccess.GetClient(int.Parse(guid));
                PrintManageClientMessage(client);
                var key = Console.ReadLine();
                if (key == "1") return Storage.DataAccess.DeleteClient(client.Guid);
                if (key == "2") return UpdateClient(client);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        public void PrintManageClientMessage(Client c)
        {
            CustomConsole.PrintStyleInfo("Client found ! : " + c);
            CustomConsole.Print("Enter : ");
            CustomConsole.PrintChoice(new Choice() {Key = "1", Message = " to delete"});
            CustomConsole.PrintChoice(new Choice() {Key = "2", Message = " to update"});
        }

        public bool UpdateClient(Client c)
        {
            CustomConsole.Print("Tap : ");
            CustomConsole.PrintChoice(new Choice() {Key = "1", Message = "to unblock"});
            CustomConsole.PrintChoice(new Choice() {Key = "2", Message = "to block"});
            CustomConsole.PrintChoice(new Choice() {Key = "3", Message = "to change pin"});
            CustomConsole.PrintChoice(new Choice() {Key = "4", Message = "to reset tries"});
            CustomConsole.PrintChoice(new Choice()
                {Key = "5", Message = "to update client informations (firstname and lastname)"});
            CustomConsole.PrintChoice(new Choice() {Key = "6", Message = "to add currencies to a client"});
            var key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    c.unBlockedAndReset();
                    return Storage.DataAccess.UpdateClient(c);
                case "2":
                    c.Blocked = true;
                    return Storage.DataAccess.UpdateClient(c);
                case "3":
                    EnterPin:
                    Console.Write("Could you enter the new pin please : ");
                    var stringPin = Console.ReadLine();
                    int pin;
                    if (!int.TryParse(stringPin, out pin) || pin > 9999 || pin < 1000)
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterPin;
                    }

                    c.Pin = pin;
                    return Storage.DataAccess.UpdateClient(c);
                case "4":
                    return Storage.DataAccess.DeleteClient(c.Guid);
                case "5":
                    CustomConsole.Print("You can update client informations (if you don't want please press Enter ) ");
                    Console.Write("Firstname : ");
                    var firstname = Console.ReadLine();

                    Console.Write("Lastname : ");
                    var lastname = Console.ReadLine();

                    if (lastname != "") c.Lastname = lastname;
                    if (firstname != "") c.Firstname = firstname;

                    return Storage.DataAccess.UpdateClient(c);
                case "6":
                    ChooseCurrency:
                    Console.WriteLine(
                        "Enter main currency (example : EUR , USD ... ) (leave empty if you don't want update ) : ");
                    List<Currency> allCurrencies = Storage.DataAccess.GetAllCurrencies();
                    string currencyString = Console.ReadLine();

                    List<CurrencyClient> currencies = new List<CurrencyClient>();


                    Currency mainCurrency = allCurrencies.Find(c => c.Name == currencyString);

                    if (mainCurrency != null)
                        currencies.Add(new CurrencyClient()
                        {
                            Client = c,
                            Amount = 0,
                            Currency = mainCurrency
                        });

                    Console.WriteLine("Enter all other currencies separate by virgule (Example : EUR;USD;ZWL ... ) : ");
                    string currenciesString = Console.ReadLine();
                    string[] listStringCurrencies = currenciesString.Split(";");
                    foreach (string str in listStringCurrencies)
                    {
                        string strCurrency = str.Trim();
                        Currency currency = allCurrencies.Find(cu => cu.Name == strCurrency);
                        if (currency != null)
                            currencies.Add(new CurrencyClient()
                            {
                                Client = c,
                                Amount = 0,
                                Currency = currency
                            });
                    }

                    c.CurrencyClients = currencies;
                    return Storage.DataAccess.UpdateClient(c);

                default:
                    return false;
            }
        }

        public bool GetTransactions()
        {
            CustomConsole.Print("Enter : ");
            CustomConsole.Print("1 : to show all transactions");
            CustomConsole.Print("2 : to show transactions of one client");
            var key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    try
                    {
                        var transactions = Storage.DataAccess.GetAllTransactions();
                        string msg = "";
                        foreach (var transaction in transactions) msg += "" + transaction + "\n";
                        if (transactions.Count == 0) msg = "No transaction";
                        CustomConsole.PrintStyleInfo(msg);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }

                case "2":
                    EnterGuid:
                    Console.Write("Could you enter the guid of client : ");
                    var stringGuid = Console.ReadLine();
                    int guid;
                    if (!int.TryParse(stringGuid, out guid))
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterGuid;
                    }

                    try
                    {
                        var transactions = Storage.DataAccess.GetClientTransactions(guid);
                        foreach (var transaction in transactions) Console.WriteLine(transaction);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool ViewAllClients()
        {
            try
            {
                var clients = Storage.DataAccess.GetAll();
                string header = "CLIENTS  NAMES : ";
                string details = "DETAILS : \n\n";
                foreach (var client in clients)
                {
                    header += client.Firstname + " " + client.Lastname + " , ";
                    details += client + "\n";
                }

                CustomConsole.PrintStyleInfo(header.Remove(header.Length - 2) + "\n\n" + details);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ViewMessages()
        {
            try
            {
                List<Message> messages = Storage.DataAccess.getMessages();
                string res = "";
                foreach (var message in messages)
                {
                    res += message;
                }

                CustomConsole.PrintStyleInfo(res);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}