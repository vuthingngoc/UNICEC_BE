using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MajorType;

namespace UNICS.Business.Services.MajorTypeSvc
{
    public interface IMajorTypeService
    {
        Task<PagingResult<ViewMajorType>> GetAll(PagingRequest request);
        Task<ViewMajorType> GetById(int id);
        Task<bool> Insert(MajorTypeInsertModel majorType);
        Task<bool> Update(ViewMajorType majorType);
        Task<bool> Delete(int id);
    }
}
