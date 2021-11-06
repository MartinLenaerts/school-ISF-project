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
            var res = "";
            foreach (var client in Clients) res += client + "\n";

            res += "\n";
            foreach (var currency in Currencies) res += currency + "\n";

            res += "\n";
            foreach (var currencyClient in CurrenciesClients) res += currencyClient + "\n";

            res += "\n";
            foreach (var transaction in Transactions) res += transaction + "\n";

            return res;
        }
    }
}