using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo
{
    public class TeamInMatchRepo : Repository<TeamInMatch>, ITeamInMatchRepo
    {
        public TeamInMatchRepo(UniCECContext context) : base(context)
        {
        }
    }
}
