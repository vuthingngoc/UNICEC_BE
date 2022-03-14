using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Area;

namespace UNICS.Business.Services.AreaSvc
{
    public interface IAreaService
    {
        public Task<PagingResult<ViewArea>> GetAll(PagingRequest request);
        public Task<ViewArea> GetByCampusId(int id);
        public Task<bool> Insert(AreaInsertModel area);
        public Task<bool> Update(ViewArea area);
        public Task<bool> Delete(int id);
    }
}
