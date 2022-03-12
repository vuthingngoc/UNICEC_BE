using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Comment;

namespace UNICS.Business.Services.CommentSvc
{
    public interface ICommentSvc
    {
        Task<PagingResult<ViewComment>> GetAll(PagingRequest request);
        Task<ViewComment> GetById(int id);
        Task<bool> Insert(CommentInsertModel comment);
        Task<bool> Update(CommentUpdateModel comment);
        Task<bool> Delete(int id);
    }
}
