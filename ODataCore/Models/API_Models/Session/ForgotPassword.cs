using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{

    public class ForgotPassword
    {
        [Required]
        public int Id { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string Token { get; set; }

        public bool ToSendMail { get; set; }

        public string Error { get; set; }

    }


}
