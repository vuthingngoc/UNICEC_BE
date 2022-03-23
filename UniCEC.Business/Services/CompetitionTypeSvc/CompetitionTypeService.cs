using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionType;

namespace UniCEC.Business.Services.CompetitionTypeSvc
{
    public class CompetitionTypeService : ICompetitionTypeService
    {
        private ICompetitionTypeRepo _competitionTypeRepo;

        public CompetitionTypeService(ICompetitionTypeRepo competitionTypeRepo)
        {
            _competitionTypeRepo = competitionTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCompetitionType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionType> GetByCompetitionTypeId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionType> Insert(CompetitionTypeInsertModel competitionType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCompetitionType competitionType)
        {
            throw new NotImplementedException();
        }
    }
}
