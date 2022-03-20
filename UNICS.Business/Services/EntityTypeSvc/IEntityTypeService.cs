using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.EntityType;

namespace UNICS.Business.Services.EntityTypeSvc
{
    public interface IEntityTypeService
    {
        public Task<PagingResult<ViewEntityType>> GetAll(PagingRequest request);
        public Task<ViewEntityType> GetByEntityTypeId(int id);
        public Task<ViewEntityType> Insert(EntityTypeInsertModel entityType);
        public Task<bool> Update(ViewEntityType entityType);
        public Task<bool> Delete(int id);
    }
}
