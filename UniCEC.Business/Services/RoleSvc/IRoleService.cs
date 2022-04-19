using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Role;

namespace UniCEC.Business.Services.RoleSvc
{
    public interface IRoleService
    {
        public Task<PagingResult<ViewRole>> GetAllPaging(PagingRequest request);
        public Task<ViewRole> GetByRoleId(int id);
        public Task<ViewRole> Insert(RoleInsertModel role);
        public Task<bool> Update(ViewRole role);
        public Task<bool> Delete(int id);
    }
}
