using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.MajorInCompetitionRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MajorInCompetition;

namespace UNICS.Business.Services.MajorInCompetitionSvc
{
    public class MajorInCompetitionService : IMajorInCompetitionService
    {
        private IMajorInCompetitionRepo _majorInCompetitionRepo;

        public MajorInCompetitionService(IMajorInCompetitionRepo majorInCompetitionRepo)
        {
            _majorInCompetitionRepo = majorInCompetitionRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewMajorInCompetition>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMajorInCompetition> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(MajorInCompetitionInsertModel majorInCompetition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewMajorInCompetition majorInCompetition)
        {
            throw new NotImplementedException();
        }
    }
}
