using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionType;

namespace UNICS.Business.Services.CompetitionTypeSvc
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

        public Task<ViewCompetitionType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CompetitionTypeInsertModel competitionType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCompetitionType competitionType)
        {
            throw new NotImplementedException();
        }
    }
}
