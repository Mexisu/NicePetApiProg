using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Helpers;
using Nicepet_API.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Nicepet_API.Helpers.ReturnUserInformationController;

namespace Nicepet_API.Controllers.Session
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginGoogleController : ControllerBase
    {
        private ApiNicepetContext _db;

        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;

       public LoginGoogleController(ApiNicepetContext nicepetAPIContext, IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher)
        {
            _db = nicepetAPIContext;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] LoginGoogle item)
        {
            // item contains Ids of ControllerPlanning and new time slots associated for DUPLICATION
            if (!ModelState.IsValid)
            {
                return new UserInformation();
            }
            UserInformation userInformation = new UserInformation();
            //ReturnUserInformationController returnUserInformationController = new ReturnUserInformationController();
            //string ErrorMessage = "";
            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var validPayload = await GoogleJsonWebSignature.ValidateAsync(item.IdToken);
                    Assert.IsNotNull(validPayload);
                    if (validPayload.EmailVerified) 
                    {
                      List<User> userLogingList = await _db.User
                     .Where(a => a.GoogleAccount == validPayload.Email)
                     .Include(a => a.UserType)
                     .ToListAsync();

                        if(userLogingList.Count > 0)
                        {
                            userLogingList[0].Email = userLogingList[0].GoogleAccount;
                            userInformation = await ReturnUserInformation(jWTAuthenticationManager,_db, userLogingList);
                            userInformation.Success = "Félicitation vous êtes connecté via votre compte google !  ";
                        }
                        else if (userLogingList.Count == 0) 
                        {
                            userInformation.Error = "pas de compte google !  ";
                        }
                    }
                    else
                    {
                        //ErrorMessage = "Faild Google token";
                        userInformation.Error = "un problème est survenu lors de la connexion à google !  ";
                    }
                    transaction.Complete();

                   return userInformation;
                }
            }
            catch (Exception ex)
            {
                return userInformation = new UserInformation
                {
                    Error = ex.Message
                };
            }
        }

        public class LoginGoogle
        {
            [Required]
            public string IdToken { get; set; }
        }
    

    }
}
