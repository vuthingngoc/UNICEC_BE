using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerInCompetitionRepo
{
    public interface IInfluencerInCompetitionRepo : IRepository<InfluencerInCompetition>
    {
        public Task<InfluencerInCompetition> GetInfluencerInCompetition(int influencerId, int competitionId);

        public Task<List<int>> GetListInfluencer_In_Competition_Id(int competitionId);

        public Task DeleteInfluencerInCompetition(int id);
    }
}
