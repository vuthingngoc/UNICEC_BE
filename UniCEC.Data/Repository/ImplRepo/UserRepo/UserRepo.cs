using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<User> GetByUserId(string userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.UserCode.Equals(userId));
        }

        public async Task<User> GetByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<PagingResult<User>> GetByCondition(UserRequestModel request)
        {
            var query = from u in context.Users
                        select new { u };
            if (request.UniversityId != null) query = query.Where(x => x.u.UniversityId.Equals(request.UniversityId));

            if (request.MajorId != null) query = query.Where(x => x.u.MajorId.Equals(request.MajorId));

            if (!string.IsNullOrEmpty(request.Fullname)) query = query.Where(x => x.u.Fullname.Equals(request.Fullname));

            if (request.Status != null) query = query.Where(x => x.u.Status.Equals(request.Status));

            List<User> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                          .Select(x => new User()
                                              {
                                                  Id = x.u.Id,
                                                  Description = x.u.Description,
                                                  Dob = x.u.Dob,
                                                  Email = x.u.Email,
                                                  Fullname = x.u.Fullname,
                                                  Gender = x.u.Gender,
                                                  MajorId = x.u.MajorId,
                                                  RoleId = x.u.RoleId,
                                                  UniversityId = x.u.UniversityId,
                                                  UserCode = x.u.UserCode,
                                                  Status = x.u.Status
                                              }
                                          ).ToListAsync();

            return new PagingResult<User>(items, context.Users.Count(), request.CurrentPage, request.PageSize);

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
