using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class AnimalHairType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
