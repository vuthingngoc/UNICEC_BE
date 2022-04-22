using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.Business.Services.ClubPreviousSvc
{
    public interface IClubPreviousService
    {
        public Task<PagingResult<ViewClubPrevious>> GetAllPaging(PagingRequest request);
        public Task<ViewClubPrevious> GetByClubPrevious(int id);
        public Task<PagingResult<ViewClubPrevious>> GetByContitions(ClubPreviousRequestModel request);
        public Task<ViewClubPrevious> Insert(ClubPreviousInsertModel clubPrevious);
        public Task Update(ClubPreviousUpdateModel clubPrevious);
        public Task Delete(int id);
    }
}
