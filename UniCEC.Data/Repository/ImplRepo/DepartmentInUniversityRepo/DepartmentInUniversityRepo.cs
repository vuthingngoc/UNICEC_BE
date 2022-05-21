using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo
{
    public class DepartmentInUniversityRepo : Repository<DepartmentInUniversity>, IDepartmentInUniversityRepo
    {
        public DepartmentInUniversityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> checkDepartmentBelongToUni(List<int> listDepartmentId, int universityId)
        {
            bool result = true;
            foreach (int DepId in listDepartmentId)
            {
                var query = await (from dep_uni in context.DepartmentInUniversities
                                   where dep_uni.DepartmentId == DepId && dep_uni.UniversityId == universityId
                                   select dep_uni).FirstOrDefaultAsync();

                if (query == null)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
