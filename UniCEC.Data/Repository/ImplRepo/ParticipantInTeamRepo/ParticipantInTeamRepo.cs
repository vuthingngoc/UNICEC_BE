using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public class ParticipantInTeamRepo : Repository<ParticipantInTeam>, IParticipantInTeamRepo
    {
        public ParticipantInTeamRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckParticipantInTeam(int TeamId, int NumberOfStudentInTeam)
        {
            var query = from pit in context.ParticipantInTeams
                        where pit.TeamId == TeamId
                        select pit;

            int count = await query.CountAsync();
            if (count < NumberOfStudentInTeam)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
