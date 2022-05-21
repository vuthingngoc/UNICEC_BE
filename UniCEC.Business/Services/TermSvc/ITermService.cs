using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.TermSvc
{
    public interface ITermService
    {
        public Task<ViewTerm> GetById(string token, int clubId, int id);
        public Task<ViewTerm> GetCurrentTermByClub(string token, int clubId);        
        public Task<PagingResult<ViewTerm>> GetByConditions(string token, int clubId, TermRequestModel request);
        public Task<ViewTerm> Insert(TermInsertModel term);
        public Task Update(string token, TermUpdateModel term, int clubId);
        //public Task Delete(int id);
        public Task CloseOldTermByClub(int clubId);
    }
}
