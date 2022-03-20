using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ClubPrevious;

namespace UNICS.Business.Services.ClubPreviousSvc
{
    public interface IClubPreviousService
    {
        public Task<PagingResult<ViewClubPrevious>> GetAll(PagingRequest request);
        public Task<ViewClubPrevious> GetByClubPreviousId(int id);
        public Task<ViewClubPrevious> Insert(ClubPreviousInsertModel clubPrevious);
        public Task<bool> Update(ClubPreviousUpdateModel clubPrevious);
        public Task<bool> Delete(int id);
    }
}
