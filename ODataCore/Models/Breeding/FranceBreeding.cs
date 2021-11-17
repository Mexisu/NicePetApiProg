using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class FranceBreeding
    {
        [Key]
        public int Id { get; set; }
        public int? BreedingProfileId { get; set; }
        public BreedingProfile BreedingProfile { get; set; }
    }
}
