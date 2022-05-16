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

        public async Task<ViewUser> GetById(int id)
        {
            var query = from u in context.Users
                        where u.Id.Equals(id)
                        select new ViewUser()
                        {
                            Id = u.Id,
                            RoleId = u.RoleId,
                            SponsorId = (u.SponsorId.HasValue) ? u.SponsorId.Value : 0,
                            UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                            MajorId = (u.MajorId.HasValue) ? u.MajorId.Value : 0,
                            Fullname = u.Fullname,
                            UserCode = u.UserCode,
                            Email = u.Email,
                            PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                            Gender = u.Gender,
                            Dob = u.Dob,
                            Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                            Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                            Status = u.Status,
                            IsOnline = u.IsOnline
                        };

            return await query.FirstOrDefaultAsync();
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
                                            SponsorId = (u.SponsorId.HasValue) ? u.SponsorId.Value : 0,
                                            UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                                            MajorId = (u.MajorId.HasValue) ? u.MajorId.Value : 0,
                                            Fullname = u.Fullname,
                                            UserCode = u.UserCode,
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

        //UserCode
        public async Task<ViewUser> GetByUserCode(string userCode)
        {
            var query = from u in context.Users
                        where u.UserCode.Equals(userCode)
                        select u;

            return await query.Select(u => new ViewUser()
            {
                Id = u.Id,
                RoleId = u.RoleId,
                SponsorId = (u.SponsorId.HasValue) ? u.SponsorId.Value : 0,
                UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                MajorId = (u.MajorId.HasValue) ? u.MajorId.Value : 0,
                Fullname = u.Fullname,
                UserCode = u.UserCode,
                Email = u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                Gender = u.Gender,
                Dob = u.Dob,
                Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                Status = u.Status,
                IsOnline = u.IsOnline
            }).FirstOrDefaultAsync();

            //return await context.Users.FirstOrDefaultAsync(u => u.UserCode.Equals(userCode));
        }

        public async Task<ViewUser> GetByEmail(string email)
        {
            var query = from u in context.Users
                        where u.Email.Equals(email)
                        select u;

            return await query.Select(u => new ViewUser()
            {
                Id = u.Id,
                RoleId = u.RoleId,
                SponsorId = (u.SponsorId.HasValue) ? u.SponsorId.Value : 0,
                UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                MajorId = (u.MajorId.HasValue) ? u.MajorId.Value : 0,
                Fullname = u.Fullname,
                UserCode = u.UserCode,
                Email = u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                Gender = u.Gender,
                Dob = u.Dob,
                Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                Status = u.Status,
                IsOnline = u.IsOnline
            }).FirstOrDefaultAsync();

            //return await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<PagingResult<ViewUser>> GetByCondition(UserRequestModel request)
        {
            var query = from u in context.Users
                        select u;

            if (request.UniversityId.HasValue) query = query.Where(u => u.UniversityId.Equals(request.UniversityId));

            if (request.MajorId.HasValue) query = query.Where(u => u.MajorId.Equals(request.MajorId));

            if (!string.IsNullOrEmpty(request.Fullname)) query = query.Where(u => u.Fullname.Equals(request.Fullname));

            if (request.Status.HasValue) query = query.Where(u => u.Status.Equals(request.Status));

            List<ViewUser> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                          .Select(u => new ViewUser()
                                          {
                                              Id = u.Id,
                                              RoleId = u.RoleId,
                                              SponsorId = (u.SponsorId.HasValue) ? u.SponsorId.Value : 0,
                                              UniversityId = (u.UniversityId.HasValue) ? u.UniversityId.Value : 0,
                                              MajorId = (u.MajorId.HasValue) ? u.MajorId.Value : 0,
                                              Fullname = u.Fullname,
                                              UserCode = u.UserCode,
                                              Email = u.Email,
                                              PhoneNumber = (!string.IsNullOrEmpty(u.PhoneNumber)) ? u.PhoneNumber : "",
                                              Gender = u.Gender,
                                              Dob = u.Dob,
                                              Description = (!string.IsNullOrEmpty(u.Description)) ? u.Description : "",
                                              Avatar = (!string.IsNullOrEmpty(u.Avatar)) ? u.Avatar : "",
                                              Status = u.Status,
                                              IsOnline = u.IsOnline
                                          }
                                          ).ToListAsync();

            return (items.Count() > 0) ? new PagingResult<ViewUser>(items, context.Users.Count(), request.CurrentPage, request.PageSize) : null;
        }

        public async Task<bool> CheckExistedEmail(string email)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return (user != null) ? true : false;
        }

        public async Task<bool> CheckExistedUser(int universityId, string userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UniversityId.Equals(universityId) && u.UserCode.Equals(userId));
            return (user != null) ? true : false;
        }

        public async Task<bool> CheckExistedUser(string userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UniversityId == null && u.UserCode.Equals(userId));
            return (user != null) ? true : false;
        }
    }
}
