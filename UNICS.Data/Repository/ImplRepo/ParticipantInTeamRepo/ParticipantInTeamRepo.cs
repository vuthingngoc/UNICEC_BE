using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ParticipantInTeamRepo
{
    public class ParticipantInTeamRepo : Repository<ParticipantInTeam>, IParticipantInTeamRepo
    {
        public ParticipantInTeamRepo(UNICSContext context) : base(context)
        {

        }
    }
}
