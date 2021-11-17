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
using BC = BCrypt.Net.BCrypt;

namespace Nicepet_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ApiNicepetContext _db;

        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;

        public LoginController(ApiNicepetContext nicepetAPIContext, IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher)
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
            ReturnUserInformationController returnUserInformationController = new ReturnUserInformationController();
            //string ErrorMessage = "";
            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                     List<User> userLogingList = await  _db.User
                    .Include(a =>a.UserType).Where(a => a.Email == item.Email)
                    .ToListAsync();


                    if (userLogingList.Count == 0) 
                    {
                        //ErrorMessage = "No existing mail";
                        userInformation = new UserInformation
                        {
                            Error = "Ce compte n'existe pas !   "
                        };
                    }
                    else if (userLogingList.Count > 0)
                    {
                      //  if(BC.Verify(item.Password, userLogingList[0].Password)) 
                        if(item.Password == userLogingList[0].Password) 
                        {
                            userInformation = new UserInformation
                            {
                                Success = "Félicitation vous êtes connecté ! "
                            };
                            userInformation = await ReturnUserInformationController.ReturnUserInformation(jWTAuthenticationManager, _db, userLogingList);
                        }
                        else 
                        {
                            //ErrorMessage = "The password is not correct";
                            userInformation = new UserInformation
                            {
                                Error = "Ce mot de passe est erroné !  "
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
    }
}
