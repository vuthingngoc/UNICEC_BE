using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubActivityRepo
{
    public class ClubActivityRepo : Repository<ClubActivity>, IClubActivityRepo
    {
        public ClubActivityRepo(UniCECContext context) : base(context)
        {

        }
    }
}
