﻿using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.JWT;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        private DecodeToken _decodeToken;
        private IConfiguration _configuration;

        public UserService(IUserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
        }

        public int DecodeToken(string token, string nameClaim)
        {
            return _decodeToken.Decode(token, nameClaim);
        }

        public async Task<ViewUser> GetById(string token, int id)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool isFullInfo = false;

            // if admin or him/her-self call api
            if (roleId.Equals(1) || userId.Equals(id)) isFullInfo = true;
            ViewUser user = await _userRepo.GetById(id, isFullInfo);
            return (user == null) ? throw new NullReferenceException() : user;
        }

        // for admin
        public async Task<PagingResult<ViewUser>> GetByUniversity(int universityId, UserStatus status, PagingRequest request)
        {
            PagingResult<ViewUser> users = await _userRepo.GetUserByUniversity(universityId, status, request);
            if (users == null) throw new NullReferenceException("Not found any students");
            return users;
        }

        private async Task<bool> CheckDuplicatedEmailAndUserCode(int? universityId, string email, string userCode)
        {
            bool isExisted = await _userRepo.CheckExistedEmail(email);
            if (isExisted) throw new ArgumentException("Duplicated Email");

            if (universityId != null)
            {
                isExisted = await _userRepo.CheckExistedUser(universityId.Value, userCode);
            }
            else
            {
                isExisted = await _userRepo.CheckExistedUser(userCode);
            }

            if (isExisted) throw new ArgumentException("Duplicated UserCode");

            return isExisted;
        }

        public async Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request)
        {
            PagingResult<ViewUser> users = await _userRepo.GetByCondition(request);
            if (users.Items == null) throw new NullReferenceException("Not found any users");
            return users;
        }

        public async Task<bool> Insert(string token, UserAccountInsertModel account)
        {
            if (string.IsNullOrEmpty(account.Email) || string.IsNullOrEmpty(account.Password))
                throw new ArgumentException("Email Null || Password Null");

            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            bool isExisted = await _userRepo.CheckExistedEmail(account.Email);
            if (isExisted) throw new ArgumentException("The email is already existed");

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var userAccount = await auth.CreateUserWithEmailAndPasswordAsync(account.Email, account.Password, "Admin", true);
                if (userAccount != null)
                {
                    Data.Models.DB.User user = new Data.Models.DB.User()
                    {
                        Email = account.Email,
                        RoleId = 1, // university admin
                        UniversityId = account.UniversityId,
                        Fullname = account.Fullname,
                        Dob = account.Dob,
                        Gender = account.Gender,
                        Status = UserStatus.Active, // default status when insert                        
                        IsOnline = false // default status
                    };

                    int id = await _userRepo.Insert(user);
                    return (id > 0) ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<string> LoginAccount(UserLoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentException("Email Null || Password Nulll");

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var account = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                if(account != null)
                {
                    UserTokenModel user = await _userRepo.GetByEmail(model.Email);
                    return (user != null) 
                        ? JWTUserToken.GenerateJWTTokenUser(user) 
                        : throw new ArgumentException();
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(UserUpdateModel model, string token)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int roleId = _decodeToken.Decode(token, "RoleId");

            if (!userId.Equals(model.Id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Data.Models.DB.User user = await _userRepo.Get(model.Id);
            if (user == null) throw new NullReferenceException("Not found this user");

            bool isInvalid = await CheckDuplicatedEmailAndUserCode(user.UniversityId, user.Email, user.StudentCode);
            if (isInvalid) return isInvalid;

            if (!string.IsNullOrEmpty(model.Description)) user.Description = model.Description;
            if (!string.IsNullOrEmpty(model.Dob)) user.Dob = model.Dob;
            if (!string.IsNullOrEmpty(model.Email)) user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Fullname)) user.Fullname = model.Fullname;
            if (!string.IsNullOrEmpty(model.Gender)) user.Gender = model.Gender;
            if (!string.IsNullOrEmpty(model.StudentCode)) user.StudentCode = model.StudentCode;
            if (!string.IsNullOrEmpty(model.Avatar)) user.Avatar = model.Avatar;
            if (model.DepartmentId.HasValue) user.DepartmentId = model.DepartmentId;

            // for admin
            if (model.RoleId != 0 && roleId.Equals(1)) user.RoleId = model.RoleId.Value;
            if (model.Status.HasValue && roleId.Equals(1)) user.Status = model.Status.Value;

            await _userRepo.Update();
            return true;
        }

        public async Task UpdateStatusOnline(int id, bool status)
        {
            Data.Models.DB.User element = await _userRepo.Get(id);
            if (element == null) throw new NullReferenceException("Not found this user");

            element.IsOnline = status;
            await _userRepo.Update();
        }

        public async Task<bool> Delete(int id)
        {
            Data.Models.DB.User user = await _userRepo.Get(id);
            if (user == null) throw new NullReferenceException("Not found this user");

            user.Status = UserStatus.InActive;
            await _userRepo.Update();
            return true;
        }



        //------------------------------------------------LOGIN------------------------------------------------

        // CheckUserEmail
        public async Task<bool> CheckUserEmailExsit(string email)
        {
            UserTokenModel user = await _userRepo.GetByEmail(email);
            return (user != null) ? true : false;
        }

        // firebase 
        public async Task<int> InsertNewUser(UserTokenModel userModel, string email, string phoneNumber)
        {
            try
            {
                Data.Models.DB.User user = new Data.Models.DB.User
                {
                    RoleId = userModel.RoleId,
                    Email = email,
                    Status = UserStatus.Active,
                    Avatar = userModel.Avatar,
                    Fullname = userModel.Fullname,
                    PhoneNumber = phoneNumber,
                    //auto
                    Dob = "",
                    Gender = "",
                    StudentCode = "",
                    Description = "",
                    IsOnline = true // default status when log in
                };
                return await _userRepo.Insert(user);

            }
            catch (Exception) { throw; }
        }

        public async Task UpdateInfoToken(int userId, int universityId, string token)
        {
            int idUser = _decodeToken.Decode(token, "Id");
            if (!userId.Equals(idUser)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Data.Models.DB.User user = await _userRepo.Get(userId);
            if (user == null) throw new NullReferenceException("Not found this user");

            if (!user.UniversityId.HasValue) user.UniversityId = universityId;
            await _userRepo.Update();
        }

        public async Task<UserTokenModel> GetUserByEmail(string email)
        {
            return await _userRepo.GetByEmail(email);
        }

        public async Task UpdateAvatar(int userId, string srcAvatar)
        {
            Data.Models.DB.User user = await _userRepo.Get(userId);
            if (user == null) throw new NullReferenceException("Not found this user");

            user.Avatar = srcAvatar;
            await _userRepo.Update();
        }

        public async Task<UserTokenModel> GetUserTokenById(int id, string token)
        {
            int userId = _decodeToken.Decode(token, "Id");
            if (!userId.Equals(id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            return await _userRepo.GetUserTokenById(id);
        }
    }
}
