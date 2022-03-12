using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CampusRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Campus;

namespace UNICS.Business.Services.CampusSvc
{
    public class CampusService : ICampusService
    {
        private ICampusRepo _campusRepo;

        public CampusService(ICampusRepo campusRepo)
        {
            _campusRepo = campusRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCampus>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCampus> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CampusInsertModel campus)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(CampusUpdateModel campus)
        {
            throw new NotImplementedException();
        }
    }
}
