using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Participant;

namespace UNICS.Business.Services.ParticipantSvc
{
    public interface IParticipantService
    {
        Task<PagingResult<ViewParticipant>> GetAll(PagingRequest request);
        Task<ViewParticipant> GetById(int id);
        Task<bool> Insert(ParticipantInsertModel participant);
        Task<bool> Update(ViewParticipant participant);
        Task<bool> Delete(int id);
    }
}
