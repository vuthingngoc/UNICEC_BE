using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo
{
    public interface ITeamInRoundRepo : IRepository<TeamInRound>
    {
        public Task<bool> CheckExistedTeamsInRound(int roundId);
        public Task<PagingResult<ViewTeamInRound>> GetByConditions(TeamInRoundRequestModel request);
        public Task<ViewTeamInRound> GetById(int id, bool? status);
        public Task<List<ViewTeamInRound>> GetTopTeamsInCompetition(int competitionId, int top); // no use anymore                
        public Task<List<ViewTeamInRound>> GetViewTeams(List<int> teamIds, int roundId);
        public Task<int> GetTotalPointsTeam(int teamId, int competitionId);        
        public Task<List<int>> GetTeamIdsByRound(int roundId, bool? status);
        public Task<List<ViewTeamInRound>> GetTeamsByRound(int roundId, int? top);
        public Task<List<TeamInRound>> GetTeamsByRound(int roundId);        
        public Task<List<ViewParticipantInTeam>> GetMembersInTeam(int teamId);
        public int GetNumberOfParticipatedMatches(int teamId, int roundId);
        public Task InsertMultiTeams(List<int> teamIds, int roundId);
        public Task InsertMultiTeams(List<TeamInRound> teams);
        public Task UpdateRankTeamsInRound(int roundId);
        public Task UpdateResultTeamsInRound(int roundId, int teamId, int? scores, bool? status);
        public Task DeleteMultiTeams(List<TeamInRound> teams);
        public Task DeleteMultiTeams(List<int> teamIds, int roundId, bool? status);
    }
}
