using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;

namespace Nicepet_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateForgotPasswordController : ControllerBase
    {
        private ApiNicepetContext _db;

        public TemplateForgotPasswordController(ApiNicepetContext nicepetAPIContext)
        {
            _db = nicepetAPIContext;
        }

        [HttpGet]
        [Produces("text/html")]
        [AllowAnonymous]
        public string Get(string userId, string tokenPart)
        {
             string _Result = String.Format("<!doctype html>\r\n<html>\r\n<head>\r\n<meta charset=\"utf-8\">" +
                 "\r\n<title></title>\r\n</head>\r\n<body style=\"font-family:Gotham, 'Helvetica Neue', Helvetica," +
                 " Arial, sans-serif; background-color:#f0f2ea; margin:0; padding:0; color:#333333;\">\r\n\r\n\r\n     " +
                 "<a href=\'http://localhost:8080/session/Forgot-password?userId=" + userId + "&access=" + tokenPart + "\' target=\"_blank\">confirmer</a>\r\n   \r\n</html>\r\n", userId);

            //string _result = "<a href=\'http://localhost:8080/session/Forgot-password?userId=" + userId + "&access="+ tokenPart + "\' target=\"_blank\">confirmer</a>";

            string _result = "<form ><a href=\'http://localhost:8080/session/Forgot-password?userId=" + userId + "&step=2 &access=" + tokenPart + "\' target=\"_blank\">confirmer</a></form>";
            //string _result = @"<b>Return HTML content using Web API method </b>";
            return _result;
        }
    }

    public class HtmlOutputFormatter : StringOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add("text/html");
        }
    }
}
