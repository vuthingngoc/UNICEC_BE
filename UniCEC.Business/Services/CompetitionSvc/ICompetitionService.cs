using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {
        public Task<PagingResult<ViewCompetition>> GetAll(PagingRequest request);
        public Task<ViewCompetition> GetById(int id);
        public Task<ViewCompetition> Insert(CompetitionInsertModel competition);
        public Task<bool> Update(ViewCompetition competition);
        public Task<bool> Delete(int id);
    }
}
