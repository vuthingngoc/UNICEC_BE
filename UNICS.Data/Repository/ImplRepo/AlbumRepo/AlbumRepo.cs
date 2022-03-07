using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.AlbumRepo
{
    public class AlbumRepo : Repository<Album>, IAlbumRepo
    {
        public AlbumRepo(UNICSContext context) : base(context)
        {

        }
    }
}
