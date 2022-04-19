using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.University;
using UniCEC.Data.ViewModels.Entities.User;
using UniCEC.Data.ViewModels.Firebase.Auth;

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
                //check Uni in System or not
                if (checkUni)
                {
                    //-------Check User
                    bool checkExistUser = await _userService.CheckUserEmailExsit(email_user);
                    //1.If User not in system
                    if (checkExistUser == false)
                    {
                        //Create UserModelTemporary
                        UserModelTemporary userModelTemporary = new UserModelTemporary();
                        userModelTemporary.Email = email_user;
                        //auto Role Student - Status True 
                        userModelTemporary.RoleId = 3;
                        //Add In DB [User]
                        ViewUser userTem = await _userService.InsertUserTemporary(userModelTemporary);
                        //Get List University Belong To Email
                        List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(email_Uni);
                        //complete ViewUserInfo
                        //thông tin để trả ra cho BE để User Tiếp tục Update thông tin User
                        ViewUserInfo vui = new ViewUserInfo
                        {
                            Email = userTem.Email,
                            RoleId = userTem.RoleId,
                            listUniBelongToGmail = listUniBelongToEmail
                        };
                        //----------------Generate JWT Token và kèm theo thông tin này lên FE

                    }
                    //2.If User in system
                    else
                    {
                        //Check Role
                        //----------------Generate JWT Token
                    }
                }
                //Not In System
                else
                {
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
