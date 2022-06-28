using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.EntityType;

namespace UniCEC.Data.Repository.ImplRepo.EntityTypeRepo
{
    public interface IEntityTypeRepo : IRepository<EntityType>
    {
        public Task<List<ViewEntityType>> GetAll();
    }
}
