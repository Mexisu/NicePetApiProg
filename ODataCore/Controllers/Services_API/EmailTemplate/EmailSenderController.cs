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

namespace Nicepet_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController : ControllerBase
    {
        private ApiNicepetContext _db;
        private readonly IEmailSender _emailSender;

        public EmailSenderController(ApiNicepetContext nicepetAPIContext, IEmailSender emailSender)
        {
            _db = nicepetAPIContext;
            _emailSender = emailSender;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromBody] EmailContent emailData)
        {
            // item contains Ids of ControllerPlanning and new time slots associated for DUPLICATION
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _emailSender.SendEmailAsync(emailData.Email, emailData.Subject, $""+ emailData.EmailBody + "", emailData.TemplateUrl);

                return Ok();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public class EmailContent

        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required]
            public string Subject { get; set; }
           
            public string EmailBody { get; set; }
            public string TemplateUrl { get; set; }
        }
       
    }
}
