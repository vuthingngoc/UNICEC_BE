using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.ParticipantInTeamRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ParticipantInTeam;

namespace UNICS.Business.Services.ParticipantInTeamSvc
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

        public Task<PagingResult<ViewParticipantInTeam>> GetAll(PagingRequest request)
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
