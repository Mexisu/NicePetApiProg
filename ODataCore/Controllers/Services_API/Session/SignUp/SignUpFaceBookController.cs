using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Nicepet_API.Helpers;
using System.Text.RegularExpressions;
using Nicepet_API.Helpers.JWT;
using System.Net.Http;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Nicepet_API.Controllers.Services_API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpFaceBookController : ControllerBase
    {
        private ApiNicepetContext _db;
        private string facebookAppToken = "840285756828186|ScC68BIYOA8_QJPbYYAR3y4FiWA";
        private string facebookAppId = "840285756828186";

        public SignUpFaceBookController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] LoginFaceBook item)
        {
            UserInformation userInformation = new UserInformation();

            if (!ModelState.IsValid)
            {
                return userInformation = new UserInformation
                {
                    Error = BadRequest(ModelState).ToString()
                };
            }

            try
            {
                using (var transaction = new System.Transactions
                    .TransactionScope(TransactionScopeAsyncFlowOption.Enabled))

                {
                    using var client = new HttpClient();

                    var tokenInfo = await client
                        .GetAsync("https://graph.facebook.com/debug_token?input_token="
                        + item.accessToken + "&access_token=" + facebookAppToken);

                    dynamic response = tokenInfo.Content.ReadAsStringAsync().Result;
                    var model = JsonConvert.DeserializeObject<object>(response).data;
                    Assert.IsNotNull(tokenInfo);
                    string fbUserId = model.user_id;

                    if (model.app_id == facebookAppId && model.is_valid == true)
                    {
                        List<User> userSignUpList = await _db.User
                       .Where(a => a.FacebookAccount == fbUserId )
                       .ToListAsync();
                        if (userSignUpList.Count > 0)
                        {
                            userInformation.Error = "L'adresse e-mail est déjà utilisée !  ";
                        }
                        else if (userSignUpList.Count == 0)
                        {
                            await CreateAccount(fbUserId);

                            User userSignUp = await _db.User
                                .Where(a => a.FacebookAccount == fbUserId)
                                .FirstAsync();

                            await CreateUserAddress(userSignUp.Id);
                            await CreateUserProfile(userSignUp.Id);

                            userInformation.Success = "Félicitation votre compte à été créé avec succès ! ";
                            
                            //Un email pour activer votre compte vient de vous être envoyé.
                        }
                        
                    }
                    transaction.Complete();

                    return userInformation;

                }
            }
            catch (Exception ex)
            {
                return userInformation = new UserInformation
                {
                    Error = ex.Message.ToString()
                };
            }
        }


        //------------------------User Creation------------------------
        public async Task<IActionResult> CreateAccount(string fb_user_id)
        {
            UserController userController = new UserController(_db);

            User user = new User
            {
                FacebookAccount = fb_user_id,
                UserTypeId=2
            };
               await userController.PostAsync(user);

            return Ok();
        }

        //------------------------User Address Creation------------------------
        public async Task<IActionResult> CreateUserAddress(int userId)
        {
            UserAddressController userAddressController = new UserAddressController(_db);
            UserAddress userAddress = new UserAddress
            {
                UserId = userId,
            };

            await userAddressController.PostAsync(userAddress);

            return Ok();
        }

        //------------------------User Profile Creation------------------------
        public async Task<IActionResult> CreateUserProfile(int userId)
        {
            UserProfileController userProfileController = new UserProfileController(_db);
            UserProfile userProfile = new UserProfile
            {
                UserId = userId,
            };

            await userProfileController.PostAsync(userProfile);

            return Ok();
        }
        public class LoginFaceBook
        {
            [Required]
            public string accessToken { get; set; }
        }

    }
}
