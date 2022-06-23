using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Role;

namespace UniCEC.Business.Services.RoleSvc
{
    public interface IRoleService
    {
        public Task<List<ViewRole>> GetAll();
        public Task<ViewRole> GetByRoleId(int id);
        public Task<ViewRole> Insert(string roleName);
        public Task Update(ViewRole role);
        //public Task Delete(int id);
    }
}
