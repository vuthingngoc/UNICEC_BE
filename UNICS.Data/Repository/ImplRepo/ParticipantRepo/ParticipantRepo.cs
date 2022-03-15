using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ParticipantRepo
{
    public class ParticipantRepo : Repository<Participant>, IParticipantRepo
    {
        public ParticipantRepo(UNICSContext context) : base(context)
        {

        }
    }
}
