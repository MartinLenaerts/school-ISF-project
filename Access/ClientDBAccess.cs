using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Context;
using Bank.Models;

namespace Bank.Access
{
    public class ClientDbAccess : IClientDataAccess
    {
        public static readonly ClientDbContext Context = new ClientDbContext();

        public List<Client> GetAll()
        {
            return Context.Clients.ToList();
        }

        public bool CreateClient(Client c)
        {
            try
            {
                Context.Clients.Add(c);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Client GetClient(int guid)
        {
            return Context.Clients.FirstOrDefault(c => c.Guid == guid);
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                Client client = Context.Clients.FirstOrDefault(cl => cl.Guid == c.Guid);
                client.Merge(c);
                Context.SaveChanges();
                return true;
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
                var client = Context.Clients.Find(guid);
                Context.Clients.Remove(client);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            return Context.Transactions.ToList();
        }

        public List<Transaction> GetClientTransactions(int guid)
        {
            return Context.Transactions.Where(t => t.Receiver.Guid == guid || t.Sender.Guid == guid).ToList();
        }

        public bool AddTransaction(Transaction transaction)
        {
            try
            {
                Context.Transactions.Add(transaction);
                Context.SaveChanges();
                return true;
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
                Context.CurrenciesClients.Add(currencyClient);
                Context.SaveChanges();
                return true;
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
                var cc = Context.CurrenciesClients.First(c =>
                    c.ClientId == currencyClient.ClientId && c.CurrencyId == currencyClient.CurrencyId);
                Context.CurrenciesClients.Remove(cc);
                Context.SaveChanges();
                return true;
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
                var cc = Context.CurrenciesClients.First(c =>
                    c.ClientId == currencyClient.ClientId && c.CurrencyId == currencyClient.CurrencyId);
                cc.Merge(currencyClient);
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ExchangeCurrency(CurrencyClient sender, CurrencyClient receiver, double amount)
        {
            try
            {
                CurrencyClient ccSender = Context.CurrenciesClients.First(c =>
                    c.ClientId == sender.ClientId && c.CurrencyId == sender.CurrencyId);
                CurrencyClient ccReceiver = Context.CurrenciesClients.First(c =>
                    c.ClientId == receiver.ClientId && c.CurrencyId == receiver.CurrencyId);
                if (ccSender.Amount < amount) return false;
                ccSender.Amount -= amount;
                ccReceiver.Amount += amount;
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public CurrencyClient GetMainCurrencyClient(int guid)
        {
            return Context.CurrenciesClients.FirstOrDefault(cc => cc.ClientId == guid && cc.HasMain == true);
        }

        public List<CurrencyClient> GetCurrenciesClient(int guid)
        {
            return Context.CurrenciesClients.Where(cc => cc.ClientId == guid).ToList();
        }
    }
}