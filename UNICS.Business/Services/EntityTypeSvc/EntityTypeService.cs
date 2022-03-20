using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.EntityTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.EntityType;

namespace UNICS.Business.Services.EntityTypeSvc
{
    public class EntityTypeService : IEntityTypeService
    {
        private IEntityTypeRepo _entityTypeRepo;

        public EntityTypeService(IEntityTypeRepo entityTypeRepo)
        {
            _entityTypeRepo = entityTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewEntityType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewEntityType> GetByEntityTypeId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewEntityType> Insert(EntityTypeInsertModel entityType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewEntityType entityType)
        {
            throw new NotImplementedException();
        }
    }
}
