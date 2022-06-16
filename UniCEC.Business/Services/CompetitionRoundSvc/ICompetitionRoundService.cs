using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public interface ICompetitionRoundService
    {
        public Task<ViewCompetitionRound> GetById(string token, int id);
        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(string token, CompetitionRoundRequestModel request);
        public Task<ViewCompetitionRound> Insert(string token, CompetitionRoundInsertModel model);
        public Task Update(string token, CompetitionRoundUpdateModel model);
        public Task Delete(string token, int id);
    }
}
