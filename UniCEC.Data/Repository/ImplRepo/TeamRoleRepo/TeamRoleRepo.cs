using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamRoleRepo
{
    public class TeamRoleRepo : Repository<TeamRole>, ITeamRoleRepo
    {
        public TeamRoleRepo(UniCECContext context) : base(context)
        {
        }
    }
}
