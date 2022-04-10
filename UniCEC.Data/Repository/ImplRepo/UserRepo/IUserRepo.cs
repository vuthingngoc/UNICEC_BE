using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public interface IUserRepo : IRepository<User>
    {
        public Task<User> GetUser(string userId);
        public Task<bool> CheckExistedEmail(string email);
        public Task<bool> CheckExistedUser(int universityId, string userId);
        public Task<bool> CheckExistedUser(string userId);
    }
}
