using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CampusRepo
{
    public class CampusRepo : Repository<Campus>, ICampusRepo
    {
        public CampusRepo(UNICSContext context) : base(context)
        {

        }
    }
}
