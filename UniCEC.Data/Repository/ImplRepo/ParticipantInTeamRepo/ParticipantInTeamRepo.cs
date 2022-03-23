using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public class ParticipantInTeamRepo : Repository<ParticipantInTeam>, IParticipantInTeamRepo
    {
        public ParticipantInTeamRepo(UniCECContext context) : base(context)
        {

        }
    }
}
