using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
    }
}
