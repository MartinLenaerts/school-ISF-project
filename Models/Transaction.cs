namespace Bank.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Client Sender { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Client Receiver { get; set; }

        public double Amount { get; set; }
    }
}