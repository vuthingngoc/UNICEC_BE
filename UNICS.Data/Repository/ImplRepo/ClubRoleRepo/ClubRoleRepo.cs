using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ClubRoleRepo 
{
    public class ClubRoleRepo : Repository<ClubRole>, IClubRoleRepo 
    {
        public ClubRoleRepo(UNICSContext context) : base(context) 
        {

        }
    }
}
