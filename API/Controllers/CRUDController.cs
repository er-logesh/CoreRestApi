using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Mdoel;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CRUDController : ControllerBase
    {
        private readonly string repoFilePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName,
                                        "UserData.json");
        private readonly ILogger<CRUDController> _logger;
        public CRUDController(ILogger<CRUDController> _logger)
        {
            this._logger = _logger;
        }

        [HttpPost]
        public IActionResult Create(UserModel createUser)
        {
            List<UserModel> lstUserdata = new List<UserModel>();

            if (System.IO.File.Exists(repoFilePath))
            {
                lstUserdata = JsonConvert.DeserializeObject<List<UserModel>>(System.IO.File.ReadAllText(repoFilePath));
            }
            var alreadyExists = lstUserdata.Where(x => x.Email.Equals(createUser.Email));
            if (alreadyExists.Any())
            {
                return BadRequest("User already exists.");
            }
            else
            {
                System.IO.File.Delete(repoFilePath);
            }
            lstUserdata.Add(createUser);
            System.IO.File.AppendAllText(repoFilePath, JsonConvert.SerializeObject(lstUserdata));
            return Ok(true);
        }
        [HttpGet]
        public List<UserModel> Get()
        {
            List<UserModel> lstUserdata = new List<UserModel>();
            if (System.IO.File.Exists(repoFilePath))
            {
                lstUserdata = JsonConvert.DeserializeObject<List<UserModel>>(System.IO.File.ReadAllText(repoFilePath));
            }
            return lstUserdata;
        }
        [HttpPost]
        public IActionResult Update(UserModel updateUser)
        {
            List<UserModel> lstUserdata = new List<UserModel>();

            if (System.IO.File.Exists(repoFilePath))
            {
                lstUserdata = JsonConvert.DeserializeObject<List<UserModel>>(System.IO.File.ReadAllText(repoFilePath));
            }
            var alreadyExists = lstUserdata.Where(x => x.Email.Equals(updateUser.Email)).FirstOrDefault();
            if (alreadyExists != null)
            {
                lstUserdata.Remove(alreadyExists);
                alreadyExists.FirstName = updateUser.FirstName;
                alreadyExists.LastName = updateUser.LastName;
                alreadyExists.Age = updateUser.Age;
                alreadyExists.UserName = updateUser.UserName;
                lstUserdata.Add(alreadyExists);
                System.IO.File.Delete(repoFilePath);
                System.IO.File.AppendAllText(repoFilePath, JsonConvert.SerializeObject(lstUserdata));
                return Ok(alreadyExists);
            }
            return BadRequest("user doesn't exist");
        }
        [HttpPost]
        public IActionResult Delete(UserModel deleteuser){
            List<UserModel> lstUserdata = new List<UserModel>();

            if (System.IO.File.Exists(repoFilePath))
            {
                lstUserdata = JsonConvert.DeserializeObject<List<UserModel>>(System.IO.File.ReadAllText(repoFilePath));
            }
            var alreadyExists = lstUserdata.Where(x => x.Email.Equals(deleteuser.Email)).FirstOrDefault();
            if (alreadyExists != null)
            {
                lstUserdata.Remove(alreadyExists);
                System.IO.File.Delete(repoFilePath);
                System.IO.File.AppendAllText(repoFilePath, JsonConvert.SerializeObject(lstUserdata));
                return Ok(true);
            }
            return BadRequest("user doesn't exist");
        }
    }
}