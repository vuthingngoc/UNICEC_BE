using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.RatingRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Rating;

namespace UNICS.Business.Services.RatingSvc
{
    public class RatingSvc : IRatingSvc
    {
        private IRatingRepo _ratingRepo;

        public RatingSvc(IRatingRepo ratingRepo)
        {
            _ratingRepo = ratingRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewRating>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewRating> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(RatingInsertModel rating)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewRating rating)
        {
            throw new NotImplementedException();
        }
    }
}
