using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ParticipantRepo
{
    public class ParticipantRepo : Repository<Participant>, IParticipantRepo
    {
        public ParticipantRepo(UniCECContext context) : base(context)
        {

        }
    }
}
