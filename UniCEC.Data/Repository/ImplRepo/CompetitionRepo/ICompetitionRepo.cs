using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRepo
{
    public interface ICompetitionRepo : IRepository<Competition>
    {
        public Task<bool> CheckExistCode(string code);
    }
}
