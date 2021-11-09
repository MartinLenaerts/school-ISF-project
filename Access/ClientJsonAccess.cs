using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Bank.Context;
using Bank.Models;
using Bank.Utils;

namespace Bank.Access
{
    public class ClientJsonAccess : IClientDataAccess
    {
        public static readonly ClientApiAccess ApiAccess = new();

        public ClientJsonAccess()
        {
            if (!File.Exists(DbJsonFile)) File.Create(DbJsonFile);
        }

        public static string DbJsonFile { get; } = "database.json";


        public List<Client> GetAll()
        {
            try
            {
                var context = GetContext();
                if (context != null) return AddCurrenciesToClients(context.Clients, context);
                return new List<Client>();
            }
            catch (Exception e)
            {
                CustomConsole.PrintError(e.Message);
                throw;
            }
        }


        public bool CreateClient(Client c)
        {
            try
            {
                var context = GetContext();
                context.Clients.Add(c);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Client GetClient(int guid)
        {
            var context = GetContext();
            var c = AddCurrenciesToClient(context.Clients.Find(c => c.Guid == guid), context);
            return c;
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                var context = GetContext();
                var client = context.Clients.Find(client => client.Guid == c.Guid);
                client.Merge(c);
                return PushContext(context);
                ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteClient(int guid)
        {
            try
            {
                var context = GetContext();
                context.Clients.Remove(new Client {Guid = guid});
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            try
            {
                var context = GetContext();
                if (context != null) return AddClientToTransactions(context.Transactions, context);
                return new List<Transaction>();
            }
            catch (Exception e)
            {
                CustomConsole.PrintError(e.Message);
                throw;
            }
        }

        public List<Transaction> GetClientTransactions(int guid)
        {
            var context = GetContext();
            return AddClientToTransactions(
                context.Transactions.FindAll(t => t.Sender.Guid == guid || t.Receiver.Guid == guid), context);
        }

        public bool AddTransaction(Transaction transaction)
        {
            try
            {
                var context = GetContext();
                context.Transactions.Add(transaction);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddCurrencyClient(CurrencyClient currencyClient)
        {
            try
            {
                var context = GetContext();
                context.CurrenciesClients.Add(currencyClient);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool DeleteCurrencyClient(CurrencyClient currencyClient)
        {
            try
            {
                var context = GetContext();
                context.CurrenciesClients.Remove(currencyClient);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool UpdateCurrencyClient(CurrencyClient currencyClient)
        {
            try
            {
                var context = GetContext();
                var cc = context.CurrenciesClients.Find(c => c == currencyClient);
                cc.Merge(currencyClient);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> ExchangeCurrency(CurrencyClient sender, CurrencyClient receiver, double amount)
        {
            try
            {
                var rate = await ApiAccess.GetPair(sender.Currency.Name, receiver.Currency.Name);
                var context = GetContext();
                if (sender.Amount < amount) return false;
                sender.Amount -= amount;
                receiver.Amount += amount * amount;
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> TransfertMoney(Client sender, Client receiver, double amount)
        {
            try
            {
                var context = GetContext();
                var ccSender = context.CurrenciesClients.Find(c => c == sender.CurrencyClients && c.HasMain);
                var ccReceiver = context.CurrenciesClients.Find(c => c == receiver.CurrencyClients && c.HasMain);
                double rate = 1;
                if (!ccSender.Currency.Equals(ccReceiver.Currency))
                    rate = await ApiAccess.GetPair(ccSender.Currency.Name, ccReceiver.Currency.Name);
                if (ccSender.Amount < amount) return false;
                ccSender.Amount -= amount;
                ccReceiver.Amount += amount * rate;
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public CurrencyClient GetMainCurrencyClient(int guid)
        {
            var context = GetContext();
            var currencyClient =
                AddCurrencyToCurrenciesClient(context.CurrenciesClients.Find(cc => cc.ClientId == guid && cc.HasMain),
                    context);
            return currencyClient;
        }

        public List<CurrencyClient> GetCurrenciesClient(int guid)
        {
            var context = GetContext();
            var currenciesClient =
                AddCurrencyToCurrenciesClients(context.CurrenciesClients.FindAll(cc => cc.ClientId == guid), context);
            return currenciesClient;
        }

        public List<Currency> GetAllCurrencies()
        {
            var context = GetContext();
            var currenciesClient = context.Currencies;
            return currenciesClient;
        }


        public int getLastId()
        {
            return GetContext().Clients.Last().Guid;
        }

        public List<Message> getMessages()
        {
            var context = GetContext();
            return context.Messages.OrderBy(c => c.Date).ToList();
        }

        public bool AddMessage(Message m)
        {
            try
            {
                var context = GetContext();
                context.Messages.Add(m);
                return PushContext(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public ClientJsonContext GetContext()
        {
            var dbJson = new StreamReader(DbJsonFile).ReadToEnd();
            var json = JsonSerializer.Deserialize<ClientJsonContext>(dbJson);
            return json;
        }

        private bool PushContext(ClientJsonContext context)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(context);
                File.WriteAllText(DbJsonFile, jsonString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private List<Client> AddCurrenciesToClients(List<Client> clients, ClientJsonContext context)
        {
            var result = clients;
            for (var i = 0; i < clients.Count; i++) result[i] = AddCurrenciesToClient(result[i], context);

            return result;
        }


        private Client AddCurrenciesToClient(Client client, ClientJsonContext context)
        {
            var result = client;
            foreach (var currencyClient in context.CurrenciesClients)
                if (result.Guid == currencyClient.ClientId)
                {
                    if (result.CurrencyClients == null) result.CurrencyClients = new List<CurrencyClient>();
                    result.CurrencyClients.Add(AddCurrencyToCurrenciesClient(currencyClient, context));
                }

            return result;
        }

        private CurrencyClient AddCurrencyToCurrenciesClient(CurrencyClient currencyClient, ClientJsonContext context)
        {
            var result = currencyClient;
            foreach (var currency in context.Currencies)
                if (result.CurrencyId == currency.Id)
                    result.Currency = currency;

            return result;
        }

        private List<CurrencyClient> AddCurrencyToCurrenciesClients(List<CurrencyClient> currencyClients,
            ClientJsonContext context)
        {
            var result = currencyClients;
            for (var i = 0; i < currencyClients.Count; i++)
                result[i] = AddCurrencyToCurrenciesClient(result[i], context);

            return result;
        }

        private Transaction AddClientToTransaction(Transaction transaction, ClientJsonContext context)
        {
            var result = transaction;
            foreach (var client in context.Clients)
            {
                if (result.ReceiverId == client.Guid) result.Receiver = client;
                if (result.SenderId == client.Guid) result.Sender = client;
            }

            return result;
        }

        private List<Transaction> AddClientToTransactions(List<Transaction> transactions, ClientJsonContext context)
        {
            var result = transactions;
            for (var i = 0; i < transactions.Count; i++) result[i] = AddClientToTransaction(result[i], context);

            return result;
        }
    }
}