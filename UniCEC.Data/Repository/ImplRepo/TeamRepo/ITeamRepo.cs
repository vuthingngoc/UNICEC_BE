using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Data.Repository.ImplRepo.TeamRepo
{
    public interface ITeamRepo : IRepository<Team>
    {
        public Task<PagingResult<ViewTeam>> GetAllTeamInCompetition(TeamRequestModel request);

        public Task<ViewDetailTeam> GetDetailTeamInCompetition(int teamId, int competitionId);

        public Task<bool> CheckExistCode(string code);

        public Task<bool> CheckNumberOfTeam(int CompetitionId);

        public Task<Team> GetTeamByInvitedCode(string invitedCode);

        public Task DeleteTeam(int TeamId);

        public Task<int> CountNumberOfTeamIsLocked(int competitionId);

        public Task<bool> UpdateStatusAvailableForAllTeam(int CompetitionId);

        //
        public Task<bool> CheckExistedTeam(int teamId);
        public Task<int> GetNumberOfMemberInTeam(int teamId);
        // for get total result of a team in a competition
        public Task<List<ViewResultTeam>> GetFinalResultAllTeamsInComp(int competitionId, int? top); 
        public Task<List<int>> GetAllTeamIdsInComp(int competitionId);
        public Task<ViewTeamInCompetition> GetTotalResultTeamInCompetition(int competitionId, int teamId);
    }
}
