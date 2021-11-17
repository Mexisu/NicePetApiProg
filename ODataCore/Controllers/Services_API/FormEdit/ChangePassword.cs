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

namespace Nicepet_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private ApiNicepetContext _db;


        public ChangePasswordController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

      
        [HttpPost]
        public async Task<String> PostAsync([FromBody] ChangePwd item)
        {
            // item contains Ids of ControllerPlanning and new time slots associated for DUPLICATION
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState).ToString();
            }


            try
            {
                using (var transaction = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                     List<User> usersList = await  _db.User
                    .Where(a => a.Id == item.Id)
                    .ToListAsync();


                    if (usersList.Count == 0) 
                    {
                        return "No existing ID";
                    }
                    else if (usersList.Count > 0)
                    {
                        if(BC.Verify(item.CurrentPassword, usersList[0].Password)) 
                        {
                            var cryptPwd = BC.HashPassword(item.NewPassword);
                            await PatchPassword(item.Id, cryptPwd);
                        }
                        else 
                        {
                            return "The password is not correct";
                        }
                    }
                    transaction.Complete();
                    return "Ok";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //---------------------------------------------- Patch Password -------------------------------------------
        public async Task<ActionResult> PatchPassword(int Id,string Pwd)
        {
            Delta<User> deltaUser = new Delta<User>();
            UserController userController = new UserController(_db);

            List<User> userData = new List<User>();
            userData = await _db.User
               .Where(a => a.Id == Id)
               .ToListAsync();
            foreach (User Item in userData)
            {
                deltaUser.TrySetPropertyValue("Password", Pwd);
                await userController.PatchAsync(Id, deltaUser);
            }
            return Ok(Id);
        }


        public class ChangePwd
        {
            [Required]
            public int Id { get; set; }
            [Required]
            public string CurrentPassword { get; set; }
            [Required]
            public string NewPassword { get; set; }
        }
    }
}
