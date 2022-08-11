using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public interface IDepartmentRepo : IRepository<Department>
    {
        public Task<List<ViewDepartment>> GetAllByUniversity(int universityId);
        public Task<ViewDepartment> GetById(int id, bool? status, int? universityId);
        public Task<ViewDepartment> GetByCode(string departmentCode, bool? status, int? universityId);
        public Task<PagingResult<ViewDepartment>> GetByConditions(DepartmentRequestModel request);
        public Task<PagingResult<Department>> GetByUniversity(int universityId, PagingRequest request);
        public Task<List<int>> GetIdsByMajorId(int majorId, bool? status);
        public Task<int> CheckExistedDepartmentCode(int universityId, string code);
        public Task<int> CheckDuplicatedName(int universityId, string name);
    }
}
