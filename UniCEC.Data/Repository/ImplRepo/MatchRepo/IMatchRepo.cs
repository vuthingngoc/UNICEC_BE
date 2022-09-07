using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;

namespace UniCEC.Data.Repository.ImplRepo.MatchRepo
{
    public interface IMatchRepo : IRepository<Match>
    {
        public Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request);
    }
}
