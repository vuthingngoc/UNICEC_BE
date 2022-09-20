using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.User;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<ViewUser> GetById(int id, bool isFullInfo)
        {
            var query = from u in context.Users
                        join uni in context.Universities on u.UniversityId equals uni.Id into uniJoin
                        from uni in uniJoin.DefaultIfEmpty()
                        join d in context.Departments on u.DepartmentId equals d.Id into depJoin
                        from d in depJoin.DefaultIfEmpty()
                        where u.Id.Equals(id)
                        select new { u, uni, d };

            return (isFullInfo) ? await query.Select(selector => new ViewUser()
            {
                Id = selector.u.Id,
                RoleId = selector.u.RoleId,
                UniversityId = (selector.uni != null) ? selector.uni.Id : 0,
                UniversityName = (selector.uni != null) ? selector.uni.Name : "",
                DepartmentId = (selector.d != null) ? selector.d.Id : 0,
                DepartmentName = (selector.d != null) ? selector.d.Name : "",
                Fullname = selector.u.Fullname,
                StudentCode = selector.u.StudentCode,
                Email = selector.u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(selector.u.PhoneNumber)) ? selector.u.PhoneNumber : "",
                Gender = selector.u.Gender,
                Dob = selector.u.Dob,
                Description = (!string.IsNullOrEmpty(selector.u.Description)) ? selector.u.Description : "",
                Avatar = (!string.IsNullOrEmpty(selector.u.Avatar)) ? selector.u.Avatar : "",
                Status = selector.u.Status,
                IsOnline = selector.u.IsOnline
            }).FirstOrDefaultAsync()
                : await query.Select(selector => new ViewUser()
                {
                    Id = selector.u.Id,
                    RoleId = selector.u.RoleId,
                    UniversityId = (selector.uni != null) ? selector.uni.Id : 0,
                    UniversityName = (selector.uni != null) ? selector.uni.Name : "",
                    DepartmentId = (selector.d != null) ? selector.d.Id : 0,
                    DepartmentName = (selector.d != null) ? selector.d.Name : "",
                    Email = selector.u.Email,
                    Fullname = selector.u.Fullname,
                    Gender = selector.u.Gender,
                    Avatar = (!string.IsNullOrEmpty(selector.u.Avatar)) ? selector.u.Avatar : "",
                    Description = (!string.IsNullOrEmpty(selector.u.Description)) ? selector.u.Description : "",
                }).FirstOrDefaultAsync();
        }

        public async Task<PagingResult<ViewUser>> GetUserByUniversity(int universityId, UserStatus status, PagingRequest request)
        {
            var query = from u in context.Users
                        where u.UniversityId.Equals(universityId) && u.Status.Equals(status)
                        select u;

            int totalCount = query.Count();

            List<ViewUser> users = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                        .Select(u => new ViewUser()
                                        {
                                            Id = u.Id,
                                            RoleId = u.RoleId,
                                            UniversityId = universityId,
                                            DepartmentId = (u.DepartmentId.HasValue) ? u.DepartmentId.Value : 0,
                                            Fullname = u.Fullname,
                                            StudentCode = u.StudentCode,
                                            Email = u.Email,
                                            PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                                            Gender = u.Gender,
                                            Dob = u.Dob,
                                            Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                                            Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                                            Status = u.Status,
                                            IsOnline = u.IsOnline
                                        }).ToListAsync();

            return (totalCount > 0) ? new PagingResult<ViewUser>(users, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //StudentCode
        public async Task<ViewUser> GetByStudentCode(string studentCode)
        {
            var query = from u in context.Users
                        where u.StudentCode.Equals(studentCode)
                        select u;

            return await query.Select(u => new ViewUser()
            {
                Id = u.Id,
                RoleId = u.RoleId,
                UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                DepartmentId = (u.DepartmentId.HasValue) ? u.DepartmentId.Value : 0,
                Fullname = u.Fullname,
                StudentCode = u.StudentCode,
                Email = u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                Gender = u.Gender,
                Dob = u.Dob,
                Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                Status = u.Status,
                IsOnline = u.IsOnline
            }).FirstOrDefaultAsync();
        }

        public async Task<UserTokenModel> GetByEmail(string email)
        {
            var query = from u in context.Users
                        join r in context.Roles on u.RoleId equals r.Id
                        where u.Email.Equals(email)
                        select new { u, r };

            return await query.Select(selector => new UserTokenModel()
            {
                Id = selector.u.Id,
                RoleId = selector.u.RoleId,
                UniversityId = (selector.u.UniversityId.HasValue) ? selector.u.UniversityId.Value : 0,
                RoleName = selector.r.RoleName,
                Fullname = selector.u.Fullname,
                Avatar = (!string.IsNullOrEmpty(selector.u.Avatar)) ? selector.u.Avatar : "",
                Status = selector.u.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<UserTokenModel> GetUserTokenById(int id)
        {
            var query = from u in context.Users
                        join r in context.Roles on u.RoleId equals r.Id
                        where u.Id.Equals(id)
                        select new { u, r };

            return await query.Select(selector => new UserTokenModel()
            {
                Id = selector.u.Id,
                RoleId = selector.u.RoleId,
                UniversityId = (selector.u.UniversityId.HasValue) ? selector.u.UniversityId.Value : 0,
                RoleName = selector.r.RoleName,
                Fullname = selector.u.Fullname,
                Avatar = (!string.IsNullOrEmpty(selector.u.Avatar)) ? selector.u.Avatar : "",
                Status = selector.u.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<PagingResult<ViewUser>> GetByCondition(UserRequestModel request)
        {
            var query = from u in context.Users
                        join uni in context.Universities on u.UniversityId equals uni.Id
                        join d in context.Departments on u.DepartmentId equals d.Id
                        select new { u, uni, d };

            if (request.UniversityId.HasValue) query = query.Where(selector => selector.u.UniversityId.Equals(request.UniversityId));

            if (request.DepartmentId.HasValue) query = query.Where(selector => selector.u.DepartmentId.Equals(request.DepartmentId));

            if (!string.IsNullOrEmpty(request.SearchString))
                query = query.Where(selector => selector.u.Fullname.ToLower().Contains(request.SearchString.ToLower())
                                                    || selector.u.Email.ToLower().Contains(request.SearchString.ToLower()));

            if (request.Status.HasValue) query = query.Where(selector => selector.u.Status.Equals(request.Status));

            int totalCount = query.Count();

            List<ViewUser> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                          .Select(selector => new ViewUser()
                                          {
                                              Id = selector.u.Id,
                                              RoleId = selector.u.RoleId,
                                              UniversityId = (selector.u.UniversityId.HasValue) ? selector.u.UniversityId.Value : 0,
                                              UniversityName = (selector.u.UniversityId.HasValue) ? selector.uni.Name : "",
                                              DepartmentId = (selector.u.DepartmentId.HasValue) ? selector.u.DepartmentId.Value : 0,
                                              DepartmentName = (selector.u.DepartmentId.HasValue) ? selector.d.Name : "",
                                              Fullname = selector.u.Fullname,
                                              StudentCode = selector.u.StudentCode,
                                              Email = selector.u.Email,
                                              PhoneNumber = (!string.IsNullOrEmpty(selector.u.PhoneNumber)) ? selector.u.PhoneNumber : "",
                                              Gender = selector.u.Gender,
                                              Dob = selector.u.Dob,
                                              Description = (!string.IsNullOrEmpty(selector.u.Description)) ? selector.u.Description : "",
                                              Avatar = (!string.IsNullOrEmpty(selector.u.Avatar)) ? selector.u.Avatar : "",
                                              Status = selector.u.Status,
                                              IsOnline = selector.u.IsOnline
                                          }
                                          ).ToListAsync();

            return (items.Count() > 0) ? new PagingResult<ViewUser>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<bool> CheckExistedEmail(string email, int? userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && userId != null && !u.Id.Equals(userId));
            return (user != null) ? true : false;
        }

        public async Task<bool> CheckExistedUser(int universityId, string studentCode, int userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UniversityId.Equals(universityId) && !u.Id.Equals(userId) && u.StudentCode.Equals(studentCode) && u.Status.Equals(UserStatus.Active));
            return (user != null) ? true : false;
        }

        public async Task<bool> CheckExistedUser(string studentCode, int userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.StudentCode.Equals(studentCode) && !u.Id.Equals(userId) && u.Status.Equals(UserStatus.Active));
            return (user != null) ? true : false;
        }

        public async Task<bool> CheckExistedUser(int universityId, int userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => !u.Id.Equals(userId) && u.UniversityId.Equals(universityId) && u.Status.Equals(UserStatus.Active));
            return (user != null) ? true : false;
        }

        public async Task UpdateStatusByUniversityId(int universityId, UserStatus status)
        {
            List<User> users = await (from u in context.Users
                                      where u.UniversityId.Equals(universityId)
                                      select u).ToListAsync();

            if (users.Count > 0)
            {
                foreach (User user in users)
                {
                    user.Status = status;
                }
                await Update();
            }
        }

        public async Task<string> GetDeviceTokenByUser(int userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(user => user.Id.Equals(userId));
            return (user != null) ? user.DeviceToken : null;
        }

        public async Task<string> GetDeviceTokenByMember(int memberId)
        {
            return await (from m in context.Members
                          join u in context.Users on m.UserId equals u.Id
                          where m.Id.Equals(memberId)
                          select u.DeviceToken).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetDeviceTokenByUsers(List<int> userIds)
        {
            var query = from u in context.Users
                        where userIds.Contains(u.Id)
                        select u.DeviceToken;

            return (query.Any()) ? await query.ToListAsync() : null;
        }

        public async Task<List<string>> GetDeviceTokenByMembers(List<int> memberIds)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        where memberIds.Contains(m.Id)
                        select u.DeviceToken;

            return (query.Any()) ? await query.ToListAsync() : null;
        }

        public async Task RemoveInactiveDeviceToken(string deviceToken)
        {
            (from u in context.Users
             where u.DeviceToken.Equals(deviceToken)
             select u).ToList().ForEach(u => u.DeviceToken = null);

            await Update();
        }
    }
}
