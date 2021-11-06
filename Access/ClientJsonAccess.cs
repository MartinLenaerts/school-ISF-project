using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Bank.Context;
using Bank.Models;

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
            var context = GetContext();
            return context != null ? context.Clients : new List<Client>();
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
                return false;
            }
        }

        public Client GetClient(int guid)
        {
            var context = GetContext();
            var c = context.Clients.Find(c => c.Guid == guid);
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
                return false;
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            var context = GetContext();
            return context != null ? context.Transactions : new List<Transaction>();
        }

        public List<Transaction> GetClientTransactions(int guid)
        {
            var context = GetContext();
            var t = context.Transactions.FindAll(t => t.Sender.Guid == guid || t.Receiver.Guid == guid);
            return t;
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
                if (ccSender.Amount < amount) return false;
                ccSender.Amount -= amount;
                ccReceiver.Amount += amount;
                return PushContext(context);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CurrencyClient GetMainCurrencyClient(int guid)
        {
            var context = GetContext();
            var currencyClient = context.CurrenciesClients.Find(cc => cc.ClientId == guid && cc.HasMain);
            return currencyClient;
        }

        public List<CurrencyClient> GetCurrenciesClient(int guid)
        {
            var context = GetContext();
            var currenciesClient = context.CurrenciesClients.FindAll(cc => cc.ClientId == guid);
            return currenciesClient;
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
    }
}