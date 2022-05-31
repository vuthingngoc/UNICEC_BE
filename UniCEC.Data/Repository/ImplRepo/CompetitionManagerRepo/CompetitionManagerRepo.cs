using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo
{
    public class CompetitionManagerRepo : Repository<CompetitionManager>, ICompetitionManagerRepo
    {
        public CompetitionManagerRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<CompetitionManager> GetCompetitionManager(int CompetitionId, int ClubId, int MemberId)
        {
            var query = from cic in context.CompetitionInClubs
                        where cic.CompetitionId == CompetitionId && cic.ClubId == ClubId
                        from cm in context.CompetitionManagers
                        where cm.CompetitionInClubId == cic.Id
                        where cm.MemberId == MemberId
                        select cm;

            CompetitionManager competitionManager = await query.FirstOrDefaultAsync();
            if (competitionManager != null)
            {
                return competitionManager;
            }
            return null;
        }
    }
}
