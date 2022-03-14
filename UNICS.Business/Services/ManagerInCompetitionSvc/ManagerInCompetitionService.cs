using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.ManagerInCompetitionRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ManagerInCompetition;

namespace UNICS.Business.Services.ManagerInCompetitionSvc
{
    public class ManagerInCompetitionService : IManagerInCompetitionService
    {
        private IManagerInCompetitionRepo _managerInCompetitionRepo;

        public ManagerInCompetitionService(IManagerInCompetitionRepo managerInCompetitionRepo)
        {
            _managerInCompetitionRepo = managerInCompetitionRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewManagerInCompetition>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewManagerInCompetition> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(ManagerInCompetitionInsertModel managerInCompetition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewManagerInCompetition managerInCompetition)
        {
            throw new NotImplementedException();
        }
    }
}
