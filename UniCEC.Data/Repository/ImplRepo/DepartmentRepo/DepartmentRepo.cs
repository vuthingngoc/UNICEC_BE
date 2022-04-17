using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo
    {
        public DepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public Task<PagingResult<Department>> GetByCompetitionId(int competitionId)
        {
            throw new System.NotImplementedException();
        }
    }
}
