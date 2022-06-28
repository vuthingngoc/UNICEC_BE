using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
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

        public async Task<List<ViewEntityType>> GetAllEntityType(string token)
        {
            try
            {
                List<ViewEntityType> result = await _entityTypeRepo.GetAll();
                return (result != null) ? result : throw new NullReferenceException("No Data");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewEntityType> GetEntityTypeById(int id, string token)
        {
            try
            {
                if(id == 0) throw new ArgumentNullException("Id is NULL");

                EntityType entityType = await _entityTypeRepo.Get(id);

                if (entityType == null) throw new NullReferenceException("No Data");

                return TransferViewEntityType(entityType);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ViewEntityType> Insert(string name, string token)
        {
            try
            {

                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name is NULL");

                EntityType entityType = new EntityType();
                entityType.Name = name;
                int result = await _entityTypeRepo.Insert(entityType);
                if (result <= 0) throw new Exception("Add Failed");
                return TransferViewEntityType(entityType);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Update(ViewEntityType model, string token)
        {
            try
            {
                if (model.Id == 0 || string.IsNullOrEmpty(model.Name)) throw new ArgumentNullException("Id is NULL || Name is NULL");

                //Check
                EntityType entityType = await _entityTypeRepo.Get(model.Id);
                if (entityType == null) throw new Exception("Entity Type not found !");

                entityType.Name = model.Name;
                await _entityTypeRepo.Update();
                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        private ViewEntityType TransferViewEntityType(EntityType entityType)
        {
            return new ViewEntityType()
            {
                Id = entityType.Id,
                Name = entityType.Name,
            };
        }
    }
}
