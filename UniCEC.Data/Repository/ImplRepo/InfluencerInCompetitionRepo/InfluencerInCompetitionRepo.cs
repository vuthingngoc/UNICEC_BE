using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerInCompetitionRepo
{
    public class InfluencerInCompetitionRepo : Repository<InfluencerInCompetition>, IInfluencerInCompetitionRepo
    {
        public InfluencerInCompetitionRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<InfluencerInCompetition> GetInfluencerInCompetition(int InfluencerId, int CompetitionId)
        {
            var query = await (from iic in context.InfluencerInCompetitions
                               where iic.CompetitionId == CompetitionId && iic.InfluencerId == InfluencerId
                               select iic).FirstOrDefaultAsync();

            if (query != null)
            {
                return query;
            }
            return null;
        }
    }
}
