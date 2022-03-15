using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CommentRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Comment;

namespace UNICS.Business.Services.CommentSvc
{
    public class CommentService : ICommentService
    {
        private ICommentRepo _commentRepo;

        public CommentService(ICommentRepo commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewComment>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewComment> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(CommentInsertModel comment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(CommentUpdateModel comment)
        {
            throw new NotImplementedException();
        }
    }
}
