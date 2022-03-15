using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.BlogRepo
{
    public class BlogRepo : Repository<Blog>, IBlogRepo
    {
        public BlogRepo(UNICSContext context) : base(context)
        {

        }
    }
}
