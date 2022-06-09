using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Role;

namespace UniCEC.Data.Repository.ImplRepo.RoleRepo
{
    public interface IRoleRepo : IRepository<Role>
    {
        public Task<List<ViewRole>> GetAll();
        public Task<ViewRole> GetById(int id);
        public Task Delete(Role role);
    }
}
