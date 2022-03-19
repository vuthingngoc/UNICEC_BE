using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ClubRepo
{
    public class ClubRepo : Repository<Club>, IClubRepo
    {
        public ClubRepo(UNICSContext context) : base(context)
        {

        }
    }
}
