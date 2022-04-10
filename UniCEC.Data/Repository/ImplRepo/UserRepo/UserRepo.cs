using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<User> GetUser(string userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId));            
        }

        public async Task<bool> CheckExistedEmail(string email)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            return (user != null) ? true : false;            
        }

        public async Task<bool> CheckExistedUser(int universityId, string userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UniversityId.Equals(universityId) && u.UserId.Equals(userId));
            return (user != null) ? true : false;            
        }

        public async Task<bool> CheckExistedUser(string userId)
        {
            User user = await context.Users.FirstOrDefaultAsync(u => u.UniversityId == null && u.UserId.Equals(userId));
            return (user != null) ? true : false;
        }
    }
}
