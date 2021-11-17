using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nicepet_API.Models
{
    public class UserAddress
    {

        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string ZipCode { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
