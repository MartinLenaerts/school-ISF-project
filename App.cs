using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Access;
using Bank.Context;
using Bank.Models;

namespace Bank
{
    public class App
    {
        public Admin Admin { get; set; }
        public Client ClientCurrent { get; set; }

        public Storage Storage;

        public App(bool forceJson = false)
        {
            Storage = new Storage(forceJson);
        }

        public void PrintWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Print("Welcome to The Bank");
            Console.ForegroundColor = ConsoleColor.Black;
            Print("Enter : ");
            Print("1 : if you are an Admin");
            Print("2 : if you are a Client");
            Print("Q : to quit");
        }

        public void PrintWelcomeAdminMessage()
        {
            Print("Welcome Admin");
            Print("Enter : ");
            Print("1 : to create a client");
            Print("2 : to manage client");
            Print("3 : to verify user transactions");
            Print("4 : to view a list of all clients");
            Print("D : to disconnect");
            Print("Q : to quit");
        }

        void Print(string msg)
        {
            Console.WriteLine(msg);
        }


        public bool AskAdminCredentials()
        {
            Console.Write("Veuillez entrer votre nom d'utilsateur : ");
            string username = Console.ReadLine();

            Console.Write("Veuillez entrer votre mot de passe : ");
            string password = enterPassword();

            return username == Admin.username && password == Admin.password;
        }

        private string enterPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }

        public bool CreateClient()
        {
            if (Admin is null) return false;
            Print("Veuillez entrer les informations du nouveau client : ");
            Console.Write("Nom : ");
            string lastname = Console.ReadLine();
            Console.Write("Prenom : ");
            string firstname = Console.ReadLine();
            if (lastname == "" || firstname == "") return false;
            int pin = new Random().Next(1000, 10000);
            return Storage.DataAccess.CreateClient(new Client()
                {Lastname = lastname, Firstname = firstname, Pin = pin});
        }

        public bool ManageClient()
        {
            if (Admin is null) return false;
            Print("Veuillez entrer le guid d'un client : ");
            string guid = Console.ReadLine();
            if (guid == "") return false;
            try
            {
                Client client = Storage.DataAccess.GetClient(Int32.Parse(guid));
                PrintManageClientMessage(client);
                string key = Console.ReadLine();
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
            Print("Client found ! : " + c);
            Print("Enter : ");
            Print("1 : to delete");
            Print("2 : to update");
        }

        public bool UpdateClient(Client c)
        {
            Print("Enter : ");
            Print("1 : to unblock");
            Print("2 : to block");
            Print("3 : to change pin");
            Print("4 : to reset tries");
            Print("5 : to delete client");
            Print("5 : to update client informations (firstname and lastname)");
            string key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    c.Blocked = false;
                    return Storage.DataAccess.UpdateClient(c);
                case "2":
                    c.Blocked = true;
                    return Storage.DataAccess.UpdateClient(c);
                case "3":
                    EnterPin:
                    Console.Write("Could you enter the new pin please : ");
                    string stringPin = Console.ReadLine();
                    int pin;
                    if (!Int32.TryParse(stringPin, out pin) || pin > 9999 || pin < 1000)
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterPin;
                    }

                    c.Pin = pin;
                    return Storage.DataAccess.UpdateClient(c);
                case "4":
                    return Storage.DataAccess.DeleteClient(c.Guid);
                case "5":
                    Print("You can update client informations (if you don't want please press Enter ) ");
                    Console.Write("Firstname : ");
                    string firstname = Console.ReadLine();

                    Console.Write("Lastname : ");
                    string lastname = Console.ReadLine();

                    if (lastname != "") c.Lastname = lastname;
                    if (firstname != "") c.Lastname = firstname;

                    return Storage.DataAccess.UpdateClient(c);
                default:
                    return false;
            }
        }

        public bool GetTransactions()
        {
            Print("Enter : ");
            Print("1 : to show all transactions");
            Print("2 : to show transactions of one client");
            string key = Console.ReadLine();
            switch (key)
            {
                case "1":
                    try
                    {
                        List<Transaction> transactions = Storage.DataAccess.GetAllTransactions();
                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine(transaction);
                        }

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
                    string stringGuid = Console.ReadLine();
                    int guid;
                    if (!Int32.TryParse(stringGuid, out guid))
                    {
                        Console.Write("Could you enter a correct pin please : ");
                        goto EnterGuid;
                    }

                    try
                    {
                        List<Transaction> transactions = Storage.DataAccess.GetClientTransactions(guid);
                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine(transaction);
                        }

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
                List<Client> clients = Storage.DataAccess.GetAll();
                foreach (var client in clients)
                {
                    Console.WriteLine(client);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AskUserCredentials()
        {
            Console.Write("Veuillez entrer votre guid : ");
            string stringGuid = Console.ReadLine();

            Console.Write("Veuillez entrer votre pin : ");
            string stringPin = enterPassword();

            int guid;
            int pin;
            if (!Int32.TryParse(stringGuid, out guid) || !Int32.TryParse(stringPin, out pin)) return false;
            Client client = Storage.DataAccess.GetClient(guid);
            if (client is null || client.Pin != pin) return false;
            ClientCurrent = client;
            return true;
        }


        public void PrintWelcomeClientMessage()
        {
            Print("Welcome Client");
            Print("Enter : ");
            Print("1 : to view GUID and credentials");
            Print("2 : to view total amount in preferred currency");
            Print("3 : to retrieve money from currency");
            Print("4 : to add money to currency");
            Print("5 : to exchange between currencies");
            Print("6 : to transfer money to an another client");
            Print("7 : to leave message for admin");
            Print("D : to disconnect");
            Print("Q : to quit");
        }

        public bool ViewTotalAmount()
        {
            try
            {
                CurrencyClient currencyClient = Storage.DataAccess.GetMainCurrencyClient(ClientCurrent.Guid);
                Print(currencyClient.Amount + " " + currencyClient.Currency.Name);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool RetrieveMoney()
        {
            try
            {
                List<CurrencyClient> currenciesClient = Storage.DataAccess.GetCurrenciesClient(ClientCurrent.Guid);
                ChooseCurrency:
                Console.Write("Choose the currency : ");
                int index = 1;
                foreach (var currencyC in currenciesClient)
                {
                    Print(index + " : " + currencyC.Currency.Name);
                    index++;
                }

                string stringCurrency = Console.ReadLine();
                int currency;
                if (!Int32.TryParse(stringCurrency, out currency) || currency < 1 || currency > index)
                {
                    Console.WriteLine("Wrong key ! ");
                    goto ChooseCurrency;
                }

                Print("");
                CurrencyClient cc = currenciesClient.ElementAt(currency);
                EnterAmount:
                Console.Write("Enter the amount : ");
                string stringAmount = Console.ReadLine();
                double amount;
                if (!Double.TryParse(stringAmount, out amount) || amount < 1 || cc.Amount < amount)
                {
                    Console.WriteLine("Wrong amount ! ");
                    goto EnterAmount;
                }

                cc.Amount -= amount;
                Storage.DataAccess.UpdateCurrencyClient(cc);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddMoney()
        {
            try
            {
                List<CurrencyClient> currenciesClient = Storage.DataAccess.GetCurrenciesClient(ClientCurrent.Guid);
                ChooseCurrency:
                Console.Write("Choose the currency : ");
                int index = 1;
                foreach (var currencyC in currenciesClient)
                {
                    Print(index + " : " + currencyC.Currency.Name);
                    index++;
                }

                string stringCurrency = Console.ReadLine();
                int currency;
                if (!Int32.TryParse(stringCurrency, out currency) || currency < 1 || currency > index)
                {
                    Console.WriteLine("Wrong key ! ");
                    goto ChooseCurrency;
                }

                Print("");
                CurrencyClient cc = currenciesClient.ElementAt(currency);
                EnterAmount:
                Console.Write("Enter the amount : ");
                string stringAmount = Console.ReadLine();
                double amount;
                if (!Double.TryParse(stringAmount, out amount) || amount < 1)
                {
                    Console.WriteLine("Wrong amount ! ");
                    goto EnterAmount;
                }

                cc.Amount += amount;
                Storage.DataAccess.UpdateCurrencyClient(cc);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ExchangeMoney()
        {
            throw new NotImplementedException();
        }

        public bool TransfertMoney()
        {
            throw new NotImplementedException();
        }

        public bool ChangePin()
        {
            throw new NotImplementedException();
        }

        public bool LeaveMsg()
        {
            throw new NotImplementedException();
        }
    }
}