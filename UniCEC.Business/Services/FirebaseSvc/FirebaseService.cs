using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
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

namespace UniCEC.Business.Services.FirebaseSvc
{
    public class FirebaseService : IFirebaseService
    {
        private IUserService _userService;
        private IUniversityService _universityService;
        private IRoleService _roleService;

        public FirebaseService(IUserService userService, IUniversityService universityService, IRoleService roleService)
        {
            _userService = userService;
            _universityService = universityService;
            _roleService = roleService;
        }

        public async Task<ViewUserInfo> Authentication(string token)
        {
            // decoded IDToken
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            string uid = decodedToken.Uid;
            // Get user info
            UserRecord userInfo = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            //-----------University
            string email = userInfo.Email;
            string emailUni = email.Split('@')[1];
            bool isStudent = await _universityService.CheckEmailUniversity(emailUni);

            // check email university in system or not
            if (isStudent)
            {
                //-------Check student
                bool isExistedStudent = await _userService.CheckUserEmailExsit(email);
                //1.If student not in system
                if (isExistedStudent == false)
                {
                    //Create UserModelTemporary
                    UserModelTemporary userModelTemporary = new UserModelTemporary();
                    userModelTemporary.Email = email;
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
                    ViewUser userTemp = await _userService.InsertUserTemporary(userModelTemporary);
                    await _userService.UpdateStatusOnline(userTemp.Id, true);
                    //
                    userModelTemporary.Id = userTemp.Id.ToString();
                    //Get List University Belong To Email
                    List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(emailUni);
                    //complete ViewUserInfo
                    //thông tin để trả ra cho BE để User Tiếp tục Update thông tin User
                    ViewRole role = await _roleService.GetByRoleId(userTemp.RoleId);
                    userModelTemporary.RoleName = role.RoleName;
                    //----------------Generate JWT Token và kèm theo thông tin này lên FE để User tiếp tục update
                    string clientTokenUserTemp = JWTUserToken.GenerateJWTTokenUserTemp(userModelTemporary);
                    return new ViewUserInfo
                    {
                        Token = clientTokenUserTemp,
                        ListUniBelongToEmail = listUniBelongToEmail
                    };
                }
                //2.If Student in system
                else
                {
                    ViewUser user = await _userService.GetUserByEmail(email);
                    await _userService.UpdateStatusOnline(user.Id, true);
                    ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                    //2.1 FullFill Info
                    if (user.UniversityId != null)
                    {
                        //3.Student
                        if (role.Id == 3)
                        {
                            //----------------Generate JWT Token Student
                            string clientTokenUser = JWTUserToken.GenerateJWTTokenStudent(user, role.RoleName);
                            return new ViewUserInfo()
                            {
                                Token = clientTokenUser
                            };
                        }
                    }
                    //2.2 Not FullFill Info
                    else
                    {
                        UserModelTemporary userModelTemporary = new UserModelTemporary()
                        {
                            Id = user.Id.ToString(),
                            Email = user.Email,
                            Fullname = user.Fullname,
                            RoleName = role.RoleName,
                            RoleId = user.RoleId,
                            Avatar = user.Avatar
                        };
                        //Get List University Belong To Email
                        List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(emailUni);
                        //----------------Generate JWT Token và kèm theo thông tin này lên FE để User tiếp tục update
                        string clientTokenUserTemp = JWTUserToken.GenerateJWTTokenUserTemp(userModelTemporary);
                        return new ViewUserInfo
                        {
                            Token = clientTokenUserTemp,
                            ListUniBelongToEmail = listUniBelongToEmail
                        };
                    }
                }
            }
            //Not In University => Sponsor or Admin
            else
            {
                //Check Role
                ViewUser user = await _userService.GetUserByEmail(email);
                if (user != null)
                {
                    await _userService.UpdateStatusOnline(user.Id, true);
                    ViewRole role = await _roleService.GetByRoleId(user.RoleId);
                    //1.Admin
                    if (role.Id == 1)
                    {
                        //----------------Generate JWT Token Admin
                        string clientTokenUser = JWTUserToken.GenerateJWTTokenAdmin(user, role.RoleName);
                        return new ViewUserInfo()
                        {
                            Token = clientTokenUser
                        };
                    }
                    //2.Sponsor
                    if (role.Id == 2)
                    {
                        //----------------Generate JWT Token Sponsor
                        string clientTokenUser = JWTUserToken.GenerateJWTTokenSponsor(user, role.RoleName);
                        return new ViewUserInfo()
                        {
                            Token = clientTokenUser
                        };
                    }
                }                
            }

            // NOT IN SYSTEM && INVALID EMAIL
            return null;
        }
    }
}
