using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.User;

namespace UNICS.Business.Services.UserSvc
{
    public interface IUserService
    {
        Task<PagingResult<ViewUser>> GetAll(PagingRequest request);
        Task<ViewUser> GetById(int id);
        Task<bool> Insert(UserInsertModel user);
        Task<bool> Update(ViewUser user);
        Task<bool> Delete(int id);
    }
}
