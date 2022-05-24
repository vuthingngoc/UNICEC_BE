using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.TeamRoleRepo
{
    public class TeamRoleRepo : Repository<TeamRole>, ITeamRoleRepo
    {
        public TeamRoleRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<int> GetRoleIdByName(string roleName)
        {
            var query = from tr in context.TeamRoles
                        where tr.Name == roleName
                        select tr.Id;
            int roleId = await query.SingleOrDefaultAsync();
            if (roleId != 0)
            {
                return roleId;
            }
            else
            {
                return 0;
            }
        }
    }
}
