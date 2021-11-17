using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Nicepet_API.Models
{
    public class AnimalBreed
    {
        [Key]
        public int Id { get; set; }
        public int? AnimalSpeciesId { get; set; }
        public AnimalSpecies AnimalSpecies { get; set; }
        public string Name { get; set; }
        public int? AnimalHairTypeId { get; set; }
        public AnimalHairType AnimalHairType { get; set; }
    }
}
