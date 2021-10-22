using System.Collections.Generic;
using Bank.Models;

namespace Bank.Context
{
    public class ClientJsonContext
    {
        public List<Client> Clients { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<CurrencyClient> CurrenciesClients { get; set; }
        public List<Transaction> Transactions { get; set; }

        public override string ToString()
        {
            string res = "";
            foreach (var client in Clients)
            {
                res += client + "\r\n";
            }

            res += "\r\n";
            foreach (var currency in Currencies)
            {
                res += currency + "\r\n";
            }

            res += "\r\n";
            foreach (var currencyClient in CurrenciesClients)
            {
                res += currencyClient + "\r\n";
            }
            
            res += "\r\n";
            foreach (var transaction in Transactions)
            {
                res += transaction + "\r\n";
            }

            return res;
        }
    }
}