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
        public Task<ViewClubActivity> Insert(ClubActivityInsertModel clubActivity);
        public Task<bool> Update(ClubActivityUpdateModel clubActivity);
        public Task<bool> Delete(int id);
        //Get List ClubActivity By Conditions
        public Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions);
        //Get Top 4 club activity dựa trên ngày gần với hiện tại
        public Task<List<ViewClubActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId, DateTime createDate);
        //Get Process club Activity by clubActivity
        public Task<ViewProcessClubActivity> GetProcessClubActivity(int clubActivityId, MemberTakesActivityStatus status);

    }
}
