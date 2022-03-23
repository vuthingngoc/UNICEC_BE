using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.BlogRepo
{
    public class BlogRepo : Repository<Blog>, IBlogRepo
    {
        public BlogRepo(UniCECContext context) : base(context)
        {

        }
    }
}
