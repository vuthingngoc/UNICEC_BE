using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Blog;

namespace UNICS.Business.Services.BlogSvc
{
    public interface IBlogSvc
    {
        Task<PagingResult<ViewBlog>> GetAll(PagingRequest request);
        Task<ViewBlog> GetById(int id);
        Task<bool> Insert();
        Task<bool> Update();
        Task<bool> Delete(int id);

    }
}
