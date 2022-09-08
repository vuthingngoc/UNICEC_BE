using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;

namespace UniCEC.Business.Services.MatchSvc
{
    public interface IMatchService
    {
        public Task<ViewMatch> GetById(int id, string token);
        public Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request, string token);
        public Task<ViewMatch> Insert(MatchInsertModel model, string token);
        public Task Update(MatchUpdateModel model, string token);
        public Task Delete(int id, string token);
    }
}
