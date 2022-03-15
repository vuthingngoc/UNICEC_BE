using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.GroupUniversityRepo
{
    public class GroupUniversityRepo : Repository<GroupUniversity>, IGroupUniversityRepo
    {
        public GroupUniversityRepo(UNICSContext context) : base(context)
        {

        }
    }
}
