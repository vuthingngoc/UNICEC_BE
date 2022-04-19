using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public interface IUserService
    {
        public Task<PagingResult<ViewUser>> GetAllPaging(PagingRequest request);
        public Task<ViewUser> GetUserByUserId(string userId);
        public Task<ViewUser> GetUserByEmail(string email);
        public Task<PagingResult<ViewUser>> GetUserCondition(UserRequestModel request);
        public Task<ViewUser> Insert(UserInsertModel user);
        public Task<bool> Update(ViewUser user);
        public Task<bool> Delete(int id);

        //Check-User-Exist
        public Task<bool> CheckUserEmailExsit(string email_user);
        //Insert-User-Temporary
        public Task<ViewUser> InsertUserTemporary(UserModelTemporary userTem);
    }
}
