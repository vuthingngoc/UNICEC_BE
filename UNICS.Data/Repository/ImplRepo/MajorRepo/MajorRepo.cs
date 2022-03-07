using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.MajorRepo
{
    public class MajorRepo : Repository<Major>, IMajorRepo
    {
        public MajorRepo(UNICSContext context) : base(context)
        {

        }
    }
}
