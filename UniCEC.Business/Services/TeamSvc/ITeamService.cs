using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
{
    public interface ITeamService
    {
               
        public Task<PagingResult<ViewTeam>> GetAllTeamInCompetition(TeamRequestModel request, string token);

        public Task<ViewDetailTeam> GetDetailTeamInCompetition(int teamId, int competitionId, string token);

        //-------------------------------------INSERT
        public Task<ViewTeam> InsertTeam(TeamInsertModel model, string token);
        public Task<ViewParticipantInTeam> InsertMemberInTeam(ParticipantInTeamInsertModel model, string token);
        //-------------------------------------UPDATE
        public Task<bool> UpdateTeamRole(ParticipantInTeamUpdateModel model, string token);
        public Task<bool> UpdateTeam(TeamUpdateModel model, string Token);
        public Task<bool> CompetitionManagerLockTeam(LockTeamModel model, string token);

        //-------------------------------------DELETE

        public Task<bool> DeleteMemberByLeader(int teamId, int participantId, string token);

        public Task<bool> DeleteByLeader(int teamId, string token);

        public Task<bool> OutTeam(int teamId, string token);

        // Gwi
        public Task<ViewTeamInCompetition> GetTotalResultTeamInCompetition(int competitionId, int teamId);
        public Task<List<ViewResultTeam>> GetFinalResultTeamsInCompetition(string token, int competitionId, int? top);
    }
}
