using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Nicepet_API.Models
{
    public class UserProfile
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Gallery { get; set; }
        public string About { get; set; }
    }
}
