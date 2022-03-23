using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public class MajorRepo : Repository<Major>, IMajorRepo
    {
        public MajorRepo(UniCECContext context) : base(context)
        {

        }
    }
}
