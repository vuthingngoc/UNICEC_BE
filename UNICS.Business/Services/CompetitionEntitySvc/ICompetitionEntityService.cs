using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionEntity;

namespace UNICS.Business.Services.CompetitionEntitySvc
{
    public interface ICompetitionEntityService
    {
        public Task<PagingResult<ViewCompetitionEntity>> GetAll(PagingRequest request);
        public Task<ViewCompetitionEntity> GetByCompetitionEntityId(int id);
        public Task<ViewCompetitionEntity> Insert(CompetitionEntityInsertModel competitionEntity);
        public Task<bool> Update(CompetitionEntityUpdateModel competitionEntity);
        public Task<bool> Delete(int id);
    }
}
