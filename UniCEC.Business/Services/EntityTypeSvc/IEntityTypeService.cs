using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.EntityType;

namespace UniCEC.Business.Services.EntityTypeSvc
{
    public interface IEntityTypeService
    {
        public Task<PagingResult<ViewEntityType>> GetAllPaging(PagingRequest request);
        public Task<ViewEntityType> GetByEntityTypeId(int id);
        public Task<ViewEntityType> Insert(EntityTypeInsertModel entityType);
        public Task<bool> Update(ViewEntityType entityType);
        public Task<bool> Delete(int id);
    }
}
