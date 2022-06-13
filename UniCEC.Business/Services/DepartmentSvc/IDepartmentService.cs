using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<ViewDepartment> GetById(string token, int id);
        public Task<PagingResult<ViewDepartment>> GetByConditions(string token, DepartmentRequestModel request);
        public Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<ViewDepartment> Insert(string token, string name);
        public Task Update(string token, DepartmentUpdateModel model);
        public Task Delete(string token, int id);
    }
}
