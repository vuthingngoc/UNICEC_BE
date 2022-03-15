using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Major;

namespace UNICS.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        Task<PagingResult<ViewMajor>> GetAll(PagingRequest request);
        Task<ViewMajor> GetById(int id);
        Task<bool> Insert(MajorInsertModel major);
        Task<bool> Update(ViewMajor major);
        Task<bool> Delete(int id);

    }
}
