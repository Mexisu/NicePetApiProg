using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nicepet_API.Models;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicepet_API.Controllers.Services_API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotativaPDFController : ControllerBase
    {
        private ApiNicepetContext _db;
        public static IWebHostEnvironment _environment;

        public RotativaPDFController(ApiNicepetContext nicepetAPIContext, IWebHostEnvironment environment)
        {
            _db = nicepetAPIContext;
            _environment = environment;
        }

   

        public IActionResult Index()
        {
            List<Customer> customerList = new List<Customer>()
             {
                 new Customer { CustomerID = 1, Address = "Taj Lands Ends 1", City = "Mumbai" , Country ="India", Name ="Sai", Phoneno ="9000000000"},
                 new Customer { CustomerID = 2, Address = "Taj Lands Ends 2", City = "Mumbai" , Country ="India", Name ="Ram", Phoneno ="9000000000"},
                 new Customer { CustomerID = 3, Address = "Taj Lands Ends 3", City = "Mumbai" , Country ="India", Name ="Sainesh", Phoneno ="9000000000"},
                 new Customer { CustomerID = 4, Address = "Taj Lands Ends 4", City = "Mumbai" , Country ="India", Name ="Saineshwar", Phoneno ="9000000000"},
                 new Customer { CustomerID = 5, Address = "Taj Lands Ends 5", City = "Mumbai" , Country ="India", Name ="Saibags", Phoneno ="9000000000"}

             };

            var report = new ViewAsPdf("Index", customerList)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Legal,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
            };

            var bytes = report.BuildFile(ControllerContext).Result;
            string filePath = _environment.WebRootPath + "/PdfFiles/test13.pdf";
            if (!System.IO.File.Exists(filePath))
            {

                System.IO.File.WriteAllBytes(filePath, bytes);
            }
            return report;
        }
    }
}
