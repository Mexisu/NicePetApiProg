using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Nicepet_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.OData;
using System.Transactions;
using Microsoft.Extensions.FileProviders;

namespace Nicepet_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadingController : ControllerBase
    {
        private ApiNicepetContext _db;

        public static IWebHostEnvironment _environment;
        public FileUploadingController(IWebHostEnvironment environment, ApiNicepetContext nicepetAPIContext)
        {
            _environment = environment;
            _db = nicepetAPIContext;
        }
       
        public class FileUplaodAPI
        {
            public IFormFile files { get; set; }
        }
        [HttpPost]
        public async Task<string> Post([FromForm]FileUplaodAPI objFile)
        {
            if (objFile.files.Length > 0)
            {
                try
                {
                    string userId = objFile.files.FileName.Split("\\")[0];
                    string shallowFolder = objFile.files.FileName.Split("\\")[1];
                    string deepFolder = objFile.files.FileName.Split("\\")[2];
                    string fileName = objFile.files.FileName.Split("\\")[3];
                    string ProfileId = objFile.files.FileName.Split("\\")[4];



                    //test method
                    //userId = "24";
                    string path = await GeneratePath(userId, shallowFolder, deepFolder);

                   
                    if (!Directory.Exists(_environment.WebRootPath + "\\" + path))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\" + path);
                    }

                    if (shallowFolder == "User")
                    {
                        await PatchUsersFileName(ProfileId, fileName, deepFolder);
                    }
                    else if (shallowFolder == "Animal")
                    {
                        await PatchAnimalsFileName(ProfileId, fileName, deepFolder);
                    }

                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\" + path + fileName.ToString()))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        return (path + fileName).ToString();
                    }

                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }
            
        }
        //--------------------------------------------- Path Creation ----------------------------------------------
        public async Task<string> GeneratePath(string Id, string shallowFolder, string deepFolder)
        {
            string path = shallowFolder;
            foreach (char element in Id) 
            {
                path += "\\" + element;
            }
            path += "\\" + Id + "\\" + deepFolder + "\\";

            return path;
        }

        //---------------------------------------------- Patch User File Name-------------------------------------------
        public async Task<ActionResult> PatchUsersFileName(string userProfileId, string fileName, string deepFolder)
        {
            Delta<UserProfile> deltaUserProfile = new Delta<UserProfile>();
            UserProfileController userProfileController = new UserProfileController(_db);

            List<UserProfile> createdAvatar = new List<UserProfile>();
            createdAvatar = await _db.UserProfile
               .Where(a => a.Id == Convert.ToInt32(userProfileId))
               .ToListAsync();
            foreach (UserProfile Item in createdAvatar)
            {
                deltaUserProfile.TrySetPropertyValue(deepFolder, fileName);
                await userProfileController.PatchAsync(Convert.ToInt32(userProfileId), deltaUserProfile);
            }

            return Ok(userProfileId);
        }

        //---------------------------------------------- Patch Animal File Name-------------------------------------------
        public async Task<ActionResult> PatchAnimalsFileName(string animalProfileId, string fileName, string deepFolder)
        {
            Delta<AnimalProfile> deltaAnimalProfile = new Delta<AnimalProfile>();
            AnimalProfileController animalProfileController = new AnimalProfileController(_db);

            List<AnimalProfile> createdAvatar = new List<AnimalProfile>();
            createdAvatar = await _db.AnimalProfile
               .Where(a => a.Id == Convert.ToInt32(animalProfileId))
               .ToListAsync();
            foreach (AnimalProfile Item in createdAvatar)
            {
                deltaAnimalProfile.TrySetPropertyValue(deepFolder, fileName);
                await animalProfileController.PatchAsync(Convert.ToInt32(animalProfileId), deltaAnimalProfile);
            }

            return Ok(animalProfileId);
        }

    }
}
