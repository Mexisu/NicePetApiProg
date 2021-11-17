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
using System.Transactions;
using Nicepet_API.Helpers;
using BC = BCrypt.Net.BCrypt;


namespace Nicepet_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private ApiNicepetContext _db;

        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;

        public UserLoginController(ApiNicepetContext nicepetAPIContext, IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher)
        {
            _db = nicepetAPIContext;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
        }

      
        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] UserLoginIn item)
        {
            // item contains Ids of ControllerPlanning and new time slots associated for DUPLICATION
            if (!ModelState.IsValid)
            {
                return new UserInformation();
            }
              UserInformation userInformation = new UserInformation();
            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                     List<User> userLogingList = await  _db.User
                    .Include(a =>a.UserType).Where(a => a.Email == item.Email)
                    .ToListAsync();

                    if(userLogingList.Count == 0) 
                    {
                        //ErrorMessage = "No existing mail";
                        userInformation = new UserInformation
                        {
                            Error = "No existing mail"
                        };
                    }
                    else if (userLogingList.Count > 0)
                    {
                       // if(BC.Verify(item.Password, userLogingList[0].Password))
                            if (item.Password == userLogingList[0].Password)
                            {
                            //ErrorMessage = "Good";
                            var token = jWTAuthenticationManager.Authenticate(userLogingList[0].FirstName, userLogingList[0].UserType.Type);

                            userInformation = new UserInformation
                            {
                                UserId = userLogingList[0].Id,
                                FirstName = userLogingList[0].FirstName,
                                LastName = userLogingList[0].LastName,
                                Phone = userLogingList[0].PhoneNumber,
                                AccessToken = token.JwtToken,
                                Success = "Connexion s'est éffectué avec succès ! "
                            };
                        }
                        else 
                        {
                            //ErrorMessage = " The password is not correct";
                            userInformation = new UserInformation
                            {
                                Error = " The password is not correct"
                            };
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
                    Error = ex.Message
                };
            }
        }

        public class UserLoginIn

        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }

        public class UserInformation
        {
            public int? UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName  { get; set; }
            public string Phone { get; set; }
            public string Error { get; set; }
            public string Success { get; set; }
            public string AccessToken { get; set; }
        }
    }
}
