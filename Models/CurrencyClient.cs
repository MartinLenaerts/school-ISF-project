using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    [Index(nameof(CurrencyId), nameof(ClientId), IsUnique = true)]
    public class CurrencyClient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CurrencyClientId { get; set; }
        [ForeignKey("Currency"), Required]
        public int CurrencyId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Currency Currency { get; set; }

        [ForeignKey("Client"), Required]
        public int ClientId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Client Client { get; set; }

        public bool HasMain { get; set; }

        public double Amount { get; set; }


        public override string ToString()
        {
            return "[" + Client + "]  --- [" + Currency + "] \r\n" + "[" + CurrencyClientId + "] --> " + ClientId +
                   ", " +
                   CurrencyId + " ==> " + HasMain;
        }

        public void Merge(CurrencyClient c)
        {
            HasMain = c.HasMain;
            Amount = c.Amount == -1 ? Amount : c.Amount;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is CurrencyClient)) return false;
            CurrencyClient cc = ((CurrencyClient) obj);
            return ClientId == cc.ClientId && ClientId == cc.ClientId;
        }
    }
}