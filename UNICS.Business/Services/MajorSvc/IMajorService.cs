using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Major;

namespace UNICS.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        public Task<PagingResult<ViewMajor>> GetAll(PagingRequest request);
        public Task<ViewMajor> GetByMajorId(int id);
        public Task<ViewMajor> Insert(MajorInsertModel major);
        public Task<bool> Update(ViewMajor major);
        public Task<bool> Delete(int id);

    }
}
