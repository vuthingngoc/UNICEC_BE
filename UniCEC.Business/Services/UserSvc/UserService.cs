﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        private ViewUser TransformViewModel(User user)
        {
            return new ViewUser()
            {
                Id = user.Id,
                Description = user.Description,
                Dob = user.Dob,
                Email = user.Email,
                Fullname = user.Fullname,
                Gender = user.Gender,
                MajorId = user.MajorId,
                RoleId = user.RoleId,
                UniversityId = user.UniversityId,
                UserId = user.UserId,
                Status = user.Status
            };
        }

        public async Task<PagingResult<ViewUser>> GetAllPaging(PagingRequest request)
        {
            PagingResult<User> users = await _userRepo.GetAllPaging(request);
            if (users.Items != null)
            {
                List<ViewUser> listViewUser = new List<ViewUser>();
                users.Items.ForEach(e =>
                {
                    ViewUser viewUser = TransformViewModel(e);
                    listViewUser.Add(viewUser);
                });

                return new PagingResult<ViewUser>(listViewUser, users.TotalCount, users.CurrentPage, users.PageSize);
            }

            throw new NullReferenceException("Not Found");
        }

        public async Task<ViewUser> GetUserByUserId(string userId)
        {
            User user = await _userRepo.GetByUserId(userId);
            if (user == null) throw new NullReferenceException("Not Found");
            return TransformViewModel(user);
        }

        public async Task<ViewUser> GetUserByEmail(string email)
        {
            User user = await _userRepo.GetByEmail(email);
            if (user == null) throw new NullReferenceException("Not Found");
            return TransformViewModel(user);
        }

        private async Task<bool> CheckDuplicatedEmailAndUserId(int? universityId, string email, string userId)
        {
            bool isExisted = await _userRepo.CheckExistedEmail(email);
            if (isExisted) throw new ArgumentException("Duplicated Email");

            if (universityId != null)
            {
                isExisted = await _userRepo.CheckExistedUser((int)universityId, userId);
            }
            else
            {
                isExisted = await _userRepo.CheckExistedUser(userId);
            }

            if (isExisted) throw new ArgumentException("Duplicated UserId");

            return isExisted;
        }



        public async Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request)
        {
            PagingResult<User> users = await _userRepo.GetByCondition(request);
            if (users.Items == null) throw new NullReferenceException("Not Found");

            List<ViewUser> items = new List<ViewUser>();
            users.Items.ForEach(u =>
            {
                ViewUser user = TransformViewModel(u);
                items.Add(user);
            });

            return new PagingResult<ViewUser>(items, users.TotalCount, users.CurrentPage, users.PageSize);
        }

        public async Task<ViewUser> Insert(UserInsertModel user)
        {
            if (user == null) throw new ArgumentNullException("Null Argument");

            bool isInvalid = await CheckDuplicatedEmailAndUserId(user.UniversityId, user.Email, user.UserId);

            if (isInvalid) return null;

            // default status when insert is true
            bool status = true;
            User element = new User()
            {
                Description = user.Description,
                Dob = user.Dob,
                Email = user.Email,
                Fullname = user.Fullname,
                Gender = user.Gender,
                MajorId = user.MajorId,
                RoleId = user.RoleId,
                Status = status,
                UniversityId = user.UniversityId,
                UserId = user.UserId
            };

            int id = await _userRepo.Insert(element);
            if (id > 0)
            {
                element.Id = id;
                return TransformViewModel(element);
            }

            return null;
        }

        public async Task<bool> Update(ViewUser user)
        {
            if (user == null) throw new ArgumentNullException("Null Argument");

            User element = await _userRepo.Get(user.Id);
            if (element == null) throw new NullReferenceException("Not Found");

            bool isInvalid = await CheckDuplicatedEmailAndUserId(user.UniversityId, user.Email, user.UserId);
            if (isInvalid) return isInvalid;

            element.Description = user.Description;
            element.Dob = user.Dob;
            element.Email = user.Email;
            element.Fullname = user.Fullname;
            element.Gender = user.Gender;
            element.MajorId = user.MajorId;
            element.RoleId = user.RoleId;
            element.UserId = user.UserId;
            element.UniversityId = user.UniversityId;
            element.Status = user.Status;

            return await _userRepo.Update();

        }

        public async Task<bool> Delete(int id)
        {
            User user = await _userRepo.Get(id);
            if (user == null) throw new NullReferenceException("Not found");

            user.Status = false;
            return await _userRepo.Update();
        }



        //------------------------------------------------LOGIN------------------------------------------------

        //CheckUserEmail
        public async Task<bool> CheckUserEmailExsit(string email_user)
        {
            bool check = false;
            check = await _userRepo.CheckExistedEmail(email_user);
            return check;
        }

        //Insert - UserTemporary
        public async Task<ViewUser> InsertUserTemporary(UserModelTemporary userTem)
        {
            try
            {
                User user = new User
                {
                    RoleId = userTem.RoleId,
                    Email = userTem.Email,
                    Status = true
                };
                int id = await _userRepo.Insert(user);
                if (id > 0)
                {
                    return new ViewUser
                    {
                        Id = id,
                        RoleId = userTem.RoleId,
                        Email = userTem.Email,
                        Status = true
                    };
                }
                return null;
            }
            catch (Exception) { throw; }
        }
    }
}
