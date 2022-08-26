using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo
{
    public interface ITeamInRoundRepo : IRepository<TeamInRound>
    {
        public Task<PagingResult<ViewTeamInRound>> GetByConditions(TeamInRoundRequestModel request);
        public Task<ViewTeamInRound> GetById(int id, bool? status);
        public Task<List<ViewTeamInRound>> GetTopTeamsInCompetition(int competitionId, int top);
        public Task UpdateRankTeamsInRound(int roundId);
        public Task<List<ViewTeamInRound>> GetViewTeams(List<int> teamIds);
    }
}
