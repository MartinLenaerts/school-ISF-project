using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionId { get; set; }


        [JsonIgnore] public Client Sender { get; set; }

        [ForeignKey("Sender")] [Required] public int SenderId { get; set; }

        [JsonIgnore] public Client Receiver { get; set; }

        [ForeignKey("Receiver")] [Required] public int ReceiverId { get; set; }

        public double Amount { get; set; }


        public override string ToString()
        {
            return "Client n°" + Sender.Guid
                               + "(" + Sender.Firstname + " " + Sender.Lastname + ") " +
                               "have send " + Amount + " to " +
                               "Client n°" + Receiver.Guid +
                               "(" + Receiver.Firstname + " " + Receiver.Lastname + ") ";
        }
    }
}