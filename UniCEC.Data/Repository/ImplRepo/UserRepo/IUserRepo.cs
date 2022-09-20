using System.Collections.Generic;
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
        public Task<PagingResult<ViewUser>> GetByCondition(UserRequestModel request);
        public Task<bool> CheckExistedEmail(string email, int? userId);
        public Task<bool> CheckExistedUser(int universityId, string studentCode, int userId);
        public Task<bool> CheckExistedUser(string studentCode, int userId);
        public Task<bool> CheckExistedUser(int universityId, int userId);
        public Task UpdateStatusByUniversityId(int universityId, UserStatus status);
        // firebase
        public Task<UserTokenModel> GetByEmail(string email);
        public Task<UserTokenModel> GetUserTokenById(int id);
        // notification
        public Task<string> GetDeviceTokenByUser(int userId);
        public Task<List<string>> GetDeviceTokenByUsers(List<int> userIds);
        public Task<string> GetDeviceTokenByMember(int memberId);
        public Task<List<string>> GetDeviceTokenByMembers(List<int> memberIds);
        public Task RemoveInactiveDeviceToken(string deviceToken);
    }
}
