using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public interface IUserService
    {
        public Task<PagingResult<ViewUser>> GetByUniversity(int universityId, UserStatus status, PagingRequest request);
        public Task<ViewUser> GetUserByUserCode(string userCode);
        public Task<ViewUser> GetUserByEmail(string email);
        public Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request);
        public Task<ViewUser> Insert(UserInsertModel user);
        public Task<bool> Update(ViewUser user);
        public Task UpdateStatusOnline(int id, bool status);
        public Task<bool> Delete(int id);

        //Check-User-Exist
        public Task<bool> CheckUserEmailExsit(string email_user);
        //Insert-User-Temporary
        public Task<ViewUser> InsertUserTemporary(UserModelTemporary userTem);
    }
}
