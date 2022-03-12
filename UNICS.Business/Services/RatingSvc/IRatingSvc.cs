using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Rating;

namespace UNICS.Business.Services.RatingSvc
{
    public interface IRatingSvc
    {
        Task<PagingResult<ViewRating>> GetAll(PagingRequest request);
        Task<ViewRating> GetById(int id);
        Task<bool> Insert(RatingInsertModel rating);
        Task<bool> Update(ViewRating rating);
        Task<bool> Delete(int id);
    }
}
