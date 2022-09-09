using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.EntityType;

namespace UniCEC.Business.Services.EntityTypeSvc
{
    public interface IEntityTypeService
    {
        public Task<List<ViewEntityType>> GetAllEntityType(string token);

        public Task<ViewEntityType> GetEntityTypeById(int id , string token);

        public Task<ViewEntityType> Insert(string name, string token);

        public Task<bool> Update(ViewEntityType model, string token);


    }
}
