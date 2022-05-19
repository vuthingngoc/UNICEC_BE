using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public interface IClubActivityService
    {
        public Task<ViewClubActivity> GetByClubActivityId(int id);
        public Task<ViewClubActivity> Insert(ClubActivityInsertModel clubActivity, string token);
        public Task<bool> Update(ClubActivityUpdateModel clubActivity, string token);
        public Task<bool> Delete(ClubActivityDeleteModel model, string token);
        //Get List ClubActivity By Conditions
        public Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions);

        public Task<List<ViewProcessClubActivity>> GetTop4_Process(int clubId, string token);

    }
}
