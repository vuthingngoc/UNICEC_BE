using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public class SponsorInCompetitionRepo : Repository<SponsorInCompetition>, ISponsorInCompetitionRepo
    {
        public SponsorInCompetitionRepo(UNICSContext context) : base(context)
        {

        }
    }
}
