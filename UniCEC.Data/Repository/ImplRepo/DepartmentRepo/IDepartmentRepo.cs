using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public interface IDepartmentRepo : IRepository<Department>
    {
        public Task<List<Department>> GetByName(string name);
        public Task<List<Department>> GetByCompetition(int competitionId);
    }
}
