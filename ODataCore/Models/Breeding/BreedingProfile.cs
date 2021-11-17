using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class BreedingProfile
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public int? StarsLevel { get; set; }
        public string BreedingName { get; set; }
    }
}
