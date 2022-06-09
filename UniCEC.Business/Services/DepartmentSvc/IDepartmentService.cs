using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<ViewDepartment> GetById(int id);
        public Task<PagingResult<ViewDepartment>> GetByConditions(DepartmentRequestModel request);
        public Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<ViewDepartment> Insert(string name);
        public Task Update(DepartmentUpdateModel model);
        public Task Delete(int id);
    }
}
