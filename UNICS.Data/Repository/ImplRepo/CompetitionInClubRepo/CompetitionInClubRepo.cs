using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CompetitionInClubRepo 
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo {
        public CompetitionInClubRepo(UNICSContext context) : base(context) {

        }
    }
}
