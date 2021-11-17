using System;
using System.ComponentModel.DataAnnotations;

namespace Nicepet_API.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int RecipientUserId { get; set; }
        public User RecipientUser { get; set; }

        public int SenderUserId { get; set; }
        public User SenderUser { get; set; }

        public DateTimeOffset Time { get; set; }

        [Required]
        public string Body { get; set; }

        public bool? Readed { get; set; }

        

        
    }
}
