using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo
{
    public class DepartmentInUniversityRepo : Repository<DepartmentInUniversity>, IDepartmentInUniversityRepo
    {
        public DepartmentInUniversityRepo(UniCECContext context) : base(context)
        {

        }
    }
}
