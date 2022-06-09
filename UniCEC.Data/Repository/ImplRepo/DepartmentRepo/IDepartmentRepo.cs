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
        public Task<ViewDepartment> GetById(int id);
        public Task<PagingResult<ViewDepartment>> GetByConditions(DepartmentRequestModel request);
        public Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request);
        //
        public Task<bool> checkDepartment(List<int> listDepartmentId);
    }
}
