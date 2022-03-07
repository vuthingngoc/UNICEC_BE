using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.AlbumTypeRepo
{
    public class AlbumTypeRepo : Repository<AlbumType>, IAlbumTypeRepo
    {
        public AlbumTypeRepo(UNICSContext context) : base(context)
        {

        }
    }
}
