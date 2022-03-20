using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Blog;

namespace UNICS.Business.Services.BlogSvc
{
    public interface IBlogService
    {
        Task<PagingResult<ViewBlog>> GetAll(PagingRequest request);
        Task<ViewBlog> GetById(int id);
        Task<ViewBlog> Insert(BlogInsertModel blog);
        Task<bool> Update(BlogUpdateModel blog);
        Task<bool> Delete(int id);

    }
}
