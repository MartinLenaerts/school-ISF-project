using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Bank.Access;
using Bank.Models;
using Bank.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bank
{
    public class ClientApp
    {
        public Client Client { get; set; }
        public Storage Storage { get; set; }

        public bool Start()
        {
            BeginClient:
            try
            {
                if (Client is null && !AskCredentials()) throw new Exception("\r\n Guid or pin incorrect");
                PrintWelcomeMessage();
                string key = Console.ReadLine();
                switch (key)
                {
                    case "1": //  view GUID and credentials
                        CustomConsole.PrintInfo(Client.ToString());
                        goto BeginClient;
                    case "2": // total amount in preferred currency
                        if (!ViewTotalAmount()) // Ask Credentials
                        {
                            throw new Exception("\r\n An error occured");
                        }

                        goto BeginClient;
                    case "3": // retrieve money from currency
                        RetrieveMoney:
                        if (!RetrieveMoney())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto RetrieveMoney;
                        }

                        goto BeginClient;
                    case "4": // add money from currency
                        AddMoney:
                        if (!AddMoney())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto AddMoney;
                        }

                        goto BeginClient;
                    case "5": // exchange money
                        ExchangeMoney:
                        if (!ExchangeMoney())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto ExchangeMoney;
                        }

                        goto BeginClient;
                    case "6": // transfert money
                        TransfertMoney:
                        if (!TransfertMoney())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto TransfertMoney;
                        }

                        goto BeginClient;
                    case "7": // change pin
                        ChangePin:
                        if (!ChangePin())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto ChangePin;
                        }

                        goto BeginClient;
                    case "8": // Leave message for admin
                        LeaveMsg:
                        if (!LeaveMsg())
                        {
                            CustomConsole.PrintError("\r\n An eroor occured");
                            goto LeaveMsg;
                        }

                        goto BeginClient;
                    case "d": // Disconnect
                        Client = null;
                        CustomConsole.PrintSuccess("Vous avez bien été déconnecté \r\n");
                        return false;
                    case "D": // Disconnect
                        Client = null;
                        CustomConsole.PrintSuccess("Vous avez bien été déconnecté \r\n");
                        return false;
                    case "q": // Quit
                        return true;
                        break;
                    case "Q": // Quit
                        return true;
                        break;
                    default:
                        goto BeginClient;
                }
            }
            catch (Exception e)
            {
                CustomConsole.PrintError(e.Message);
                return false;
            }
        }

        public bool AskCredentials()
        {
            Console.Write("Veuillez entrer votre guid : ");
            string stringGuid = Console.ReadLine();

            Console.Write("Veuillez entrer votre pin : ");
            string stringPin = CustomConsole.EnterPassword();

            int guid;
            int pin;
            if (!Int32.TryParse(stringGuid, out guid) || !Int32.TryParse(stringPin, out pin)) return false;
            Client client = Storage.DataAccess.GetClient(guid);
            Console.Write("YES");
            if (client is null || client.Pin != pin) return false;
            Client = client;
            return true;
        }

        public void PrintWelcomeMessage()
        {
            CustomConsole.PrintInfo("Welcome Client");
            CustomConsole.PrintInfo("Enter : ");
            CustomConsole.PrintAllChoices(new List<Choice>()
            {
                new() {Key = "1", Message = "to view GUID and credentials"},
                new() {Key = "2", Message = "to view total amount in preferred currency"},
                new() {Key = "3", Message = "to retrieve money from currency"},
                new() {Key = "4", Message = "to add money to currency"},
                new() {Key = "5", Message = "to exchange between currencies"},
                new() {Key = "6", Message = "to transfer money to an another client"},
                new() {Key = "7", Message = "to leave message for admin"},
                new() {Key = "D", Message = "to disconnect"},
                new() {Key = "Q", Message = "to quit"}
            });
        }

        public bool ViewTotalAmount()
        {
            CurrencyClient currencyClient = Storage.DataAccess.GetMainCurrencyClient(Client.Guid);
            if (currencyClient == null)
            {
                CustomConsole.Print("Aucune devise pour ce client");
                return false;
            }

            CustomConsole.PrintInfo(currencyClient.Amount + " " + currencyClient.Currency.Name);
            return true;
        }
        
        
        






        public bool RetrieveMoney()
        {
            try
            {
                List<CurrencyClient> currenciesClient = Storage.DataAccess.GetCurrenciesClient(Client.Guid);
                if (currenciesClient.Count == 0)
                {
                    CustomConsole.Print("Aucune devise pour ce client");
                    return false;
                }

                ChooseCurrency:
                Console.Write("Choose the currency : ");
                int index = 1;
                foreach (var currencyC in currenciesClient)
                {
                    CustomConsole.Print(index + " : " + currencyC.Currency.Name);
                    index++;
                }

                string stringCurrency = Console.ReadLine();
                int currency;
                if (!Int32.TryParse(stringCurrency, out currency) || currency < 1 || currency > index)
                {
                    Console.WriteLine("Wrong key ! ");
                    goto ChooseCurrency;
                }

                CustomConsole.Print("");
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
                List<CurrencyClient> currenciesClient = Storage.DataAccess.GetCurrenciesClient(Client.Guid);
                ChooseCurrency:
                Console.Write("Choose the currency : ");
                int index = 1;
                foreach (var currencyC in currenciesClient)
                {
                    CustomConsole.Print(index + " : " + currencyC.Currency.Name);
                    index++;
                }

                string stringCurrency = Console.ReadLine();
                int currency;
                if (!Int32.TryParse(stringCurrency, out currency) || currency < 1 || currency > index)
                {
                    Console.WriteLine("Wrong key ! ");
                    goto ChooseCurrency;
                }

                CustomConsole.Print("");
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