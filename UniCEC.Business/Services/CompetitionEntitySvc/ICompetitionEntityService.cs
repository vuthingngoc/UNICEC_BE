using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Business.Services.CompetitionEntitySvc
{
    public interface ICompetitionEntityService
    {
        public Task<PagingResult<ViewCompetitionEntity>> GetAllPaging(PagingRequest request);
        public Task<ViewCompetitionEntity> GetByCompetitionEntityId(int id);
        public Task<ViewCompetitionEntity> Insert(CompetitionEntityInsertModel competitionEntity);
        public Task<bool> Update(CompetitionEntityUpdateModel competitionEntity);
        public Task<bool> Delete(int id);
    }
}
