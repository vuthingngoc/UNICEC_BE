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
        public Task<ViewTeam> InsertTeam(TeamInsertModel model, string Token);
        public Task<ViewParticipantInTeam> InsertMemberInTeam(ParticipantInTeamInsertModel model, string Token);
        //-------------------------------------UPDATE
        public Task<bool> UpdateTeamRole(ParticipantInTeamUpdateModel model, string Token);
        public Task<bool> UpdateTeam(TeamUpdateModel model, string Token);

        //-------------------------------------DELETE
        public Task<bool> DeleteByLeader(int TeamId, string Token);

        public Task<bool> OutTeam(int TeamId, string Token);
    }
}
