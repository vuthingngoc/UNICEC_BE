using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Role;

namespace UNICS.Business.Services.RoleSvc
{
    public interface IRoleService
    {
        public Task<PagingResult<ViewRole>> GetAll(PagingRequest request);
        public Task<ViewRole> GetByRoleId(int id);
        public Task<ViewRole> Insert(RoleInsertModel role);
        public Task<bool> Update(ViewRole role);
        public Task<bool> Delete(int id);
    }
}
