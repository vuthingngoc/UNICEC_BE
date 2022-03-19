using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.AreaRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public class CityService : ICityService
    {
        private ICityRepo _areaRepo;

        public CityService(ICityRepo areaRepo)
        {
            _areaRepo = areaRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCity> GetByCampusId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CityInsertModel area)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCity area)
        {
            throw new NotImplementedException();
        }
    }
}
