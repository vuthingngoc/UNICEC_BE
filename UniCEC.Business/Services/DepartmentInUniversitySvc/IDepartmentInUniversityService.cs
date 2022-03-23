using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.DepartmentInUniversity;

namespace UniCEC.Business.Services.DepartmentInUniversitySvc
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
