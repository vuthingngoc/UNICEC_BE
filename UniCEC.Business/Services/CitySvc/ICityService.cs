using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public interface ICityService
    {
        //public Task UploadFile(IFormFile file, string token);
        public Task<PagingResult<ViewCity>> GetListCities(CityRequestModel request);
        public Task<ViewCity> GetByCityId(int id);
        public Task<ViewCity> Insert(CityInsertModel city);
        public Task<bool> Update(ViewCity city);
        public Task<bool> Delete(int id);
    }
}
