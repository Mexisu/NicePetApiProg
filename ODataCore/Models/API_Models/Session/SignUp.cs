using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class SignUp
    {
        [Required]
        public int UserId { get; set; }

        public string Token { get; set; }

        public bool ToSendMail { get; set; }
    }

}
