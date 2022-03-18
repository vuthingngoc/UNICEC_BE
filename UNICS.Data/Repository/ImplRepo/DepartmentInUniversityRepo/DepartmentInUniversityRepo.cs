using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.DepartmentInUniversityRepo 
{
    public class DepartmentInUniversityRepo : Repository<DepartmentInUniversity>, IDepartmentInUniversityRepo {
        public DepartmentInUniversityRepo(UNICSContext context) : base(context) 
        {

        }
    }
}
