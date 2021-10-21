using System.Collections.Generic;
using Bank.Models;

namespace Bank.Context
{
    public class ClientJsonContext
    {
        public List<Client> Clients { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<CurrencyClient> CurrenciesClient { get; set; }

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
            foreach (var currencyClient in CurrenciesClient)
            {
                res += currencyClient + "\r\n";
            }

            return res;
        }
    }
}