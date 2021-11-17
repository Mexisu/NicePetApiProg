using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nicepet_API.Helpers
{
    [ApiController]
    public class ReturnUserInformationController : ControllerBase
    {
        public static async Task<UserInformation> ReturnUserInformation(IJWTAuthenticationManager jWTAuthenticationManager,ApiNicepetContext _db, List<User> userLogingList)
        {
            UserInformation userInformation = new UserInformation();
        
            if (userLogingList.Count == 0)
            {
                userInformation = new UserInformation
                {
                    Error = "Ce compte n'existe pas !   "
                };
            }
            else if (userLogingList.Count > 0)
            {
                //----------------------------User Address----------------------------------------
                List<UserAddressInformation> userAddresses = new List<UserAddressInformation>();
                List<UserAddress> userAddress = await _db.UserAddress
                .Where(a => a.UserId == userLogingList[0].Id)
                .ToListAsync();
                foreach (UserAddress userAddressGetted in userAddress)
                {
                    userAddresses.Add(new UserAddressInformation
                    {
                        Street = userAddressGetted.Street,
                        City = userAddressGetted.City,
                        Region = userAddressGetted.Region,
                        Department = userAddressGetted.Department,
                        ZipCode = userAddressGetted.ZipCode
                    });
                }
                //-----------------------------User Profile----------------------------------------
                List<UserProfileInformation> userProfiles = new List<UserProfileInformation>();
                List<UserProfile> userProfile = await _db.UserProfile
                .Where(a => a.UserId == userLogingList[0].Id)
                .ToListAsync();
                foreach (UserProfile userProfileGetted in userProfile)
                {
                    userProfiles.Add(new UserProfileInformation
                    {
                        UserProfileId = userProfileGetted.Id,
                        Title = userProfileGetted.Title,
                        Gallery = userProfileGetted.Gallery,
                        About = userProfileGetted.About
                    });
                }
                //-----------------------------Breeding Profile----------------------------------------
                List<BreedingProfileInformation> breedingProfiles = new List<BreedingProfileInformation>();
                List<BreedingProfile> breedingProfile = await _db.BreedingProfile
                .Where(a => a.UserId == userLogingList[0].Id)
                .ToListAsync();
                foreach (BreedingProfile breedingProfileGetted in breedingProfile)
                {
                    breedingProfiles.Add(new BreedingProfileInformation
                    {
                        StartTime = breedingProfileGetted.StartTime,
                        StarsLevel = breedingProfileGetted.StarsLevel,
                        BreedingName = breedingProfileGetted.BreedingName
                    });
                }
                //ErrorMessage = "Good";
                string name = "";
                if (userLogingList[0].Email != null) name = userLogingList[0].Email;
                else if (userLogingList[0].FacebookAccount != null) name = userLogingList[0].FacebookAccount;
                else if (userLogingList[0].GoogleAccount != null) name = userLogingList[0].GoogleAccount;
                if(name != "")
                {
                  var token = jWTAuthenticationManager.Authenticate(name, userLogingList[0].UserType.Type);
                   
                    userInformation = new UserInformation
                    {
                        UserId = (int)userLogingList.First().Id,
                        FirstName = userLogingList.First().FirstName,
                        LastName = userLogingList.First().LastName,
                        Phone = userLogingList.First().PhoneNumber,
                        IsValidEmail = userLogingList.First().IsValidEmail,
                        UserAvatar= userLogingList.First().Avatar,
                        AccessToken = token.JwtToken,
                        userAddresses = userAddresses,
                        userProfile = userProfiles,
                        //breedingProfile = breedingProfiles
                    };
                }
            

            }
            return userInformation;
        }
    }
}
