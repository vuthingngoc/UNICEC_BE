using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo
{
    public class ClubPreviousRepo : Repository<ClubPreviou>, IClubPreviousRepo
    {
        public ClubPreviousRepo(UniCECContext context) : base(context)
        {

        }
    }
}
