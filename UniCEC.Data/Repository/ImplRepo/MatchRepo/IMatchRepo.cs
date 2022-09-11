using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;

namespace UniCEC.Data.Repository.ImplRepo.MatchRepo
{
    public interface IMatchRepo : IRepository<Match>
    {
        public Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request);
        public Task<ViewMatch> GetById(int id);
        public Task<bool> CheckDuplicatedMatch(string title, int roundId);
        public Task<int> GetCompetitionIdByMatch(int matchId);
        public Task<bool> CheckAvailableMatchId(int matchId);
        public Task UpdateStatusMatchesByRound(int roundId, MatchStatus status);
        public Task UpdateStatusMatchesByComp(int competitionId, MatchStatus status);
    }
}
