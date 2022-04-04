using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;

namespace UniCEC.Business.Services.ParticipantInTeamSvc
{
    public class ParticipantInTeamService : IParticipantInTeamService
    {
        private IParticipantInTeamRepo _participantInTeamRepo;

        public ParticipantInTeamService(IParticipantInTeamRepo participantInTeamRepo)
        {
            _participantInTeamRepo = participantInTeamRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewParticipantInTeam>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewParticipantInTeam> GetByParticipantInTeamId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewParticipantInTeam> Insert(ParticipantInTeamInsertModel participantInTeam)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewParticipantInTeam participantInTeam)
        {
            throw new NotImplementedException();
        }
    }
}
