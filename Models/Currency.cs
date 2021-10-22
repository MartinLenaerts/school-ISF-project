using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Currency
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public List<CurrencyClient> CurrencyClients { get; set; }
        
        public override string ToString()
        {
            int count = this.CurrencyClients == null ? -1 : this.CurrencyClients.Count;
            return "[" + this.Id + "] --> " + this.Name + " => {" + count + "}";
        }
    }
}