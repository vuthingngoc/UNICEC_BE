using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public interface IDepartmentService
    {
        public Task<List<ViewDepartment>> GetAllByUniversity(int universityId,string token);
        //public Task<PagingResult<ViewMajor>> GetByUniversity(int universityId, PagingRequest request);
        public Task<ViewDepartment> GetById(string token, int id);
        public Task<ViewDepartment> GetByCode(string token, string majorCode);
        public Task<PagingResult<ViewDepartment>> GetByConditions(string token, DepartmentRequestModel request);
        public Task<ViewDepartment> Insert(string token, DepartmentInsertModel major);
        public Task Update(string token, DepartmentUpdateModel department);
        public Task Delete(string token, int id);        
    }
}
