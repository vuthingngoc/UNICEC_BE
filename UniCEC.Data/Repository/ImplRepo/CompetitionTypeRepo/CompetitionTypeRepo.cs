using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo
{
    public class CompetitionTypeRepo : Repository<CompetitionType>, ICompetitionTypeRepo
    {
        public CompetitionTypeRepo(UniCECContext context) : base(context)
        {

        }
    }
}
