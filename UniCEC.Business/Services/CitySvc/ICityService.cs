using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.City;

namespace UniCEC.Business.Services.CitySvc
{
    public interface ICityService
    {
        public Task<PagingResult<ViewCity>> GetAllPaging(PagingRequest request);
        public Task<ViewCity> GetByCityId(int id);
        public Task<ViewCity> Insert(CityInsertModel city);
        public Task<bool> Update(ViewCity city);
        public Task<bool> Delete(int id);
    }
}
