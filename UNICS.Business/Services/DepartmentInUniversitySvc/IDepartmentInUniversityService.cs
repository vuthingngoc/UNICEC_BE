using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.DepartmentInUniversity;

namespace UNICS.Business.Services.DepartmentInUniversitySvc
{
    public interface IDepartmentInUniversityService
    {
        public Task<PagingResult<ViewDepartmentInUniversity>> GetAll(PagingRequest request);
        public Task<ViewDepartmentInUniversity> GetByDepartmentInUniversityId(int id);
        public Task<ViewDepartmentInUniversity> Insert(DepartmentInUniversityInsertModel departmentInUniversity);
        public Task<bool> Update(ViewDepartmentInUniversity departmentInUniversity);
        public Task<bool> Delete(int id);
    }
}
