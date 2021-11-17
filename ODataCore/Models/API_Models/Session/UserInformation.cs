using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.Edm;

namespace Nicepet_API.Models
{
    public class UserInformation
    {
        [Required]
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string UserAvatar { get; set; }
        public bool?   IsValidEmail { get; set; }
        public string Error { get; set; }
        public string Success { get; set; }
        public string AccessToken { get; set; }
        public ICollection<UserAddressInformation> userAddresses { get; set; }
        public ICollection<UserProfileInformation> userProfile { get; set; }
        public ICollection<BreedingProfileInformation> breedingProfile { get; set; }
    }

    public class UserAddressInformation
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string ZipCode { get; set; }
    }

    public class UserProfileInformation
    {
        public int? UserProfileId { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string Gallery { get; set; }
        public string About { get; set; }
    }
    public class BreedingProfileInformation
    {
        public DateTimeOffset? StartTime { get; set; }
        public int? StarsLevel { get; set; }
        public string BreedingName { get; set; }
    }
}
