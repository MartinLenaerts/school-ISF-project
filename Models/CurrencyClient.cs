using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class CurrencyClient
    {
        public int CurrencyClientId { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public bool HasMain { get; set; }

        public double Amount { get; set; }

        public override string ToString()
        {
            return "[" + Client + "]  --- [" + Currency + "] \r\n" + "[" + CurrencyClientId + "] --> " + ClientId +
                   ", " +
                   CurrencyId + " ==> " + HasMain;
        }
    }
}