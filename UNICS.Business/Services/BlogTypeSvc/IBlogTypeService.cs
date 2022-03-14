using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.BlogType;

namespace UNICS.Business.Services.BlogTypeSvc
{
    public interface IBlogTypeService
    {
        Task<PagingResult<BlogType>> GetAll(PagingRequest request);
        Task<BlogType> GetById(int id);
        Task<bool> Insert(BlogTypeInsertModel blogType);
        Task<bool> Update(ViewBlogType blogType);
        Task<bool> Delete(int id);

    }
}
