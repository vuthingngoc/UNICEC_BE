using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Data.Repository.ImplRepo.CityRepo
{
    public interface ICityRepo : IRepository<City>
    {
        public Task<PagingResult<ViewCity>> SearchCitiesByName(string name, PagingRequest request);
        public Task<ViewCity> GetById(int id);
    }
}
