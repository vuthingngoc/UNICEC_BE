using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public interface IParticipantInTeamRepo : IRepository<ParticipantInTeam>
    {
        public Task<bool> CheckParticipantInTeam(int TeamId, int NumberOfStudentInTeam);
    }
}
