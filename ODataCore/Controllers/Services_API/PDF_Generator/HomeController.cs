using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nicepet_API.Models;
using Pdf417EncoderLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace Nicepet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly IGeneratePdf _generatePdf;
        public static IWebHostEnvironment _environment;
        public HomeController(IGeneratePdf generatePdf, IWebHostEnvironment environment)
        {
            _generatePdf = generatePdf;
            _environment = environment;
        }
        
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetEmployeeInfo()
        {
            var empObj = new EmployeeInfo
            {
                EmpId="1001",
                EmpName="Arthur",
                Department="Lille",
                Designation="Dev Engineer"
            };
            var generatedPdf2 = await _generatePdf.GetByteArray("Views/Employee/EmployeeInfo.cshtml", empObj);
            string filePath = _environment.WebRootPath + "/PdfFiles/test4.pdf";
            
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllBytes(filePath, generatedPdf2);
            }
            return await _generatePdf.GetPdf("Views/Employee/EmployeeInfo.cshtml", empObj);
        }
    }
}
