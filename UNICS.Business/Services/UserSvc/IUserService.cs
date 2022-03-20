using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.User;

namespace UNICS.Business.Services.UserSvc
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
