using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.TermRepo
{
    public interface ITermRepo : IRepository<Term>
    {
        public Task<PagingResult<Term>> GetByClub(int clubId, PagingRequest request);
        public Task<PagingResult<Term>> GetByConditions(int clubId, TermRequestModel request);
        public Task<Term> GetById(int clubId, int id);
    }
}
