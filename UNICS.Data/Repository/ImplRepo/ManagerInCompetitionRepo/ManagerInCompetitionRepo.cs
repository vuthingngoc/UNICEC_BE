using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ManagerInCompetitionRepo
{
    public class ManagerInCompetitionRepo : Repository<ManagerInCompetition>, IManagerInCompetitionRepo
    {
        public ManagerInCompetitionRepo(UNICSContext context) : base(context)
        {

        }
    }
}
