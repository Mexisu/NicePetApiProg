using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int? AnimalProfileId {get; set;}
        public AnimalProfile AnimalProfile { get; set; }
        public int? AnimalBreedId { get; set; }
        public AnimalBreed AnimalBreed { get; set; }
        public string Name { get; set; }
        public bool? Sexe { get; set; }
        //public Date? BirthDate { get; set; }
        public int? FatherId { get; set; }
        public Animal Father { get; set; }
        public int? MotherId { get; set; }
        public Animal Mother { get; set; }
        public int? MatriculationNumber { get; set; }
        public int? PedigreeNumber { get; set; }
        public bool? IsPedigree { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public bool? IsHandicapped { get; set; }
    }
}
