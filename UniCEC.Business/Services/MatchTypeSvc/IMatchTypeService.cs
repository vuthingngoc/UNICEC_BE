using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.MatchType;

namespace UniCEC.Business.Services.MatchTypeSvc
{
    public interface IMatchTypeService
    {
        public Task<ViewMatchType> GetById(int id, string token);
        public Task<List<ViewMatchType>> GetByConditions(MatchTypeRequestModel request, string token);
        public Task<ViewMatchType> Insert(MatchTypeInsertModel model, string token);
        public Task Update(MatchTypeUpdateModel model, string token);
        public Task Delete(int id, string token);
    }
}
