using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.SponsorRepo
{
    public class SponsorRepo : Repository<Sponsor>, ISponsorRepo
    {
        public SponsorRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckSponsorIsCreated(string Email)
        {
            var query = from sp in context.Sponsors
                        where sp.Email == Email
                        select sp;

            Sponsor sponsor = await query.FirstOrDefaultAsync();
            if (sponsor == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
