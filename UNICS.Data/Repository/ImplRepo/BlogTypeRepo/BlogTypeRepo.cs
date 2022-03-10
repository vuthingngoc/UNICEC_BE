using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.BlogTypeRepo
{
    public class BlogTypeRepo : Repository<BlogType>, IBlogTypeRepo
    {
        public BlogTypeRepo(UNICSContext context) : base(context)
        {

        }
    }
}
