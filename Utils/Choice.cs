using System.Collections.Generic;
using Bank.Models;

namespace Bank.Utils
{
    public class Choice
    {
        public string Key { get; set; }
        public string Message { get; set; }


        public static List<Choice> CreateChoices(ICollection<CurrencyClient> currencyClients)
        {
            var choices = new List<Choice>();

            var count = 1;
            foreach (var currencyClient in currencyClients)
            {
                choices.Add(new Choice {Key = "" + currencyClient.CurrencyClientId, Message = currencyClient.Currency.Name});
                count++;
            }

            return choices;
        }
        
        public static List<Choice> CreateChoices(ICollection<Currency> currencies)
        {
            var choices = new List<Choice>();

            var count = 1;
            foreach (var currency in currencies)
            {
                choices.Add(new Choice {Key = "" + currency.Id, Message = currency.Name});
                count++;
            }

            return choices;
        }
        

        public static List<Choice> CreateChoices(ICollection<Client> clients)
        {
            var choices = new List<Choice>();

            var count = 1;
            foreach (var client in clients)
            {
                choices.Add(new Choice {Key = "" + client.Guid, Message = client.Firstname + " " + client.Lastname});
                count++;
            }

            return choices;
        }
    }
}