using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubRoleRepo
{
    public class ClubRoleRepo : Repository<ClubRole>, IClubRoleRepo
    {
        public ClubRoleRepo(UniCECContext context) : base(context)
        {

        }
    }
}
