using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.RequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FirebaseController : ControllerBase
    {

        private IUserService _userService;
        private IUniversityService _universityService;

        public FirebaseController(IUserService userService, IUniversityService universityService)
        {
            _userService = userService;
            _universityService = universityService; 
        }

        // GET: api/<FirebaseController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FirebaseController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FirebaseController>
        [HttpPost("Authenicated/User")]
        public async Task<IActionResult> AuthenticateUser()
        {
            try
            {
                //Get The IDToken From FE
                //... continute

                //decoded IDToken
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync("IdToken");
                string uid = decodedToken.Uid;
            
                //lấy user info
                UserRecord userInfo = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

                //-----------University
                string email_user = userInfo.Email;
                string[] arrList = email_user.Split('@');
                string email_Uni = arrList[1]; 
                bool checkUni = await _universityService.CheckEmailUniversity(email_Uni);
                if (checkUni)
                {
                    //-------Check User
                    bool checkExistUser = await _userService.CheckUserEmailExsit(email_user);
                    //1.If User not in system
                    if (checkExistUser == false)
                    {
                
                    }
                    //2.If User in system
                    else
                    { 
                    
                    
                    }
                   

                }
                else {
                    BadRequest();
                }





            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }

            

            return Ok();
        }



        // PUT api/<FirebaseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FirebaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
