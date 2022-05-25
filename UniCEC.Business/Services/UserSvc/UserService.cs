using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
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
        private JwtSecurityTokenHandler _tokenHandler;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public async Task<ViewUser> GetById(string token, int id)
        {
            int userId = DecodeToken(token, "Id");
            int roleId = DecodeToken(token, "RoleId");
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

        //public async Task<ViewUser> Insert(UserInsertModel user)
        //{
        //    if (string.IsNullOrEmpty(user.Description) || string.IsNullOrEmpty(user.Dob) || string.IsNullOrEmpty(user.Fullname)
        //        || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Gender) || user.RoleId == 0 || string.IsNullOrEmpty(user.UserCode))
        //        throw new ArgumentNullException("Description Null || Dob Null || Fullname Null || Email Null || Gender Null || RoleId || UserId Null");

        //    bool isInvalid = await CheckDuplicatedEmailAndUserCode(user.UniversityId, user.Email, user.UserCode);

        //    if (isInvalid) return null;

        //    // default status when insert is true
        //    UserStatus status = UserStatus.Active;
        //    User element = new User()
        //    {
        //        Description = user.Description,
        //        Dob = user.Dob,
        //        Email = user.Email,
        //        Fullname = user.Fullname,
        //        Gender = user.Gender,
        //        MajorId = user.MajorId,
        //        RoleId = user.RoleId,
        //        Status = status,
        //        UniversityId = user.UniversityId,
        //        UserCode = user.UserCode,
        //        Avatar = user.Avatar,
        //        IsOnline = true // default status when log in
        //    };

        //    int id = await _userRepo.Insert(element);
        //    if (id > 0)
        //    {
        //        element.Id = id;
        //        return TransformViewModel(element);
        //    }

        //    return null;
        //}

        public async Task<bool> Update(UserUpdateModel model, string token)
        {
            int userId = DecodeToken(token, "Id");
            int roleId = DecodeToken(token, "RoleId");

            if (!userId.Equals(model.Id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            User user = await _userRepo.Get(model.Id);
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
            if (model.MajorId.HasValue) user.MajorId = model.MajorId;

            // for admin
            if (model.RoleId != 0 && roleId.Equals(1)) user.RoleId = model.RoleId.Value;
            if (model.Status.HasValue && roleId.Equals(1)) user.Status = model.Status.Value;

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
                User user = new User
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
            int idUser = DecodeToken(token, "Id");
            if (!userId.Equals(idUser)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            User user = await _userRepo.Get(userId);
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
            User user = await _userRepo.Get(userId);
            if (user == null) throw new NullReferenceException("Not found this user");

            user.Avatar = srcAvatar;
            await _userRepo.Update();
        }

        public async Task<UserTokenModel> GetUserTokenById(int id, string token)
        {
            int userId = DecodeToken(token, "Id");
            if (!userId.Equals(id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            return await _userRepo.GetUserTokenById(id);
        }
    }
}
