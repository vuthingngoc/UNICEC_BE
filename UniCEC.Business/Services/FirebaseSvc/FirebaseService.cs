using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Services.RoleSvc;
using UniCEC.Business.Services.SeedsWalletSvc;
using UniCEC.Business.Services.UniversitySvc;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.JWT;
using UniCEC.Data.ViewModels.Entities.Notification;
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
        private ISeedsWalletService _seedsWalletService;
        private INotificationService _notificationService;

        public FirebaseService(IUserService userService, IUniversityService universityService
                                , IRoleService roleService, ISeedsWalletService seedsWalletService
                                , INotificationService notificationService)
        {
            _userService = userService;
            _universityService = universityService;
            _roleService = roleService;
            _seedsWalletService = seedsWalletService;
            _notificationService = notificationService;
        }

        public async Task<ViewUserInfo> Authentication(string token, string deviceToken)
        {
            // decoded IDToken
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            string uid = decodedToken.Uid;
            // Get user info
            UserRecord userInfo = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
            string email = userInfo.Email;
            string emailUni = email.Split('@')[1];
            // check email university in system or not
            bool isStudent = await _universityService.CheckEmailUniversity(emailUni);
            if (isStudent)
            {
                //-------Check student
                bool isExistedStudent = await _userService.CheckUserEmailExsit(email);
                //1.If student not in system
                if (isExistedStudent == false)
                {
                    //Create NewUserModel
                    UserTokenModel userModel = new UserTokenModel()
                    {
                        Fullname = userInfo.DisplayName,
                        Avatar = (string.IsNullOrEmpty(userInfo.PhotoUrl)) ? "" : userInfo.PhotoUrl,
                        RoleId = 3 // role student
                    };
                    //Add In DB [User]
                    string phoneNumber = (string.IsNullOrEmpty(userInfo.PhoneNumber)) ? "" : userInfo.PhoneNumber;
                    userModel.Id = await _userService.InsertNewUser(userModel, email, phoneNumber, deviceToken);
                    ViewRole role = await _roleService.GetByRoleId(userModel.RoleId);
                    userModel.RoleName = role.RoleName;
                    userModel.UniversityId = 0;

                    // create seeds wallet
                    await _seedsWalletService.InsertSeedsWallet(userModel.Id);

                    //----------------Generate JWT Token và kèm theo thông tin này lên FE để User tiếp tục update
                    string userToken = JWTUserToken.GenerateJWTTokenUser(userModel);
                    // Get List University Belong To Email
                    List<ViewUniversity> listUniBelongToEmail = await _universityService.GetListUniversityByEmail(emailUni);                    

                    return new ViewUserInfo()
                    {
                        Token = userToken,
                        ListUniBelongToEmail = listUniBelongToEmail
                    };
                }
                //2.If Student in system
                else
                {
                    UserTokenModel user = await _userService.GetUserByEmail(email);
                    // check user is active or inactive
                    if (user.Status.Equals(UserStatus.InActive)) throw new UnauthorizedAccessException("Your account is inactive now! Please contact with admin to be supported.");

                    bool isOnline = true;
                    await _userService.UpdateInfoUserLogin(user.Id, userInfo.PhotoUrl, isOnline, deviceToken);
                    user.Avatar = userInfo.PhotoUrl;
                    //await _userService.UpdateAvatar(user.Id, userInfo.PhotoUrl);
                    //await _userService.UpdateStatusOnline(user.Id, true);

                    string userToken = JWTUserToken.GenerateJWTTokenUser(user);
                    if (user.UniversityId == 0)
                    {
                        return new ViewUserInfo()
                        {
                            Token = userToken,
                            ListUniBelongToEmail = await _universityService.GetListUniversityByEmail(emailUni)
                        };
                    }

                    return new ViewUserInfo()
                    {
                        Token = userToken
                    };
                }
            }
            //Not In University => Admin
            else
            {
                //Check Role
                UserTokenModel user = await _userService.GetUserByEmail(email);

                if (user != null)
                {
                    if (user.Status.Equals(UserStatus.InActive))
                        throw new UnauthorizedAccessException("Your account is inactive now! Please contact with admin to be supported.");

                    await _userService.UpdateAvatar(user.Id, userInfo.PhotoUrl);
                    user.Avatar = userInfo.PhotoUrl;
                    await _userService.UpdateStatusOnline(user.Id, true);

                    string userToken = JWTUserToken.GenerateJWTTokenUser(user);

                    return new ViewUserInfo()
                    {
                        Token = userToken
                    };
                }
            }

            // NOT IN SYSTEM && INVALID EMAIL
            return null;
        }
    }
}
