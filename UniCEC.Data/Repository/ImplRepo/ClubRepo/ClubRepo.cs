using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public class ClubRepo : Repository<Club>, IClubRepo
    {
        public ClubRepo(UniCECContext context) : base(context)
        {

        }
    }
}
