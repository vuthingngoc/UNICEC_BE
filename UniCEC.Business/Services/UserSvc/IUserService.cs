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
        public Task<PagingResult<ViewUser>> GetUserCondition(string token, UserRequestModel request);
        public Task<bool> Insert(string token, UserAccountInsertModel user);
        public Task<bool> Delete(string token, int id);
        public Task<bool> CheckUserEmailExsit(string email);
        // notification
        public Task<string> GetDeviceTokenByUser(int userId);
        // Login
        public Task<int> InsertNewUser(UserTokenModel userModel, string email, string phoneNumber, string deviceToken);
        public Task<string> LoginAccount(UserLoginModel model);
        public Task<bool> Update(UserUpdateModel user, string token);
        public Task UpdateStatusUser(int id, UserStatus status, string token);
        // firebase
        public Task<UserTokenModel> GetUserByEmail(string email);
        public Task<UserTokenModel> GetUserTokenById(int id, string token);
        public Task UpdateAvatar(int userId, string srcAvatar);
        public Task UpdateInfoToken(UserUpdateWithJWTModel model, string token);
        public Task UpdateStatusOnline(int id, bool status);
        public Task UpdateInfoUserLogin(int userId, string srcAvatar, bool status, string deviceToken);
    }
}
