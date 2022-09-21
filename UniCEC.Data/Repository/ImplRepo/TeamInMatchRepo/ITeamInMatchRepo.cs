using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo
{
    public interface ITeamInMatchRepo : IRepository<TeamInMatch>
    {
        public Task<PagingResult<ViewTeamInMatch>> GetByConditions(TeamInMatchRequestModel request);
        public Task<ViewTeamInMatch> GetById(int id);
        public Task<int> GetRoundIdByMatch(int matchId);
        public Task<bool> CheckIsLoseMatch(int matchId); 
        public Task Delete(TeamInMatch teamInMatch);
        public bool CheckDuplicatedTeamInMatch(int matchId, int teamId, int? teamInMatchId);
        public Task InsertMultiTeams(List<TeamInMatch> teams);
        public void UpdateStatusTeams(int matchId, TeamInMatchStatus status); // update status follow match
    }
}
