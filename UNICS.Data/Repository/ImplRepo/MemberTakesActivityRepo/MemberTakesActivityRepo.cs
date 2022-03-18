using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.MemberTakesActivityRepo {
    public class MemberTakesActivityRepo : Repository<MemberTakesActivity>, IMemberTakesActivityRepo 
    {
        public MemberTakesActivityRepo(UNICSContext context) : base(context) {

        }
    }
}
