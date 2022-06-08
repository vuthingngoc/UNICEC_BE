using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.Data.Repository.ImplRepo.ClubRoleRepo
{
    public class ClubRoleRepo : Repository<ClubRole>, IClubRoleRepo
    {
        public ClubRoleRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckExistedClubRole(int id)
        {
            return await (from cr in context.ClubRoles
                          where cr.Id.Equals(id)
                          select cr).FirstOrDefaultAsync() != null;
        }

        public async Task Delete(ClubRole clubRole)
        {
            context.ClubRoles.Remove(clubRole);
            await Update();
        }

        public async Task<List<ViewClubRole>> GetAll()
        {
            return await (from cr in context.ClubRoles
                          select new ViewClubRole()
                          {
                              Id = cr.Id,
                              Name = cr.Name
                          }).ToListAsync();
        }

        public async Task<ViewClubRole> GetById(int id)
        {
            return await (from cr in context.ClubRoles
                          where cr.Id.Equals(id)
                          select new ViewClubRole()
                          {
                              Id = cr.Id,
                              Name = cr.Name
                          }).FirstOrDefaultAsync();
        }
    }
}
