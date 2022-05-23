using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamRepo
{
    public interface ITeamRepo : IRepository<Team>
    {
        public Task<bool> CheckExistCode(string code);

    }
}
