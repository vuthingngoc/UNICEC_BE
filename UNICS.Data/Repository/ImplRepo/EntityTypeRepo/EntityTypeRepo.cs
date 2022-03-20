using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.EntityTypeRepo
{
    public class EntityTypeRepo : Repository<EntityType>, IEntityTypeRepo
    {
        public EntityTypeRepo(UNICSContext context) : base(context)
        {

        }
    }
}
