using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo
    {
        public CompetitionInClubRepo(UniCECContext context) : base(context)
        {

        }
    }
}
