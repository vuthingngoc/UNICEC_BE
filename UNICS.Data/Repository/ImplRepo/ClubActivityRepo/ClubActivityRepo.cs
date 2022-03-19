using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ClubActivityRepo
{
    public class ClubActivityRepo : Repository<ClubActivity>, IClubActivityRepo
    {
        public ClubActivityRepo(UNICSContext context) : base(context)
        {

        }
    }
}
