using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Business.Services.CompetitionEntitySvc
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
