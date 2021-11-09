using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Message
    {
        public Message()
        {
            Date = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }


        public int ClientId { get; set; }
        public Client Client { get; set; }

        public override string ToString()
        {
            return "Message n°" + Id + " \n" +
                   "       Client n° " + Client.Guid + " : " + Client.Firstname + " " + Client.Lastname + " \n" +
                   "       Content : " + Content + " \n" +
                   "       Date : " + Date.Day + "/" + Date.Month + "/" + Date.Year + " \n" +
                   "       Hour : " + Date.Hour + ":" + Date.Minute + ":" + Date.Second + "\n";
        }
    }
}