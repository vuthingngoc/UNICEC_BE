using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.BlogRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Blog;

namespace UNICS.Business.Services.BlogSvc
{
    public class BlogService : IBlogService
    {
        private IBlogRepo _blogRepo;

        public BlogService(IBlogRepo blogRepo)
        {
            _blogRepo = blogRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewBlog>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewBlog> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewBlog> Insert(BlogInsertModel blog)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BlogUpdateModel blog)
        {
            throw new NotImplementedException();
        }
    }
}
