using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ClubPreviousRepo 
{
    public class ClubPreviousRepo : Repository<ClubPreviou>, IClubPreviousRepo 
    {
        public ClubPreviousRepo(UNICSContext context) : base(context) {

        }
    }
}
