using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.AreaRepo
{
    public class AreaRepo : Repository<Area>, IAreaRepo
    {
        public AreaRepo(UNICSContext context) : base(context)
        {

        }
    }
}
