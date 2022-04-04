using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Blog;

namespace UniCEC.Business.Services.BlogSvc
{
    public interface IBlogService
    {
        Task<PagingResult<ViewBlog>> GetAllPaging(PagingRequest request);
        Task<ViewBlog> GetById(int id);
        Task<ViewBlog> Insert(BlogInsertModel blog);
        Task<bool> Update(BlogUpdateModel blog);
        Task<bool> Delete(int id);

    }
}
