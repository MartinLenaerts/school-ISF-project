using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Transaction
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionId { get; set; }
        

        [System.Text.Json.Serialization.JsonIgnore]
        public Client Sender { get; set; }
        [ForeignKey("Sender"), Required]
        public int SenderId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Client Receiver { get; set; }
        [ForeignKey("Receiver"), Required]
        public int ReceiverId { get; set; }

        public double Amount { get; set; }


        public override string ToString()
        {
            return "Client n°" + this.Sender.Guid
                               + "(" + this.Sender.Firstname + " " + this.Sender.Lastname + ") " +
                               "have send " + this.Amount + " to " +
                               "Client n°" + this.Receiver.Guid +
                               "(" + this.Receiver.Firstname + " " + this.Receiver.Lastname + ") ";
        }
    }
}