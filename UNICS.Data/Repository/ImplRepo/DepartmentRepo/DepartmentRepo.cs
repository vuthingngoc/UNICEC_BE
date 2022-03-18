using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.DepartmentRepo 
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo {
        public DepartmentRepo(UNICSContext context) : base(context) {

        }
    }
}
