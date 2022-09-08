using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.MatchTypeRepo
{
    public class MatchTypeRepo : Repository<MatchType>, IMatchTypeRepo
    {
        public MatchTypeRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<bool> CheckExistedType(int typeId)
        {
            return await context.MatchTypes.FirstOrDefaultAsync(type => type.Id.Equals(typeId)) != null;
        }
    }
}
