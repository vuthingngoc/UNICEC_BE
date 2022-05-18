using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public class SponsorInCompetitionRepo : Repository<SponsorInCompetition>, ISponsorInCompetitionRepo
    {
        public SponsorInCompetitionRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckDuplicateCreateCompetitionOrEvent(int sponsorId, int competitionId)
        {
            var query = from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId && sic.CompetitionId == competitionId
                        select sic;
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

        //
        public async Task<List<int>> GetListSponsorId_In_Competition(int CompetitionId)
        {
            List<int> sponsorIdList = await (from sic in context.SponsorInCompetitions
                                             where CompetitionId == sic.CompetitionId
                                             select sic.SponsorId).ToListAsync();

            if (sponsorIdList.Count > 0)
            {
                return sponsorIdList;
            }

            return null;
        }
    }
}
