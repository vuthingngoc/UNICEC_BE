using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ParticipantInTeam;

namespace UNICS.Business.Services.ParticipantInTeamSvc
{
    public interface IParticipantInTeamService
    {
        public Task<PagingResult<ViewParticipantInTeam>> GetAll(PagingRequest request);
        public Task<ViewParticipantInTeam> GetByParticipantInTeamId(int id);
        public Task<ViewParticipantInTeam> Insert(ParticipantInTeamInsertModel participantInTeam);
        public Task<bool> Update(ViewParticipantInTeam participantInTeam);
        public Task<bool> Delete(int id);
    }
}
