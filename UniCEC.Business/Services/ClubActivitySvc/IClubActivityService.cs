using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public interface IClubActivityService
    {
        public Task<ViewClubActivity> GetByClubActivityId(int id);
        public Task<ViewClubActivity> Insert(ClubActivityInsertModel clubActivity);
        public Task<bool> Update(ClubActivityUpdateModel clubActivity);
        public Task<bool> Delete(int id);

        //Get List ClubActivity By Conditions
        public Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions);
    }
}
