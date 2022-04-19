using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<PagingResult<ViewDepartment>> GetAllPaging(PagingRequest request);
        public Task<ViewDepartment> GetByDepartment(int id);
        public Task<List<ViewDepartment>> GetByName(string name);
        public Task<List<ViewDepartment>> GetByCompetition(int competitionId);
        public Task<ViewDepartment> Insert(DepartmentInsertModel department);
        public Task<bool> Update(ViewDepartment department);
        public Task<bool> Delete(int id);
    }
}
