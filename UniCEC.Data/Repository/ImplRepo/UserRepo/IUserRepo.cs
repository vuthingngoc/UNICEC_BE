using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public interface IUserRepo : IRepository<User>
    {
        public Task<ViewUser> GetById(int id, bool isFullInfo);
        public Task<PagingResult<ViewUser>> GetUserByUniversity(int universityId, UserStatus status, PagingRequest request);
        public Task<ViewUser> GetByEmail(string email);
        public Task<PagingResult<ViewUser>> GetByCondition(UserRequestModel request);
        public Task<bool> CheckExistedEmail(string email);
        public Task<bool> CheckExistedUser(int universityId, string userId);
        public Task<bool> CheckExistedUser(string userId);
    }
}
