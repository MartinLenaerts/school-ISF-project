using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    [Index(nameof(CurrencyId), nameof(ClientId), IsUnique = true)]
    public class CurrencyClient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CurrencyClientId { get; set; }

        [ForeignKey("Currency")] [Required] public int CurrencyId { get; set; }

        [JsonIgnore] public Currency Currency { get; set; }

        [ForeignKey("Client")] [Required] public int ClientId { get; set; }

        [JsonIgnore] public Client Client { get; set; }

        public bool HasMain { get; set; }

        public double Amount { get; set; }


        public string ToStringWithClient()
        {
            return "Client n°" + ClientId + " have " + Amount + " " + Currency.Name;
        }
        
        public override string ToString()
        {
            return  Amount + " " + Currency.Name;
        }

        public void Merge(CurrencyClient c)
        {
            HasMain = c.HasMain;
            Amount = c.Amount == -1 ? Amount : c.Amount;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is CurrencyClient)) return false;
            var cc = (CurrencyClient) obj;
            return ClientId == cc.ClientId && ClientId == cc.ClientId;
        }
    }
}