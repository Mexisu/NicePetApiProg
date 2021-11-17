using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class HistoricalOwner
    {
        [Key]
        public int Id { get; set; }
        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }
        //public Date? Date { get; set; }
        public string OwnerName { get; set; }
        public int? UserId { get; set; }
        public User User {get; set;}
        public int? AcquisitionTypeId { get; set; }
    }
}
