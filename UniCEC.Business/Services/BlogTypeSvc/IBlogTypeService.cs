using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.BlogType;

namespace UniCEC.Business.Services.BlogTypeSvc
{
    public interface IBlogTypeService
    {
        Task<PagingResult<BlogType>> GetAll(PagingRequest request);
        Task<BlogType> GetById(int id);
        Task<ViewBlogType> Insert(BlogTypeInsertModel blogType);
        Task<bool> Update(ViewBlogType blogType);
        Task<bool> Delete(int id);

    }
}
