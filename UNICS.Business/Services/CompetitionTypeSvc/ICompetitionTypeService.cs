using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionType;

namespace UNICS.Business.Services.CompetitionTypeSvc
{
    public interface ICompetitionTypeService
    {
        public Task<PagingResult<ViewCompetitionType>> GetAll(PagingRequest request);
        public Task<ViewCompetitionType> GetByCompetitionTypeId(int id);
        public Task<ViewCompetitionType> Insert(CompetitionTypeInsertModel competitionType);
        public Task<bool> Update(ViewCompetitionType competitionType);
        public Task<bool> Delete(int id);
    }
}
