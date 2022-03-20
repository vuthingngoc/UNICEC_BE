using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ClubActivity;

namespace UNICS.Business.Services.ClubActivitySvc
{
    public interface IClubActivityService
    {
        public Task<PagingResult<ViewClubActivity>> GetAll(PagingRequest request);
        public Task<ViewClubActivity> GetByClubActivityId(int id);
        public Task<ViewClubActivity> Insert(ClubActivityInsertModel clubActivity);
        public Task<bool> Update(ClubActivityUpdateModel clubActivity);
        public Task<bool> Delete(int id);
    }
}
