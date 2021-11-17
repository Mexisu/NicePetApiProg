using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int? AnnouncementTypeId { get; set; }
        public AnnouncementType AnnouncementType { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photos { get; set; }
        public bool? IsAvailable { get; set; }
        //public Date? AvailabilityDate { get; set; }

    }
}
