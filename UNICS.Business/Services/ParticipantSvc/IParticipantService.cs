using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Participant;

namespace UNICS.Business.Services.ParticipantSvc
{
    public interface IParticipantService
    {
        public Task<PagingResult<ViewParticipant>> GetAll(PagingRequest request);
        public Task<ViewParticipant> GetById(int id);
        public Task<ViewParticipant> Insert(ParticipantInsertModel participant);
        public Task<bool> Update(ViewParticipant participant);
        public Task<bool> Delete(int id);
    }
}
