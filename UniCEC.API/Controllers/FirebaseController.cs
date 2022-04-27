using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.JWT;
using UniCEC.Data.ViewModels.Entities.Role;
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
        private IRoleService _roleService;

        public FirebaseController(IUserService userService, IUniversityService universityService, IRoleService roleService)
        {
            _userService = userService;
            _universityService = universityService;
            _roleService = roleService;
        }

        // POST api/<FirebaseController>
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser()
        {
            try
            {
                //Get The IDToken From FE
                var header = Request.Headers;
                string idToken = "";
                if (header.ContainsKey("Authorization"))
                {
                    String tempID = header["Authorization"].ToString();
                    idToken = tempID.Split(" ")[1];
                }
                //decoded IDToken
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
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
                        if (userInfo.PhotoUrl != null)
                        {
                            userModelTemporary.Avatar = userInfo.PhotoUrl.ToString();
                        }
                        else
                        {
                            userModelTemporary.Avatar = "";
                        }
                        //auto Role Student - Status True 
                        //Database roleId 3 is Student
                        userModelTemporary.RoleId = 3;
                        //Add In DB [User]
                        ViewUser userTem = await _userService.InsertUserTemporary(userModelTemporary);
                        //
                        userModelTemporary.Id = userTem.Id.ToString();
                        //Get List University Belong To Email
                        List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(email_Uni);
                        //complete ViewUserInfo
                        //thông tin để trả ra cho BE để User Tiếp tục Update thông tin User
                        ViewRole role = await _roleService.GetByRoleId(userTem.RoleId);
                        userModelTemporary.RoleName = role.RoleName;
                        //----------------Generate JWT Token và kèm theo thông tin này lên FE để User tiếp tục update
                        string clientTokenUserTemp = JWTUserToken.GenerateJWTTokenUserTemp(userModelTemporary);
                        ViewUserInfo vui = new ViewUserInfo
                        {
                            tokenTemp = clientTokenUserTemp,
                            listUniBelongToEmail = listUniBelongToEmail
                        };
                        return Ok(vui);
                    }
                    //2.If Student in system
                    else
                    {
                        ViewUser user = await _userService.GetUserByEmail(email_user);
                        ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                        //2.1 Update FullFill Info
                        if (user.UniversityId != null)
                        {
                            //3.Student
                            if (role.Id == 3)
                            {
                                //----------------Generate JWT Token Student
                                string clientTokenUser = JWTUserToken.GenerateJWTTokenStudent(user, role.RoleName);
                                return Ok(clientTokenUser);
                            }
                        }
                        //2.2 Not Update FullFill Info
                        else
                        {
                            UserModelTemporary userModelTemporary = new UserModelTemporary()
                            {
                                Id = user.Id.ToString(),
                                Email = user.Email,
                                RoleName = role.RoleName,
                                RoleId = user.RoleId,

                            };
                            //Get List University Belong To Email
                            List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(email_Uni);
                            //----------------Generate JWT Token và kèm theo thông tin này lên FE để User tiếp tục update
                            string clientTokenUserTemp = JWTUserToken.GenerateJWTTokenUserTemp(userModelTemporary);
                            ViewUserInfo vui = new ViewUserInfo
                            {
                                tokenTemp = clientTokenUserTemp,
                                listUniBelongToEmail = listUniBelongToEmail
                            };
                            return Ok(vui);
                        }
                    }
                }
                //Not In University => Sponsor or Admin
                else
                {
                    //Check Role
                    ViewUser user = await _userService.GetUserByEmail(email_user);
                    if (user != null)
                    {
                        ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                        //1.Admin
                        if (role.Id == 1)
                        {
                            //----------------Generate JWT Token Admin
                            string clientTokenUser = JWTUserToken.GenerateJWTTokenAdmin(user, role.RoleName);
                            return Ok(clientTokenUser);
                        }
                        //2.Sponsor
                        if (role.Id == 2)
                        {
                            //----------------Generate JWT Token Sponsor
                            string clientTokenUser = JWTUserToken.GenerateJWTTokenSponsor(user, role.RoleName);
                            return Ok(clientTokenUser);
                        }
                    }
                    //NOT IN SYSTEM
                    else
                    {
                        return BadRequest();
                    }
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }
            return BadRequest();
        }
    }


}
