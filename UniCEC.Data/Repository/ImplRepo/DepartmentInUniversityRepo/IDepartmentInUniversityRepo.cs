using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo
{
    public interface IDepartmentInUniversityRepo : IRepository<DepartmentInUniversity>
    {
        public Task<bool> CheckDepartmentBelongToUni(List<int> listDepartmentId, int universityId);
    }
}
