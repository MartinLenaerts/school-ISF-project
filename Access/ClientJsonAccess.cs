using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Bank.Context;
using Bank.Models;

namespace Bank.Access
{
    public class ClientJsonAccess : IClientDataAccess
    {
        private static string dbJsonFile = "database.json";

        public static string DbJsonFile
        {
            get => dbJsonFile;
        }

        public ClientJsonContext GetContext()
        {
            string dbJson = new StreamReader(dbJsonFile).ReadToEnd();
            ClientJsonContext json = JsonSerializer.Deserialize<ClientJsonContext>(dbJson);
            return json;
        }

        private bool PushContext(ClientJsonContext context)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize<ClientJsonContext>(context);
                File.WriteAllText(dbJsonFile, jsonString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public List<Client> GetAll()
        {
            ClientJsonContext context = GetContext();
            return context != null ? context.Clients : new List<Client>();
        }

        public bool CreateClient(Client c)
        {
            try
            {
                ClientJsonContext context = GetContext();
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
            ClientJsonContext context = GetContext();
            Client c = context.Clients.Find(c => c.Guid == guid);
            return c;
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                ClientJsonContext context = GetContext();
                Client client = context.Clients.Find(client => client.Guid == c.Guid);
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
                ClientJsonContext context = GetContext();
                context.Clients.Remove(new Client() {Guid = guid});
                return PushContext(context);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            ClientJsonContext context = GetContext();
            return context != null ? context.Transactions : new List<Transaction>();
        }

        public List<Transaction> GetClientTransactions(int guid)
        {
            ClientJsonContext context = GetContext();
            List<Transaction> t = context.Transactions.FindAll(t => t.ReceiverId == guid || t.SenderId == guid);
            return t;
        }

        public bool AddTransaction(Transaction transaction)
        {
            try
            {
                ClientJsonContext context = GetContext();
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
                ClientJsonContext context = GetContext();
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
                ClientJsonContext context = GetContext();
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
                ClientJsonContext context = GetContext();
                CurrencyClient cc = context.CurrenciesClients.Find(c => c == currencyClient);
                cc.Merge(currencyClient);
                return PushContext(context);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ExchangeCurrency(CurrencyClient sender, CurrencyClient receiver, double amount)
        {
            try
            {
                ClientJsonContext context = GetContext();
                CurrencyClient ccSender = context.CurrenciesClients.Find(c => c == sender);
                CurrencyClient ccReceiver = context.CurrenciesClients.Find(c => c == receiver);
                if (ccSender.Amount < amount) return false;
                ccSender.Amount -= amount;
                ccSender.Amount += amount;
                return PushContext(context);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CurrencyClient GetMainCurrencyClient(int guid)
        {
            ClientJsonContext context = GetContext();
            CurrencyClient currencyClient = context.CurrenciesClients.Find(cc => cc.ClientId==guid && cc.HasMain);
            return currencyClient;
        }

        public List<CurrencyClient> GetCurrenciesClient(int guid)
        {
            ClientJsonContext context = GetContext();
            List<CurrencyClient> currenciesClient = context.CurrenciesClients.FindAll(cc => cc.ClientId==guid);
            return currenciesClient;
        }
    }
}