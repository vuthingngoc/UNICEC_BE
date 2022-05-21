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
        public Task<ViewUser> GetUserByEmail(string email);
        public Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request);
        //public Task<ViewUser> Insert(UserInsertModel user);
        // Insert-User-Temporary
        public Task<int> InsertUserTemporary(UserModelTemporary userTem);
        public Task<bool> Update(UserUpdateModel user, string token);
        // firebase
        public Task UpdateAvatar(int userId, string srcAvatar);
        public Task UpdateInfoToken(int userId, int universityId, string token);
        public Task UpdateStatusOnline(int id, bool status);
        public Task<bool> Delete(int id);

        //Check-User-Exist
        public Task<bool> CheckUserEmailExsit(string email_user);
        
    }
}
