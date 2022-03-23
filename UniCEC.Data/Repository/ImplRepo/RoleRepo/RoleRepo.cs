using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.RoleRepo
{
    public class RoleRepo : Repository<Role>, IRoleRepo
    {
        public RoleRepo(UniCECContext context) : base(context)
        {

        }
    }
}
