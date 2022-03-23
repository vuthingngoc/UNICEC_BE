using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public class SponsorInCompetitionRepo : Repository<SponsorInCompetition>, ISponsorInCompetitionRepo
    {
        public SponsorInCompetitionRepo(UniCECContext context) : base(context)
        {

        }
    }
}
