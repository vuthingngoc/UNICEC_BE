using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public interface ICompetitionRoundService
    {
        public Task<ViewCompetitionRound> GetById(string token, int id);
        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel request);
        public Task<List<ViewCompetitionRound>> Insert(string token, List<CompetitionRoundInsertModel> models);
        public Task Update(string token, CompetitionRoundUpdateModel model);
        public Task Delete(string token, int id);
    }
}
