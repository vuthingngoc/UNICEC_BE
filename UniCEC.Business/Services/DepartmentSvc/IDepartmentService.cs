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
        public Task<PagingResult<ViewDepartment>> GetByName(string name, PagingRequest request);
        public Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<ViewDepartment> Insert(DepartmentInsertModel department);
        public Task Update(ViewDepartment department);
        public Task Delete(int id);
    }
}
