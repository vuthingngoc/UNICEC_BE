using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.Business.Services.ClubPreviousSvc
{
    public interface IClubPreviousService
    {
        public Task<PagingResult<ViewClubPrevious>> GetAllPaging(PagingRequest request);
        public Task<ViewClubPrevious> GetByClubPreviousId(int id);
        public Task<ViewClubPrevious> Insert(ClubPreviousInsertModel clubPrevious);
        public Task<bool> Update(ClubPreviousUpdateModel clubPrevious);
        public Task<bool> Delete(int id);
    }
}
