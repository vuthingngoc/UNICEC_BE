using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.EntityTypeRepo
{
    public class EntityTypeRepo : Repository<EntityType>, IEntityTypeRepo
    {
        public EntityTypeRepo(UniCECContext context) : base(context)
        {

        }
    }
}
