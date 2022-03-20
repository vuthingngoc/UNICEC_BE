using System;
using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.ImplRepo.BlogTypeRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.BlogType;

namespace UNICS.Business.Services.BlogTypeSvc
{
    public class BlogTypeService : IBlogTypeService
    {
        private IBlogTypeRepo _blogTypeRepo;

        public BlogTypeService(IBlogTypeRepo blogTypeRepo)
        {
            _blogTypeRepo = blogTypeRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<BlogType>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BlogType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewBlogType> Insert(BlogTypeInsertModel blogType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewBlogType blogType)
        {
            throw new NotImplementedException();
        }
    }
}
