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

        public async Task<PagingResult<ViewUser>> GetUserByUniversity(int universityId, UserStatus status, PagingRequest request)
        {
            var query = from u in context.Users
                        where u.UniversityId.Equals(universityId) && u.Status.Equals(status)
                        select new { u };

            int totalCount = query.Count();

            List<ViewUser> users = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                        .Select(x => new ViewUser()
                                        {
                                            Id = x.u.Id,
                                            RoleId = x.u.RoleId,
                                            SponsorId = x.u.SponsorId.Value,
                                            UniversityId = x.u.UniversityId,
                                            MajorId = x.u.MajorId,
                                            Fullname = x.u.Fullname,
                                            UserCode = x.u.UserCode,
                                            Email = x.u.Email,
                                            PhoneNumber = x.u.PhoneNumber,
                                            Gender = x.u.Gender,
                                            Dob = x.u.Dob,
                                            Description = x.u.Description,
                                            Avatar = x.u.Avatar,
                                            Status = x.u.Status,
                                            IsOnline = x.u.IsOnline
                                        }).ToListAsync();

            return (totalCount > 0) ? new PagingResult<ViewUser>(users, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //UserCode
        public async Task<ViewUser> GetByUserCode(string userCode)
        {
            var query = from u in context.Users
                        where u.UserCode.Equals(userCode)
                        select new { u };

            return await query.Select(x => new ViewUser()
            {
                Id = x.u.Id,
                RoleId = x.u.RoleId,
                SponsorId = (x.u.SponsorId.HasValue) ? x.u.SponsorId.Value : 0,
                UniversityId = (x.u.UniversityId.HasValue) ? x.u.UniversityId.Value : 0,
                MajorId = (x.u.MajorId.HasValue) ? x.u.MajorId.Value : 0,
                Fullname = x.u.Fullname,
                UserCode = x.u.UserCode,
                Email = x.u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(x.u.PhoneNumber)) ? x.u.PhoneNumber : "",
                Gender = x.u.Gender,
                Dob = x.u.Dob,
                Description = (!string.IsNullOrEmpty(x.u.Description)) ? x.u.Description : "",
                Avatar = (!string.IsNullOrEmpty(x.u.Avatar)) ? x.u.Avatar : "",
                Status = x.u.Status,
                IsOnline = x.u.IsOnline
            }).FirstOrDefaultAsync();

            //return await context.Users.FirstOrDefaultAsync(u => u.UserCode.Equals(userCode));
        }

        public async Task<ViewUser> GetByEmail(string email)
        {
            var query = from u in context.Users
                        where u.Email.Equals(email)
                        select new { u };

            return await query.Select(x => new ViewUser()
            {
                Id = x.u.Id,
                RoleId = x.u.RoleId,
                SponsorId = (x.u.SponsorId.HasValue) ? x.u.SponsorId.Value : 0,
                UniversityId = (x.u.UniversityId.HasValue) ? x.u.UniversityId.Value : 0,
                MajorId = (x.u.MajorId.HasValue) ? x.u.MajorId.Value : 0,
                Fullname = x.u.Fullname,
                UserCode = x.u.UserCode,
                Email = x.u.Email,
                PhoneNumber = (!string.IsNullOrEmpty(x.u.PhoneNumber)) ? x.u.PhoneNumber : "",
                Gender = x.u.Gender,
                Dob = x.u.Dob,
                Description = (!string.IsNullOrEmpty(x.u.Description)) ? x.u.Description : "",
                Avatar = (!string.IsNullOrEmpty(x.u.Avatar)) ? x.u.Avatar : "",
                Status = x.u.Status,
                IsOnline = x.u.IsOnline
            }).FirstOrDefaultAsync();

            //return await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<PagingResult<ViewUser>> GetByCondition(UserRequestModel request)
        {
            var query = from u in context.Users
                        select new { u };

            if (request.UniversityId.HasValue) query = query.Where(x => x.u.UniversityId.Equals(request.UniversityId));

            if (request.MajorId.HasValue) query = query.Where(x => x.u.MajorId.Equals(request.MajorId));

            if (!string.IsNullOrEmpty(request.Fullname)) query = query.Where(x => x.u.Fullname.Equals(request.Fullname));

            if (request.Status.HasValue) query = query.Where(x => x.u.Status.Equals(request.Status));

            List<ViewUser> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                          .Select(x => new ViewUser()
                                          {
                                              Id = x.u.Id,
                                              RoleId = x.u.RoleId,
                                              SponsorId = (x.u.SponsorId.HasValue) ? x.u.SponsorId.Value : 0,
                                              UniversityId = (x.u.UniversityId.HasValue) ? x.u.UniversityId.Value : 0,
                                              MajorId = (x.u.MajorId.HasValue) ? x.u.MajorId.Value : 0,
                                              Fullname = x.u.Fullname,
                                              UserCode = x.u.UserCode,
                                              Email = x.u.Email,
                                              PhoneNumber = (!string.IsNullOrEmpty(x.u.PhoneNumber)) ? x.u.PhoneNumber : "",
                                              Gender = x.u.Gender,
                                              Dob = x.u.Dob,
                                              Description = (!string.IsNullOrEmpty(x.u.Description)) ? x.u.Description : "",
                                              Avatar = (!string.IsNullOrEmpty(x.u.Avatar)) ? x.u.Avatar : "",
                                              Status = x.u.Status,
                                              IsOnline = x.u.IsOnline
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
