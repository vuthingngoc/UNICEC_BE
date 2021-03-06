using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.CompetitionActivitySvc
{
    public interface ICompetitionActivityService
    {
        public Task<List<ViewProcessCompetitionActivity>> GetTopTasksOfCompetition(int clubId, int topCompetition, int topCompetitionActivity, string token);

        public Task<ViewDetailCompetitionActivity> GetCompetitionActivityById(int id, int clubId, string token);
        public Task<ViewDetailCompetitionActivity> Insert(CompetitionActivityInsertModel clubActivity, string token);

        //Member In Competition duyệt bài
        public Task<bool> Update(CompetitionActivityUpdateModel clubActivity, string token);
        public Task<bool> Delete(CompetitionActivityDeleteModel model, string token);

        //Get List ClubActivity By Conditions 
        //-> sửa lại cho list Status
        public Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions, string token);
        
        //------------------------------------Member Take Activity
        public Task<ViewDetailMemberTakesActivity> AssignTaskForMember(MemberTakesActivityInsertModel model, string token);

        //Member Update Task Status
        public Task<bool> MemberUpdateStatusTask(MemberUpdateStatusTaskModel model, string token);

        //Remove member take task 
        public Task<bool> RemoveMemberTakeTask(RemoveMemberTakeActivityModel model, string token);


    }
}
