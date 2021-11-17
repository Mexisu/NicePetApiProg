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

namespace Nicepet_API.Controllers.Services_API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private ApiNicepetContext _db;
        private readonly IEmailSender _emailSender;


        public string Email = @"^([\w]+)@([\w]+)\.([\w]+)$";
        public string Name = "/^[a-zA-Z,.'-]+$/i";
        public string Phone = "/^(\\+33\\s[1-9]{8})|(0[1-9]\\s{8})$/";

        public bool ValidRegex(string item, string reg)
        {
            Regex myRegex = new Regex(reg);
            return myRegex.IsMatch(item); // retourne true ou false selon la vérification
        }

        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public SignUpController(ApiNicepetContext nicepetAPIContext, IJWTAuthenticationManager jWTAuthenticationManager, IEmailSender emailSender)
        {
            _db = nicepetAPIContext;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _emailSender = emailSender;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] UserSignUp item)
        {
            UserInformation userInformation = new UserInformation();

            if (!ModelState.IsValid)
            {
                return userInformation = new UserInformation
                {
                    Error = BadRequest(ModelState).ToString()
                };
            }

            JwtToMail jwtToMail = new JwtToMail();
            TemplateValidAccountController templateEditorController = new TemplateValidAccountController(_db);


            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    List<User> userSignUpList = await _db.User
                   .Where(a => a.Email == item.Email)
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
                        await CreateAccount(item);

                        User userSignUp = await _db.User.Where(a => a.Email == item.Email).FirstAsync();

                        await CreateUserAddress(userSignUp.Id);
                        await CreateUserProfile(userSignUp.Id);

                        string tokenGetted = jwtToMail.GenerateJwtToken(userSignUp.Id, "EmailSignUp");
                        string EmailBody = templateEditorController.Get(userSignUp.Id.ToString(), tokenGetted).ToString();
                        await _emailSender.SendEmailAsync(userSignUp.Email, "Activez votre compte", EmailBody, "Hello " + userSignUp.FirstName);

                        userInformation = new UserInformation
                        {
                            Success = "Félicitation votre compte à été créé avec succès ! "
                        };
                        //Un email pour activer votre compte vient de vous être envoyé.
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
        public async Task<IActionResult> CreateAccount(UserSignUp item)
        {
            UserController userController = new UserController(_db);

            if (!ValidRegex(item.Email, Email))
            {
            }

            User user = new User
            {
                Email = item.Email,
                Password = item.Password
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

        public class UserSignUp
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            //[Required]
            //public int? UserTypeId { get; set; }
        }
    }
}
