using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.EntityType;

namespace UniCEC.Data.Repository.ImplRepo.EntityTypeRepo
{
    public class EntityTypeRepo : Repository<EntityType>, IEntityTypeRepo
    {
        public EntityTypeRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<ViewEntityType>> GetAll()
        {
            List<ViewEntityType> query = await (from e in context.EntityTypes
                                            select new ViewEntityType()
                                            {
                                                Id = e.Id,
                                                Name = e.Name,  
                                            }).ToListAsync();

            return (query.Count > 0) ? query: null;
          
        }
    }
}
