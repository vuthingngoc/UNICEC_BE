using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public interface IUserService
    {
        public int DecodeToken(string token, string nameClaim);
        public Task<ViewUser> GetById(string token, int id);
        public Task<PagingResult<ViewUser>> GetByUniversity(int universityId, UserStatus status, PagingRequest request);
        public Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request);
        public Task<bool> Insert(string token, UserAccountInsertModel user);
        public Task<bool> Delete(int id);
        public Task<bool> CheckUserEmailExsit(string email);
        // Login
        public Task<int> InsertNewUser(UserTokenModel userModel, string email, string phoneNumber);
        public Task<string> LoginAccount(UserLoginModel model);
        public Task<bool> Update(UserUpdateModel user, string token);
        // firebase
        public Task<UserTokenModel> GetUserByEmail(string email);
        public Task<UserTokenModel> GetUserTokenById(int id, string token);
        public Task UpdateAvatar(int userId, string srcAvatar);
        public Task UpdateInfoToken(int userId, int universityId, string token);
        public Task UpdateStatusOnline(int id, bool status);           
    }
}
