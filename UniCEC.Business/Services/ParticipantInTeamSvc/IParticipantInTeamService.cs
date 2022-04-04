using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;

namespace UniCEC.Business.Services.ParticipantInTeamSvc
{
    public interface IParticipantInTeamService
    {
        public Task<PagingResult<ViewParticipantInTeam>> GetAllPaging(PagingRequest request);
        public Task<ViewParticipantInTeam> GetByParticipantInTeamId(int id);
        public Task<ViewParticipantInTeam> Insert(ParticipantInTeamInsertModel participantInTeam);
        public Task<bool> Update(ViewParticipantInTeam participantInTeam);
        public Task<bool> Delete(int id);
    }
}
