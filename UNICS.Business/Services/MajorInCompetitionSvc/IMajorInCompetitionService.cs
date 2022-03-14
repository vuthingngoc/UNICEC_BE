using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MajorInCompetition;

namespace UNICS.Business.Services.MajorInCompetitionSvc
{
    public interface IMajorInCompetitionService
    {
        Task<PagingResult<ViewMajorInCompetition>> GetAll(PagingRequest request);
        Task<ViewMajorInCompetition> GetById(int id);
        Task<bool> Insert(MajorInCompetitionInsertModel majorInCompetition);
        Task<bool> Update(ViewMajorInCompetition majorInCompetition);
        Task<bool> Delete(int id);
    }
}
