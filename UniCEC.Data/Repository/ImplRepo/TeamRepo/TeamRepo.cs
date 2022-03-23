using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamRepo
{
    public class TeamRepo : Repository<Team>, ITeamRepo
    {
        public TeamRepo(UniCECContext context) : base(context)
        {

        }
    }
}
