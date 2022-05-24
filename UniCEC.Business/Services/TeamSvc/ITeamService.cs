using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
{
    public interface ITeamService
    {
        public Task<PagingResult<ViewTeam>> GetAllPaging(PagingRequest request);
        public Task<ViewTeam> GetByTeamId(int id);    
                
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
