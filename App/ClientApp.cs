using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Models;
using Bank.Utils;

namespace Bank
{
    public class ClientApp
    {
        private static bool wrongCredentials;

        public ClientApp(Storage storage)
        {
            Storage = storage;
        }

        public Client Client { get; set; }
        public Storage Storage { get; set; }

        public async Task<bool> Start()
        {
            BeginClient:
            try
            {
                if (Client is null && !AskCredentials())
                {
                    if (wrongCredentials) CustomConsole.PrintError("\nWrong guid or pin");
                    goto BeginClient;
                }

                PrintWelcomeMessage();
                var key = Console.ReadLine();
                switch (key.ToLower())
                {
                    case "1": //  view GUID and credentials
                        CustomConsole.PrintStyleInfo(Client.ToString());
                        goto BeginClient;
                    case "2": // total amount in preferred currency
                        if (!ViewTotalAmount()) // view total amount
                            throw new Exception("\n An error occured");

                        goto BeginClient;
                    case "3": // total all amounts
                        if (!ViewTotatAllAmounts()) // View total all amounts
                            throw new Exception("\n An error occured");

                        goto BeginClient;
                    case "4": // retrieve money from currency
                        RetrieveMoney:
                        if (!RetrieveMoney())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto RetrieveMoney;
                        }

                        CustomConsole.PrintSuccess("Operation submitted ! ");
                        goto BeginClient;
                    case "5": // add money from currency
                        AddMoney:
                        if (!AddMoney())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto AddMoney;
                        }

                        CustomConsole.PrintSuccess("Operation submitted ! ");
                        goto BeginClient;
                    case "6": // exchange money
                        ExchangeMoney:
                        if (!await ExchangeMoney())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto ExchangeMoney;
                        }

                        goto BeginClient;
                    case "7": // transfert money
                        TransfertMoney:
                        if (!await TransfertMoney())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto TransfertMoney;
                        }

                        goto BeginClient;
                    case "8": // change pin
                        ChangePin:
                        if (!ChangePin())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto ChangePin;
                        }

                        ShowPin();

                        goto BeginClient;
                    case "9": // Leave message for admin
                        LeaveMsg:
                        if (!LeaveMsg())
                        {
                            CustomConsole.PrintError("\n An eroor occured");
                            goto LeaveMsg;
                        }

                        CustomConsole.PrintSuccess("Message sended ! ");
                        goto BeginClient;
                    case "10": // Show Pin
                        if (!ShowPin()) CustomConsole.PrintError("\n An eroor occured");

                        goto BeginClient;
                    case "d": // Disconnect
                        Client = null;
                        CustomConsole.PrintSuccess("Vous avez bien ??t?? d??connect?? \n");
                        return false;
                    case "q": // Quit
                        return true;
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
            Console.Write("Please enter your guid : ");
            var stringGuid = Console.ReadLine();

            Console.Write("Please enter your pin : ");
            var stringPin = CustomConsole.EnterPassword();

            int guid;
            int pin;
            if (!int.TryParse(stringGuid, out guid) || !int.TryParse(stringPin, out pin)) return false;
            var client = Storage.DataAccess.GetClient(guid);

            if (client is null) return false;
            if (client.Pin != pin)
            {
                client.Tries++;
                Storage.DataAccess.UpdateClient(client);
                wrongCredentials = true;
                return false;
            }

            if (client.Blocked)
            {
                CustomConsole.PrintError(
                    "\nSorry, you are blocked , if you want to unblocked, please contact an admin ");
                wrongCredentials = false;
                return false;
            }

            Client = client;
            Console.WriteLine("");
            wrongCredentials = false;
            return true;
        }

        public void PrintWelcomeMessage()
        {
            Console.WriteLine("\n");
            CustomConsole.PrintStyleInfo("Welcome Client");
            CustomConsole.PrintInfo("Tap : ");
            CustomConsole.PrintAllChoices(new List<Choice>
            {
                new() {Key = "1", Message = "to view GUID and credentials"},
                new() {Key = "2", Message = "to view total amount in preferred currency"},
                new() {Key = "3", Message = "to view total of all amounts"},
                new() {Key = "4", Message = "to retrieve money from currency"},
                new() {Key = "5", Message = "to add money to currency"},
                new() {Key = "6", Message = "to exchange between currencies"},
                new() {Key = "7", Message = "to transfer money to an another client"},
                new() {Key = "8", Message = "to change pin"},
                new() {Key = "9", Message = "to leave message for admin"},
                new() {Key = "10", Message = "to show your pin (only 5 seconds)"},
                new() {Key = "D", Message = "to disconnect"},
                new() {Key = "Q", Message = "to quit"}
            });
        }

        public bool ViewTotalAmount()
        {
            var currencyClient = Storage.DataAccess.GetMainCurrencyClient(Client.Guid);
            if (currencyClient == null)
            {
                CustomConsole.PrintStyleInfo("No currency");
                return true;
            }

            CustomConsole.PrintStyleInfo(currencyClient.Amount + " " + currencyClient.Currency.Name);
            return true;
        }

        public bool ViewTotatAllAmounts()
        {
            var currenciesClient = Storage.DataAccess.GetCurrenciesClient(Client.Guid);
            if (currenciesClient.Count == 0)
            {
                CustomConsole.PrintStyleInfo("No currency");
                return true;
            }


            var msg = "You have : \n";
            foreach (var currencyClient in currenciesClient)
                msg += "     -- " + currencyClient.Amount + " " + currencyClient.Currency.Name + "\n";

            CustomConsole.PrintStyleInfo(msg);
            return true;
        }

        public bool RetrieveMoney()
        {
            try
            {
                var currenciesClient = Storage.DataAccess.GetCurrenciesClient(Client.Guid);
                if (currenciesClient.Count == 0)
                {
                    CustomConsole.PrintStyleInfo("No currency");
                    return true;
                }

                var currencyClient = ChooseCurrency(currenciesClient);

                currencyClient.Amount -= ChooseAmount();
                return Storage.DataAccess.UpdateCurrencyClient(currencyClient);
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
                var currenciesClient = Storage.DataAccess.GetCurrenciesClient(Client.Guid);
                if (currenciesClient.Count == 0)
                {
                    CustomConsole.PrintStyleInfo("No currency");
                    return true;
                }

                var currency = ChooseCurrency(currenciesClient);


                Console.WriteLine("old :" + currency);
                currency.Amount += ChooseAmount();

                Console.WriteLine("new :" + currency);
                Storage.DataAccess.UpdateCurrencyClient(currency);
                return true;
            }
            catch (Exception e)
            {
                CustomConsole.PrintError(e.Message);
                return false;
            }
        }

        private CurrencyClient ChooseCurrency(List<CurrencyClient> currenciesClient)
        {
            ChooseCurrency:
            CustomConsole.Print("Choose the currency : ");
            CustomConsole.PrintAllChoices(Choice.CreateChoices(currenciesClient));
            var stringCurrency = Console.ReadLine();
            int currencyIndex;
            if (!int.TryParse(stringCurrency, out currencyIndex) ||
                currenciesClient.Find(c => c.CurrencyClientId == currencyIndex) == null)
            {
                Console.WriteLine("Wrong key ! ");
                goto ChooseCurrency;
            }

            var currency = currenciesClient.Find(c => c.CurrencyClientId == currencyIndex);

            CustomConsole.Print("");
            return currency;
        }


        private double ChooseAmount()
        {
            TapAmount:
            Console.Write("Tap the amount : ");
            var stringAmount = Console.ReadLine();
            double amount;
            if (!double.TryParse(stringAmount, out amount) || amount < 1)
            {
                CustomConsole.PrintError("Wrong amount ! ");
                goto TapAmount;
            }

            return amount;
        }

        public async Task<bool> ExchangeMoney()
        {
            try
            {
                ChooseCurrency:
                var currenciesChoices = Choice.CreateChoices(Client.CurrencyClients);
                if (Client.CurrencyClients.Count == 0)
                {
                    CustomConsole.PrintStyleInfo("No currency");
                    return true;
                }

                CustomConsole.Print("Select base currency : ");
                CustomConsole.PrintAllChoices(currenciesChoices);
                var currencyCount = Client.CurrencyClients.Count;
                var stringBaseCurrency = Console.ReadLine();
                int baseIndex;
                if (!int.TryParse(stringBaseCurrency, out baseIndex) || baseIndex < 1 ||
                    baseIndex > currencyCount)
                {
                    CustomConsole.PrintError("Wrong key ! ");
                    goto ChooseCurrency;
                }

                var baseCurrency = Client.CurrencyClients.ElementAt(baseIndex - 1);


                CustomConsole.Print("Select base currency : ");
                CustomConsole.PrintAllChoices(currenciesChoices);
                var stringTargetCurrency = Console.ReadLine();
                int targetIndex;
                if (!int.TryParse(stringTargetCurrency, out targetIndex) || targetIndex < 1 ||
                    targetIndex > currencyCount)
                {
                    CustomConsole.PrintError("Wrong key ! ");
                    goto ChooseCurrency;
                }

                var targetCurrency = Client.CurrencyClients.ElementAt(targetIndex - 1);
                SelectAmount:
                CustomConsole.PrintInfo("How much " + baseCurrency.Currency.Name + " do you want to exchange in " +
                                        targetCurrency.Currency.Name + " ? ");
                var strAmount = Console.ReadLine();
                int amount;
                if (!int.TryParse(strAmount, out amount))
                {
                    CustomConsole.PrintError("Wrong amount ! ");
                    goto SelectAmount;
                }

                return await Storage.DataAccess.ExchangeCurrency(baseCurrency, targetCurrency, amount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                CustomConsole.PrintError(e.Message);
                return false;
            }
        }

        public async Task<bool> TransfertMoney()
        {
            ChooseClient:
            if (Client.CurrencyClients.Count == 0)
            {
                CustomConsole.PrintStyleInfo("No currency");
                return true;
            }

            CustomConsole.PrintInfo("Please choose client to transfert money : ");
            var clients = Storage.DataAccess.GetAll();
            clients.Remove(Client);
            CustomConsole.PrintAllChoices(Choice.CreateChoices(clients));
            var stringClient = Console.ReadLine();
            int clientGuid;
            if (!int.TryParse(stringClient, out clientGuid) || !clients.Exists(c => c.Guid == clientGuid))
            {
                CustomConsole.PrintError("Wrong client ! ");
                goto ChooseClient;
            }

            var amount = ChooseAmount();

            return await Storage.DataAccess.TransfertMoney(Client, clients.Find(c => c.Guid == clientGuid), amount);
        }

        public bool ChangePin()
        {
            Client.Pin = new Random().Next(1000, 10000);
            return Storage.DataAccess.UpdateClient(Client);
        }

        public bool LeaveMsg()
        {
            try
            {
                Console.Write("Your message : ");
                var msg = Console.ReadLine();
                return msg.Trim() == "" || Storage.DataAccess.AddMessage(new Message {Content = msg, Client = Client});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public bool ShowPin()
        {
            try
            {
                var secondsToWait = 5;

                var currentLineCursor = Console.CursorTop;
                CustomConsole.PrintStyleInfo("Your pin : " + Client.Pin);
                while (secondsToWait != 0)
                {
                    CustomConsole.PrintInfo("" + secondsToWait + " . ", false);
                    Thread.Sleep(1000);
                    secondsToWait--;
                }

                for (var i = -1; i < 3; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - i);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, currentLineCursor - 1);
                }

                return true;
            }
            catch (Exception e)
            {
                CustomConsole.PrintError(e.Message);
                return false;
            }
        }
    }
}