using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.RoleRepo
{
    public class RoleRepo : Repository<Role>, IRoleRepo
    {
        public RoleRepo(UNICSContext context) : base(context)
        {

        }
    }
}
