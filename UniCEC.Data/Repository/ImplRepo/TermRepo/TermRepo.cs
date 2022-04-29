using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TermRepo
{
    public class TermRepo : Repository<Term>, ITermRepo
    {
        public TermRepo(UniCECContext context) : base(context)
        {

        }


    }
}
