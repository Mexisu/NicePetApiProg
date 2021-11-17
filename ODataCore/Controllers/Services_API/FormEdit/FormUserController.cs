using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.AspNet.OData.Query.AllowedQueryOptions;
using System.ComponentModel.DataAnnotations;

namespace Nicepet_API.Controllers.Services_API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FormUserController : ControllerBase
    {
        private ApiNicepetContext _db;

        public FormUserController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromBody] EditData editData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await PatchUser(editData);
                await PatchUserProfile(editData);
                await PatchUserAddress(editData);

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        //---------------------------------------------- Patch User -------------------------------------------
        public async Task<ActionResult> PatchUser(EditData editData)
        {
            Delta<User> deltaUser = new Delta<User>();
            UserController userController = new UserController(_db);

            List<User> userData = new List<User>();
            userData = await _db.User
               .Where(a => a.Id == Convert.ToInt32(editData.Id))
               .ToListAsync();
            foreach (User Item in userData)
            {
                deltaUser.TrySetPropertyValue("FirstName", editData.FirstName);
                deltaUser.TrySetPropertyValue("LastName", editData.LastName);
                deltaUser.TrySetPropertyValue("Email", editData.Email);
                deltaUser.TrySetPropertyValue("PhoneNumber", editData.PhoneNumber);
                await userController.PatchAsync(Convert.ToInt32(Item.Id), deltaUser);
            }

            return Ok(editData.Id);
        }
        //---------------------------------------------- Patch User Profile -------------------------------------------
        public async Task<ActionResult> PatchUserProfile(EditData editData)
        {
            Delta<UserProfile> deltaUserProfile = new Delta<UserProfile>();
            UserProfileController userProfileController = new UserProfileController(_db);

            List<UserProfile> userProfileData = new List<UserProfile>();
            userProfileData = await _db.UserProfile
               .Where(a => a.UserId == Convert.ToInt32(editData.Id))
               .ToListAsync();
            foreach (UserProfile Item in userProfileData)
            {
                deltaUserProfile.TrySetPropertyValue("Avatar", editData.Avatar);
                deltaUserProfile.TrySetPropertyValue("Title", editData.Title);
                deltaUserProfile.TrySetPropertyValue("Gallery", editData.Gallery);
                deltaUserProfile.TrySetPropertyValue("About", editData.About);
                await userProfileController.PatchAsync(Convert.ToInt32(Item.Id), deltaUserProfile);
            }
            return Ok(editData.Id);
        }
        //---------------------------------------------- Patch User Address -------------------------------------------
        public async Task<ActionResult> PatchUserAddress(EditData editData)
        {
            Delta<UserAddress> deltaUserAddress = new Delta<UserAddress>();
            UserAddressController userAddressController = new UserAddressController(_db);

            List<UserAddress> userAddressData = new List<UserAddress>();
            userAddressData = await _db.UserAddress
               .Where(a => a.UserId == Convert.ToInt32(editData.Id))
               .ToListAsync();
            foreach (UserAddress Item in userAddressData)
            {
                deltaUserAddress.TrySetPropertyValue("Street", editData.Street);
                deltaUserAddress.TrySetPropertyValue("City", editData.City);
                deltaUserAddress.TrySetPropertyValue("Region", editData.Region);
                deltaUserAddress.TrySetPropertyValue("Department", editData.Department);
                deltaUserAddress.TrySetPropertyValue("ZipCode", editData.ZipCode);
                await userAddressController.PatchAsync(Convert.ToInt32(Item.Id), deltaUserAddress);
            }
            return Ok(editData.Id);
        }

        public class EditData
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            //---------User Profile-----------
            public string Avatar { get; set; }
            public string Title { get; set; }
            public string Gallery { get; set; }
            public string About { get; set; }

            //---------User Address----------
            public string Street { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string Department { get; set; }
            public string ZipCode { get; set; }
        }
       
    }
}
