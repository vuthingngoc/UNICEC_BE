using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CompetitionTypeRepo
{
    public class CompetitionTypeRepo : Repository<CompetitionType>, ICompetitionTypeRepo
    {
        public CompetitionTypeRepo(UNICSContext context) : base(context)
        {

        }
    }
}
