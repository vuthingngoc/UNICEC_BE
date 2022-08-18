using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Business.Services.ParticipantSvc
{
    public interface IParticipantService
    {
        public Task<PagingResult<ViewParticipant>> GetAllPaging(PagingRequest request);
        public Task<PagingResult<ViewParticipant>> GetByConditions(ParticipantRequestModel request, string token);
        public Task<ViewParticipant> GetByCompetitionId(int competitionId, string token);
        public Task<bool> Delete(int id);
        public Task<ViewParticipant> Insert(ParticipantInsertModel model, string token);
        public Task<bool> UpdateAttendance(string seedsCode, string token);
    }
}
