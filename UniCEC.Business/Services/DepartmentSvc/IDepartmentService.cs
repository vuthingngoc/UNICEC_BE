using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<PagingResult<ViewDepartment>> GetAllPaging(PagingRequest request);
        public Task<ViewDepartment> GetByDepartmentId(int id);
        public Task<ViewDepartment> Insert(DepartmentInsertModel department);
        public Task<bool> Update(ViewDepartment department);
        public Task<bool> Delete(int id);
    }
}
