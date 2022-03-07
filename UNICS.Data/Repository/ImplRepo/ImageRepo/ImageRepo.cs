using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.ImageRepo
{
    public class ImageRepo : Repository<Image>, IImageRepo
    {
        public ImageRepo(UNICSContext context) : base(context)
        {

        }
    }
}
