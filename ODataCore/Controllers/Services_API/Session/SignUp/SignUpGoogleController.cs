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
using Newtonsoft.Json;
using NUnit.Framework;
using Google.Apis.Auth;

namespace Nicepet_API.Controllers.Services_API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpGoogleController : ControllerBase
    {
        private ApiNicepetContext _db;

        public SignUpGoogleController(ApiNicepetContext nicepetAPIContext)
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
                    var validPayload = await GoogleJsonWebSignature.ValidateAsync(item.IdToken);
                    Assert.IsNotNull(validPayload);

                    string fbUserId = validPayload.Email;

                    if (validPayload.EmailVerified)
                    {
                        List<User> userSignUpList = await _db.User
                       .Where(a => a.GoogleAccount == fbUserId )
                       .ToListAsync();
                        if (userSignUpList.Count > 0)
                        {
                            userInformation = new UserInformation
                            {
                                Error = "L'adresse e-mail est déjà utilisée !  "
                            };
                        }
                        else if (userSignUpList.Count == 0)
                        {
                            await CreateAccount(fbUserId);

                            User userSignUp = await _db.User
                                .Where(a => a.GoogleAccount == fbUserId)
                                .FirstAsync();

                            await CreateUserAddress(userSignUp.Id);
                            await CreateUserProfile(userSignUp.Id);

                            userInformation = new UserInformation
                            {
                                Success = "Félicitation votre compte Google à été créé avec succès ! "
                            };
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
                GoogleAccount = fb_user_id,
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
            public string IdToken { get; set; }
        }

    }
}
