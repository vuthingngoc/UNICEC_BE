using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.City;

namespace UNICS.Business.Services.CitySvc
{
    public interface ICityService
    {
        public Task<PagingResult<ViewCity>> GetAll(PagingRequest request);
        public Task<ViewCity> GetByCityId(int id);
        public Task<ViewCity> Insert(CityInsertModel city);
        public Task<bool> Update(ViewCity city);
        public Task<bool> Delete(int id);
    }
}
