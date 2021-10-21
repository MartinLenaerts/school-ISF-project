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

        public List<CurrencyClient> CurrencyClients { get; set; }

        public override string ToString()
        {
            int count = this.CurrencyClients == null ? -1 : this.CurrencyClients.Count;
            return "Client n°" + this.Guid + " \r\n       Prenom : " + this.Firstname + " \r\n       Nom : " + this.Lastname + " \r\n       Nombre de devises : " + count + "\r\n";
        }

        public void merge(Client c)
        {
            Firstname = c.Firstname == null ? Firstname : c.Firstname;
            Lastname = c.Lastname == null ? Lastname : c.Lastname;
            Pin = c.Pin == 0 ? Pin : c.Pin;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Client)) return false;
            return Guid == ((Client)obj).Guid;
        }
    }
}