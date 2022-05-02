using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.TermSvc
{
    public interface ITermService
    {
        public Task<ViewTerm> GetById(int clubId, int id);
        public Task<PagingResult<ViewTerm>> GetByClub(int clubId, PagingRequest request);        
        public Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request);
        public Task<ViewTerm> Insert(TermInsertModel term);
        public Task Update(ViewTerm term);
        public Task Delete(int id);
    }
}
