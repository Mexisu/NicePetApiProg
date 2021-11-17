using System;
using System.ComponentModel.DataAnnotations;

namespace Nicepet_API.Models
{
    public class Contacts
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int ContactId { get; set; }
        public User UserContact { get; set; }
    }
}
