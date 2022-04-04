using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionType;

namespace UniCEC.Business.Services.CompetitionTypeSvc
{
    public interface ICompetitionTypeService
    {
        public Task<PagingResult<ViewCompetitionType>> GetAllPaging(PagingRequest request);
        public Task<ViewCompetitionType> GetByCompetitionTypeId(int id);
        public Task<ViewCompetitionType> Insert(CompetitionTypeInsertModel competitionType);
        public Task<bool> Update(ViewCompetitionType competitionType);
        public Task<bool> Delete(int id);
    }
}
