using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Business.Services.ParticipantSvc
{
    public interface IParticipantService
    {
        public Task<PagingResult<ViewParticipant>> GetAllPaging(PagingRequest request); 
        public Task<ViewParticipant> GetById(int id);
        public Task<bool> Delete(int id);
        public Task<ViewParticipant> Insert(ParticipantInsertModel model, string token);
        public Task<bool> UpdateAttendance(string seedsCode, string token);
    }
}
