using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.SponsorRepo
{
    public class SponsorRepo : Repository<Sponsor>, ISponsorRepo
    {
        public SponsorRepo(UniCECContext context) : base(context)
        {

        }
    }
}
