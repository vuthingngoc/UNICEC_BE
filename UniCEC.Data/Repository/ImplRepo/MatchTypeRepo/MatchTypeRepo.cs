using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.MatchTypeRepo
{
    public class MatchTypeRepo : Repository<MatchType>, IMatchTypeRepo
    {
        public MatchTypeRepo(UniCECContext context) : base(context)
        {
        }
    }
}
