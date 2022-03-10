using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CampusRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Campus;

namespace UNICS.Business.Services.CampusSvc
{
    public class CampusSvc : ICampusSvc
    {
        private ICampusRepo _campusRepo;

        public CampusSvc(ICampusRepo campusRepo)
        {
            _campusRepo = campusRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCampus>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ViewCampus> GetById()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
