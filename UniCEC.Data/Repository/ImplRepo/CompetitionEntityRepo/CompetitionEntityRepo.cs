using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo
{
    public class CompetitionEntityRepo : Repository<CompetitionEntity>, ICompetitionEntityRepo
    {
        public CompetitionEntityRepo(UniCECContext context) : base(context)
        {

        }
    }
}
