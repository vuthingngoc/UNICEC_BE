using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.MajorInCompetitionRepo
{
    public class MajorInCompetitionRepo : Repository<MajorInCompetition>, IMajorInCompetitionRepo
    {
        public MajorInCompetitionRepo(UNICSContext context) : base(context)
        {

        }
    }
}
