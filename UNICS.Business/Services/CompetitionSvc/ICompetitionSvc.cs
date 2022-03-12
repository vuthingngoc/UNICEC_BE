using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Competition;

namespace UNICS.Business.Services.CompetitionSvc
{
    public interface ICompetitionSvc
    {
        Task<PagingResult<Competition>> GetAll(PagingRequest request);
        Task<Competition> GetById(int id);
        Task<PagingResult<Competition>> Insert(CompetitionInsertModel competition);
        Task<PagingResult<Competition>> Update(ViewCompetition competition);
        Task<PagingResult<Competition>> Delete(int id);
    }
}
