using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.TeamRepo
{
    public class TeamRepo : Repository<Team>, ITeamRepo
    {
        public TeamRepo(UNICSContext context) : base(context)
        {

        }
    }
}
