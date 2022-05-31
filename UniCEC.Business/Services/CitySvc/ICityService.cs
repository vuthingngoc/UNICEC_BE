using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public interface ICityService
    {
        //public Task<string> UploadFile(IFormFile file, string token);
        //public Task DeleteFile(string url);
        //public Task UpdateFile(string oldFileName, IFormFile file, string token);
        public Task<PagingResult<ViewCity>> SearchCitiesByName(string name, string token, PagingRequest request);
        public Task<ViewCity> GetByCityId(int id, string token);
        public Task<ViewCity> Insert(CityInsertModel city);
        public Task<bool> Update(CityUpdateModel city);
        public Task Delete(int id);
    }
}
