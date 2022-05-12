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

        public async Task<bool> CheckSponsorIsCreated(int UserId)
        {
            bool result = false;
            var query = from sponsor in context.Sponsors
                        where sponsor.UserId == UserId
                        select sponsor;

            Sponsor sp = await query.FirstOrDefaultAsync();
            if (sp != null)
            {
                result = true;
                
            }
            return result;
        }
    }
}
