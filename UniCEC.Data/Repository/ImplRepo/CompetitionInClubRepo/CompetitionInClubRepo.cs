using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public class CompetitionInClubRepo : Repository<CompetitionInClub>, ICompetitionInClubRepo
    {
        public CompetitionInClubRepo(UniCECContext context) : base(context)
        {

        }

        //
        public async Task<bool> CheckDuplicateCreateCompetition(int clubId, int competitionId)
        {
            var query = from cic in context.CompetitionInClubs
                        where cic.ClubId == clubId && cic.CompetitionId == competitionId
                        select cic;
            int check = query.Count();
            if (check > 0)
            {
                //có nghĩa là đã tạo nó r
                return false;
            }
            else
            {
                //có nghĩa là chưa 
                return true;
            }
        }
    }
}
