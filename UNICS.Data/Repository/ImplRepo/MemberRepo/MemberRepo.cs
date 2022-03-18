using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.MemberRepo {
    public class MemberRepo : Repository<Member>, IMemberRepo 
    {
        public MemberRepo(UNICSContext context) : base(context) {

        }
    }
}
