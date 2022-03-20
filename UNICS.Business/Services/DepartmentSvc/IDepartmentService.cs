using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Department;

namespace UNICS.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<PagingResult<ViewDepartment>> GetAll(PagingRequest request);
        public Task<ViewDepartment> GetByDepartmentId(int id);
        public Task<ViewDepartment> Insert(DepartmentInsertModel department);
        public Task<bool> Update(ViewDepartment department);
        public Task<bool> Delete(int id);
    }
}
