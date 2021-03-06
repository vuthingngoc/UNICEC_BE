using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Business.Services.TeamInRoundSvc
{
    public interface ITeamInRoundService
    {
        //public Task<ViewTeamInRound> GetById(string token, int id);
        public Task<PagingResult<ViewTeamInRound>> GetByConditions(string token, TeamInRoundRequestModel request);
        public Task<List<ViewTeamInRound>> GetTopTeamsInCompetition(string token, int competitionId, int top);
        public Task<List<ViewTeamInRound>> Insert(string token, List<TeamInRoundInsertModel> models);
        public Task Update(string token, TeamInRoundUpdateModel model);
        public Task Delete(string token, int id);
    }
}
