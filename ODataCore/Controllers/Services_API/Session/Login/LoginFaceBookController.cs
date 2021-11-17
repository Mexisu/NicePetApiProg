using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Helpers;
using Nicepet_API.Models;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Nicepet_API.Helpers.ReturnUserInformationController;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Nicepet_API.Controllers.Session
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginFaceBookController : ControllerBase
    {
        private ApiNicepetContext _db;
        private string facebookAppToken = "840285756828186|ScC68BIYOA8_QJPbYYAR3y4FiWA" ;
        private string facebookAppId = "840285756828186";
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private readonly ITokenRefresher tokenRefresher;

       public LoginFaceBookController(ApiNicepetContext nicepetAPIContext, IJWTAuthenticationManager jWTAuthenticationManager, ITokenRefresher tokenRefresher)
        {
            _db = nicepetAPIContext;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.tokenRefresher = tokenRefresher;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInformation> PostAsync([FromBody] LoginFaceBook item)
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
                        List<User> userLogingList = await _db.User
                       .Where(a => a.FacebookAccount == fbUserId)
                       .Include(a => a.UserType)
                       .ToListAsync();
                        if(userLogingList.Count>0)
                        {
                            //userLogingList[0] = new User
                            //{
                            //    Email = fbUserId,
                            //};
                            userInformation.Success = "Félicitation  !  ";
                            userInformation = await ReturnUserInformationController
                            .ReturnUserInformation(jWTAuthenticationManager, _db, userLogingList);
                        }
                        else if (userLogingList.Count == 0)
                        {


                            userInformation.Error = "Pas de compte fb  !  ";
                        }

                    }
                    else if (!model.is_valid )
                    {
                        //ErrorMessage = "Faild FaceBook token";
                        userInformation.Error = "un problème est survenu lors de la connexion à facebook !  " + model.is_valid.toString();
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

        public class LoginFaceBook
        {
            [Required]
            public string accessToken { get; set; }
        }

      

    }
}
