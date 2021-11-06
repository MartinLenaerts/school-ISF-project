using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Bank.Models
{
    [Table("Client")]
    public class Client
    {
        private bool _blocked;

        private int _tries;

        public Client()
        {
            CurrencyClients = new HashSet<CurrencyClient>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Guid { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Pin { get; set; }

        public bool Blocked { get; set; }

        public int Tries
        {
            get => _tries;
            set
            {
                if (Tries >= 3) Blocked = true;
                _tries = value;
            }
        }

        [JsonIgnore] public virtual ICollection<CurrencyClient> CurrencyClients { get; set; }

        public override string ToString()
        {
            string currencies = "";
            foreach (var currencyClient in CurrencyClients)
            {
                currencies += "                   --"+currencyClient+ " \n";
            }
            
            return "Client n°" + Guid + " \n" +
                   "       Firstname : " + Firstname + " \n" +
                   "       LastName : " + Lastname + " \n" +
                   "       isBlocked : " + Blocked + " \n" +
                   "       Tries : " + Tries + "\n" +
                   "       Currencies : \n" + currencies;
        }

        public void Merge(Client c)
        {
            Firstname = c.Firstname == null ? Firstname : c.Firstname;
            Lastname = c.Lastname == null ? Lastname : c.Lastname;
            Pin = c.Pin == 0 ? Pin : c.Pin;
            Blocked = c.Blocked;
            Tries = c.Tries;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Client)) return false;
            return Guid == ((Client) obj).Guid;
        }

        public void unBlockedAndReset()
        {
            _blocked = false;
            _tries = 0;
        }
    }
}