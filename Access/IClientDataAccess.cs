using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Models;

namespace Bank.Access
{
    public interface IClientDataAccess
    {
        public List<Client> GetAll();
        public bool CreateClient(Client c);
        public Client GetClient(int guid);
        public bool UpdateClient(Client c);
        public bool DeleteClient(int guid);
        public List<Transaction> GetAllTransactions();
        public List<Transaction> GetClientTransactions(int guid);
        public bool AddTransaction(Transaction transaction);

        public bool AddCurrencyClient(CurrencyClient currencyClient);

        public bool DeleteCurrencyClient(CurrencyClient currencyClient);

        public bool UpdateCurrencyClient(CurrencyClient currencyClient);

        public Task<bool> ExchangeCurrency(CurrencyClient sender, CurrencyClient receiver, double amount);
        public Task<bool> TransfertMoney(Client sender, Client receiver, double amount);

        public CurrencyClient GetMainCurrencyClient(int guid);
        public List<CurrencyClient> GetCurrenciesClient(int guid);

        public int getLastId();
    }
}