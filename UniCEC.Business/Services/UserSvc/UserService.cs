using System;
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
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
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
            if (users != null)
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
            if (user != null)
            {
                return TransformViewModel(user);

            }
            else
            {
                return null;
            }

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
            if (string.IsNullOrEmpty(user.Description) || string.IsNullOrEmpty(user.Dob) || string.IsNullOrEmpty(user.Fullname)
                || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Gender) || user.RoleId == 0 || string.IsNullOrEmpty(user.UserId))
                    throw new ArgumentNullException("Description Null || Dob Null || Fullname Null || Email Null || Gender Null || RoleId || UserId Null");

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
                UserId = user.UserId,
                Avatar = user.Avatar,
                IsOnline = true // default status when log in
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
            User element = await _userRepo.Get(user.Id);
            if (element == null) throw new NullReferenceException("Not found this user");

            //bool isInvalid = await CheckDuplicatedEmailAndUserId(user.UniversityId, user.Email, user.UserId);
            //if (isInvalid) return isInvalid;

            if (!string.IsNullOrEmpty(user.Description)) element.Description = user.Description;
            if (!string.IsNullOrEmpty(user.Dob))  element.Dob = user.Dob;
            if (!string.IsNullOrEmpty(user.Email)) element.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Fullname)) element.Fullname = user.Fullname;
            if (!string.IsNullOrEmpty(user.Gender)) element.Gender = user.Gender;
            if(user.MajorId != 0) element.MajorId = user.MajorId;
            if(user.RoleId != 0) element.RoleId = user.RoleId;
            if (!string.IsNullOrEmpty(user.UserId)) element.UserId = user.UserId;
            if(user.UniversityId != 0) element.UniversityId = user.UniversityId;
            element.Status = user.Status;
            if (!string.IsNullOrEmpty(user.Avatar)) element.Avatar = user.Avatar;

            await _userRepo.Update();
            return true;
        }

        public async Task UpdateStatusOnline(int id, bool status)
        {
            User element = await _userRepo.Get(id);
            if (element == null) throw new NullReferenceException("Not found this user");

            element.IsOnline = status;
            await _userRepo.Update();
        }

        public async Task<bool> Delete(int id)
        {
            User user = await _userRepo.Get(id);
            if (user == null) throw new NullReferenceException("Not found");

            user.Status = false;
            await _userRepo.Update();
            return true;
        }



        //------------------------------------------------LOGIN------------------------------------------------

        //CheckUserEmail
        public async Task<bool> CheckUserEmailExsit(string email_user)
        {
            bool check = false;
            User user = await _userRepo.GetByEmail(email_user);
            if (user != null)
            {
                check = true;
                return check;
            }
            else
            {
                return check;
            }

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
                    Status = true,
                    Avatar = userTem.Avatar,
                    //auto
                    Dob = "",
                    Fullname = userTem.Fullname,
                    Gender = "",
                    UserId = "",
                    Description = "",
                    IsOnline = true // default status when log in
                };
                int id = await _userRepo.Insert(user);
                if (id > 0)
                {
                    return new ViewUser
                    {
                        Id = id,
                        RoleId = userTem.RoleId,
                        Email = userTem.Email,
                        Avatar = userTem.Avatar,
                        Status = true
                    };
                }
                return null;
            }
            catch (Exception) { throw; }
        }
    }
}
