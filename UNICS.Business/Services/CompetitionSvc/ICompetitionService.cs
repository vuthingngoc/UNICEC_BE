using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Competition;

namespace UNICS.Business.Services.CompetitionSvc
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
