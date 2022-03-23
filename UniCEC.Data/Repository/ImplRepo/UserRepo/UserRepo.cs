using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.UserRepo
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(UniCECContext context) : base(context)
        {

        }
    }
}
