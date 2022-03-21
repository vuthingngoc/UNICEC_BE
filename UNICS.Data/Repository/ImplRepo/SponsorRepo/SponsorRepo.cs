using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.SponsorRepo
{
    public class SponsorRepo : Repository<Sponsor>, ISponsorRepo
    {
        public SponsorRepo(UNICSContext context) : base(context)
        {

        }
    }
}
