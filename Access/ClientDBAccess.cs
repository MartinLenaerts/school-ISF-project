using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Context;
using Bank.Models;
using Bank.Utils;
using Microsoft.EntityFrameworkCore;

namespace Bank.Access
{
    public class ClientDbAccess : IClientDataAccess
    {
        public static readonly ClientDbContext Context = new();
        public static readonly ClientApiAccess ApiAccess = new();

        public List<Client> GetAll()
        {
            return Context.Clients.Include(c => c.CurrencyClients).ThenInclude(cc => cc.Currency).ToList();
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
            return Context.Clients.Include(c => c.CurrencyClients).ThenInclude(cc => cc.Currency)
                .FirstOrDefault(c => c.Guid == guid);
        }

        public bool UpdateClient(Client c)
        {
            try
            {
                var client = Context.Clients.FirstOrDefault(cl => cl.Guid == c.Guid);
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
            return Context.Transactions.Include(tr => tr.Receiver).Include(tr => tr.Sender).ToList();
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

        public async Task<bool> ExchangeCurrency(CurrencyClient sender, CurrencyClient receiver, double amount)
        {
            try
            {
                CustomConsole.PrintSuccess("" + sender.Currency.Name + " ==> " + amount + " ==> " +
                                           receiver.Currency.Name);
                var rate = await ApiAccess.GetPair(sender.Currency.Name, receiver.Currency.Name);
                if (sender.Amount < amount) return false;
                sender.Amount -= amount;
                receiver.Amount += amount * rate;
                Context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public async Task<bool> TransfertMoney(Client sender, Client receiver, double amount)
        {
            try
            {

                var ccSender = Context.CurrenciesClients.First(c =>
                    c.ClientId == sender.Guid && c.HasMain);
                var ccReceiver = Context.CurrenciesClients.First(c =>
                    c.ClientId == receiver.Guid && c.HasMain);

                double rate = 1;
                if(!ccSender.Currency.Equals(ccReceiver.Currency)) rate = await ApiAccess.GetPair(ccSender.Currency.Name, ccReceiver.Currency.Name);
                if (ccSender.Amount < amount) return false;
                ccSender.Amount -= amount;
                ccReceiver.Amount += amount * rate;
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


        public int getLastId()
        {
            return Context.Clients.OrderBy(c=>c.Guid).Last().Guid;
        }
    }
}