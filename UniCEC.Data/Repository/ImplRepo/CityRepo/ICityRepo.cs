using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Data.Repository.ImplRepo.CityRepo
{
    public interface ICityRepo : IRepository<City>
    {
        // To be continued ...
        public Task<PagingResult<ViewCity>> GetListCities(CityRequestModel request);
    }
}
