using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.RoleRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Role;

namespace UNICS.Business.Services.RoleSvc
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

        public Task<ViewRole> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(RoleInsertModel role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewRole role)
        {
            throw new NotImplementedException();
        }
    }
}
