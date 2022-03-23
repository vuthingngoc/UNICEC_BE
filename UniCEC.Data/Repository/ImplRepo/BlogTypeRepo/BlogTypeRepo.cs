using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.BlogTypeRepo
{
    public class BlogTypeRepo : Repository<BlogType>, IBlogTypeRepo
    {
        public BlogTypeRepo(UniCECContext context) : base(context)
        {

        }
    }
}
