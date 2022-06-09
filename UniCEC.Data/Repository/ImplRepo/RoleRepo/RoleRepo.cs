using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Role;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.RoleRepo
{
    public class RoleRepo : Repository<Role>, IRoleRepo
    {
        public RoleRepo(UniCECContext context) : base(context)
        {

        }

        public async Task Delete(Role role)
        {
            context.Roles.Remove(role);
            await Update();
        }

        public async Task<List<ViewRole>> GetAll()
        {
            return await (from r in context.Roles
                          select new ViewRole()
                          {
                              Id = r.Id,
                              RoleName = r.RoleName,
                          }).ToListAsync();
        }

        public async Task<ViewRole> GetById(int id)
        {
            return await (from r in context.Roles
                          where r.Id.Equals(id)
                          select new ViewRole()
                          {
                              Id = r.Id,
                              RoleName = r.RoleName,
                          }).FirstOrDefaultAsync();
        }
    }
}
