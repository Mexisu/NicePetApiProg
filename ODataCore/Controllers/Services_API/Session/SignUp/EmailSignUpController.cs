using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using Nicepet_API.Helpers;
using Nicepet_API.Helpers.JWT;
using System.Transactions;
using Microsoft.AspNet.OData;

namespace Nicepet_API.Controllers.Services_API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSignUpController : ControllerBase
    {
        private ApiNicepetContext _db;
        private readonly IEmailSender _emailSender;

        public EmailSignUpController(ApiNicepetContext nicepetAPIContext, IEmailSender emailSender)
        {
            _db = nicepetAPIContext;
            _emailSender = emailSender;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] SignUp item)
        {
            if (!ModelState.IsValid)
            {
                return new UserInformation();
            }

            UserInformation userInformation = new UserInformation();
            JwtToMail jwtToMail = new JwtToMail();
            TemplateValidAccountController templateValidAccountController = new TemplateValidAccountController(_db);

            //string ErrorMessage = "";
            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    
                        List<User> userData = await _db.User.Where(a => a.Id == item.UserId).ToListAsync();

                        SignUp userIdInsideToken = jwtToMail.ValidateJwtToken(item.Token);
                        if (Convert.ToInt32(userIdInsideToken.UserId) == userData[0].Id && userIdInsideToken.Token == "EmailSignUp")
                        {
                            if (userData.Count > 0)
                            {
                                await PatchIsValidEmail(item.UserId);
                            }
                        }
                    transaction.Complete();

                    return userInformation = new UserInformation
                    {
                        Success = "Félicitation votre compte à été activé avec succès !  "
                    };
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

        //---------------------------------------------- Patch Password -------------------------------------------
        public async Task<ActionResult> PatchIsValidEmail(int Id)
        {
            Delta<User> deltaUser = new Delta<User>();
            UserController userController = new UserController(_db);

            List<User> userData = new List<User>();
            userData = await _db.User
               .Where(a => a.Id == Id)
               .ToListAsync();
            foreach (User Item in userData)
            {
                deltaUser.TrySetPropertyValue("IsValidEmail", true);
                await userController.PatchAsync(Id, deltaUser);
            }
            return Ok(Id);
        }
    }
}
