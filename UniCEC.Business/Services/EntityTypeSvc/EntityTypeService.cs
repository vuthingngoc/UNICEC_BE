using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.EntityTypeRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.EntityType;

namespace UniCEC.Business.Services.EntityTypeSvc
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
