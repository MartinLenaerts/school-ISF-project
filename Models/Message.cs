using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }
        
        
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public Message()
        {
            Date = DateTime.Now;
        }
    }
}