using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using BC = BCrypt.Net.BCrypt;
using Nicepet_API.Helpers;
using Nicepet_API.Helpers.JWT;

namespace Nicepet_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private ApiNicepetContext _db;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordController(ApiNicepetContext nicepetAPIContext, IEmailSender emailSender)
        {
            _db = nicepetAPIContext;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<UserInformation> PostAsync([FromBody] ForgotPassword item)
        {
            if (!ModelState.IsValid)
            {
                return new UserInformation();
            }


            UserInformation userInformation = new UserInformation();
            JwtToMail jwtToMail = new JwtToMail();
            TemplateForgotPasswordController templateForgotPasswordController = new TemplateForgotPasswordController(_db);

            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (item.ToSendMail)
                    {
                        List<User> userData = await _db.User.Where(a => a.Email == item.Email).ToListAsync(); 

                        if (userData.Count > 0)
                        {
                            string tokenGetted = jwtToMail.GenerateJwtToken(Convert.ToInt32(userData[0].Id), "ForgotPasswordNP");
                            string EmailBody = templateForgotPasswordController.Get(userData[0].Id.ToString(), tokenGetted).ToString();
                            await _emailSender.SendEmailAsync(item.Email, "Récupération de mot de passe", EmailBody, "Hello " + userData[0].FirstName);
                            userInformation = new UserInformation
                            {
                                Success = "Un email de récuperation de mot de passe vient de vous être envoyé !     "
                            };
                        }
                        else if(userData.Count == 0) 
                        {
                            userInformation = new UserInformation
                            {
                                Error = "Cette adresse mail n'a pas été trouvée dans notre base !    "
                            };
                        }
                    }
                    else if (!item.ToSendMail)
                    {
                        List<User> userData = await _db.User.Where(a => a.Id == item.Id).ToListAsync();
                        SignUp userIdInsideToken = jwtToMail.ValidateJwtToken(item.Token);
                        if (Convert.ToInt32(userIdInsideToken.UserId) == userData[0].Id && userIdInsideToken.Token == "ForgotPasswordNP")
                        {
                            if (userData.Count > 0 && item.NewPassword != null)
                            {
                                string cryptPwd = BC.HashPassword(item.NewPassword);
                                await PatchPassword(item.Id, cryptPwd);
                                 userInformation = new UserInformation
                                 {
                                     Success = "Félécitation votre mot de passe à été changé avec succès !   "
                                 };
                            }
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

        //---------------------------------------------- Patch Password -------------------------------------------
        public async Task<ActionResult> PatchPassword(int Id,string Pwd)
        {
            Delta<User> deltaUser = new Delta<User>();
            UserController userController = new UserController(_db);

            deltaUser.TrySetPropertyValue("Password", Pwd);
            await userController.PatchAsync(Id, deltaUser);
          
            return Ok(Id);
        }


      
    }
}
