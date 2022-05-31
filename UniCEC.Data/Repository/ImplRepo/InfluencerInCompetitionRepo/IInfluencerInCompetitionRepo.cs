using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerInCompetitionRepo
{
    public interface IInfluencerInCompetitionRepo : IRepository<InfluencerInCompetition>
    {
        public Task<InfluencerInCompetition> GetInfluencerInCompetition(int InfluencerId, int CompetitionId);
    }
}
