using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.MatchTypeRepo
{
    public interface IMatchTypeRepo : IRepository<MatchType>
    {
        public Task<bool> CheckExistedType(int typeId);
    }
}
