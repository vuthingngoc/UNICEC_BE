using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Competition;

namespace UNICS.Business.Services.CompetitionSvc
{
    public interface ICompetitionSvc
    {
        Task<PagingResult<ViewCompetition>> GetAll(PagingRequest request);
        Task<ViewCompetition> GetById(int id);
        Task<bool> Insert(CompetitionInsertModel competition);
        Task<bool> Update(ViewCompetition competition);
        Task<bool> Delete(int id);
    }
}
