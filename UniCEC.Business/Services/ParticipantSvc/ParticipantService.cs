using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Business.Services.ParticipantSvc
{
    public class ParticipantService : IParticipantService
    {
        private IParticipantRepo _participantRepo;

        public ParticipantService(IParticipantRepo participantRepo)
        {
            _participantRepo = participantRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewParticipant>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewParticipant> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewParticipant> Insert(ParticipantInsertModel participant)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewParticipant participant)
        {
            throw new NotImplementedException();
        }
    }
}
