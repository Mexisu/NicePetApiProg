using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nicepet_API.Models;

namespace Nicepet_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
       // private NicepetAPIContext _db;

        //[Authorize]
        [HttpGet]
        [ServiceFilter(typeof(PathFilter))]

        public ActionResult<IEnumerable<string>> Get() 
        {
            return new string[] { "Welcome to Nicepet API" };
        }
    }
}
