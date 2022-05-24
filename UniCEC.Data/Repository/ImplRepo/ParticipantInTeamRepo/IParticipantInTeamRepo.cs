using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public interface IParticipantInTeamRepo : IRepository<ParticipantInTeam>
    {
        public Task<bool> CheckNumberParticipantInTeam(int TeamId, int NumberOfStudentInTeam);

        public Task<ParticipantInTeam> CheckParticipantInTeam(int TeamId, int UserId);

        public Task<ParticipantInTeam> CheckParticipantInAnotherTeam(int CompetitionId, int UserId);

        public Task DeleteParticipantInTeam(int TeamId);
    }
}
