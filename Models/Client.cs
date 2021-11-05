using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    [Table("Client")]
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Guid { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Pin { get; set; }

        private bool _blocked;
        public bool Blocked
        {
            get { return _blocked; }
            set
            {
                if (!value) Tries = 0;
                _blocked = value;
            }
        }
        private int _tries;

        public int Tries
        {
            get { return _tries; }
            set
            {
                _tries = value;
                if (Tries == 3) _blocked = true;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<CurrencyClient> CurrencyClients { get; set; }
        
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Transaction> TransactionsSender { get; set; }
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Transaction> TransactionsReceiver { get; set; }

        
        

        public Client()
        {
            this.CurrencyClients = new HashSet<CurrencyClient>();
        }
        public override string ToString()
        {
            int count = this.CurrencyClients == null ? -1 : this.CurrencyClients.Count;
            return "Client n°" + this.Guid + " \r\n" +
                   "       Firstname : " + Firstname + " \r\n" +
                   "       LastName : " + Lastname + " \r\n" +
                   "       isBlocked : " + Blocked + " \r\n" +
                   "       Nombre de devises : " + count + "\r\n";
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
    }
}