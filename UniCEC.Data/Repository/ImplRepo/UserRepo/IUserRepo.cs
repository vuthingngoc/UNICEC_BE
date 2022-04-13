using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public interface IUserRepo : IRepository<User>
    {
        public Task<User> GetByUserId(string userId);
        public Task<User> GetByEmail(string email);
        public Task<PagingResult<User>> GetByCondition(UserRequestModel request);
        public Task<bool> CheckExistedEmail(string email);
        public Task<bool> CheckExistedUser(int universityId, string userId);
        public Task<bool> CheckExistedUser(string userId);
    }
}
