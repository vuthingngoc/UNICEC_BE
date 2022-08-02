using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public interface IParticipantInTeamRepo : IRepository<ParticipantInTeam>
    {
        public Task<bool> CheckNumberParticipantInTeam(int teamId, int numberOfStudentInTeam);

        public Task<ParticipantInTeam> CheckParticipantInTeam(int teamId, int userId);

        public Task<ParticipantInTeam> CheckParticipantInAnotherTeam(int competitionId, int userId);

        public Task<int> GetNumberOfMemberInTeam (int teamId, int competitionId);

        public Task DeleteParticipantInTeam(int participantInTeamId);

        public Task DeleteAllParticipantInTeam(int teamId);
    }
}
