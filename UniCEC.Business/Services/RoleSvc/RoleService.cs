using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Role;

namespace UniCEC.Business.Services.RoleSvc
{
    public class RoleService : IRoleService
    {
        private IRoleRepo _roleRepo;

        public RoleService(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewRole>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewRole> GetByRoleId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewRole> Insert(RoleInsertModel role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewRole role)
        {
            throw new NotImplementedException();
        }
    }
}
