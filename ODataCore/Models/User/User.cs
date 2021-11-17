using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Nicepet_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public int? UserTypeId { get; set; }
        public UserType UserType { get; set; }
      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [IgnoreDataMember]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTimeOffset? CreationDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public bool? IsValidEmail { get; set; }

        public string FacebookAccount { get; set; }

        public string GoogleAccount { get; set; }

        public ICollection<UserAddress> UserAddressMany { get; set; }

    }
}
