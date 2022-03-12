using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionType;

namespace UNICS.Business.Services.CompetitionTypeSvc
{
    public interface ICompetitionTypeService
    {
        Task<PagingResult<ViewCompetitionType>> GetAll(PagingRequest request);
        Task<ViewCompetitionType> GetById(int id);
        Task<bool> Insert(CompetitionTypeInsertModel competitionType);
        Task<bool> Update(ViewCompetitionType competitionType);
        Task<bool> Delete(int id);
    }
}
