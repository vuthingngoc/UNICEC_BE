using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.RatingRepo
{
    public class RatingRepo : Repository<Rating>, IRatingRepo
    {
        public RatingRepo(UNICSContext context) : base(context)
        {

        }
    }
}
