using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    public class Currency
    {
        public Currency()
        {
            CurrencyClients = new HashSet<CurrencyClient>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        [JsonIgnore] public ICollection<CurrencyClient> CurrencyClients { get; set; }

        public override string ToString()
        {
            var count = CurrencyClients == null ? -1 : CurrencyClients.Count;
            return "[" + Id + "] --> " + Name + " => {" + count + "}";
        }
    }
}