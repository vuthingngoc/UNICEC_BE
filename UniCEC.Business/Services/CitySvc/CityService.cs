using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.CityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
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
