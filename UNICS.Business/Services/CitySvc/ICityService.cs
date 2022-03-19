using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public interface ICityService
    {
        public Task<PagingResult<ViewCity>> GetAll(PagingRequest request);
        public Task<ViewCity> GetByCampusId(int id);
        public Task<bool> Insert(CityInsertModel area);
        public Task<bool> Update(ViewCity area);
        public Task<bool> Delete(int id);
    }
}
