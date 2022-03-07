using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.VideoRepo
{
    public class VideoRepo : Repository<Video>, IVideoRepo
    {
        public VideoRepo(UNICSContext context) : base(context)
        {

        }
    }
}
