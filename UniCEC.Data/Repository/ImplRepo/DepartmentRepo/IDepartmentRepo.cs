using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public interface IDepartmentRepo : IRepository<Department>
    {
        public Task<PagingResult<Department>> GetByName(string name, PagingRequest request);
        public Task<PagingResult<Department>> GetByCompetition(int competitionId, PagingRequest request);
        //
        public Task<bool> checkDepartment(List<int> listDepartmentId);
    }
}
