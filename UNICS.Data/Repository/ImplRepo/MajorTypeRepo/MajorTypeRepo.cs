using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.MajorTypeRepo
{
    public class MajorTypeRepo : Repository<MajorType>, IMajorTypeRepo
    {
        public MajorTypeRepo(UNICSContext context) : base(context)
        {

        }
    }
}
