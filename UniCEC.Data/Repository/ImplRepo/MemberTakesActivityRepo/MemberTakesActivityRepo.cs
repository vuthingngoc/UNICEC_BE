using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo
{
    public class MemberTakesActivityRepo : Repository<MemberTakesActivity>, IMemberTakesActivityRepo
    {
        public MemberTakesActivityRepo(UniCECContext context) : base(context)
        {

        }
    }
}
