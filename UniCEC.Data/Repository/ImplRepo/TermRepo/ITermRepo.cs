using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Data.Repository.ImplRepo.TermRepo
{
    public interface ITermRepo : IRepository<Term>
    {
        public Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request);
        public Task<ViewTerm> GetCurrentTermByClub(int clubId);
        public Task<ViewTerm> GetById(int clubId, int id);
        public Task<bool> CloseOldTermByClub(int clubId);
    }
}
