using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CityRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.City;

namespace UNICS.Business.Services.CitySvc
{
    public class CityService : ICityService
    {
        private ICityRepo _cityRepo;

        public CityService(ICityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCity> GetByCityId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCity> Insert(CityInsertModel city)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCity city)
        {
            throw new NotImplementedException();
        }
    }
}
