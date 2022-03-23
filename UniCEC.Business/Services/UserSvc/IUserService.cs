using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public interface IUserService
    {
        public Task<PagingResult<ViewUser>> GetAll(PagingRequest request);
        public Task<ViewUser> GetByUserId(int id);
        public Task<ViewUser> Insert(UserInsertModel user);
        public Task<bool> Update(ViewUser user);
        public Task<bool> Delete(int id);
    }
}
