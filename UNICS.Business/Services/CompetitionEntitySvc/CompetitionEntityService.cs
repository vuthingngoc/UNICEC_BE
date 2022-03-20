using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AlbumTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionEntity;

namespace UNICS.Business.Services.CompetitionEntitySvc
{
    public class CompetitionEntityService : ICompetitionEntityService
    {
        private ICompetitionEntityRepo _competitionEntityRepo;

        public CompetitionEntityService(ICompetitionEntityRepo competitionEntityRepo)
        {
            _competitionEntityRepo = competitionEntityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCompetitionEntity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionEntity> GetByCompetitionEntityId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionEntity> Insert(CompetitionEntityInsertModel competitionEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(CompetitionEntityUpdateModel competitionEntity)
        {
            throw new NotImplementedException();
        }
    }
}
