using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CommentRepo
{
    public class CommentRepo : Repository<Comment>, ICommentRepo
    {
        public CommentRepo(UNICSContext context) : base(context)
        {

        }
    }
}
