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
        public Task<ViewDetailCompetitionActivity> GetCompetitionActivityById(int id, int clubId, string token);
        public Task<ViewDetailCompetitionActivity> Insert(CompetitionActivityInsertModel clubActivity, string token);
        public Task<bool> Update(CompetitionActivityUpdateModel clubActivity, string token);
        public Task<bool> Delete(CompetitionActivityDeleteModel model, string token);
        //Get List ClubActivity By Conditions
        public Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions, string token);

        //public Task<List<ViewProcessCompetitionActivity>> GetTop3TasksOfCompetition(int clubId, string token);

        //------------------------------------Member Take Activity
        public Task<ViewDetailMemberTakesActivity> AssignTaskForMember(MemberTakesActivityInsertModel model, string token);

        public Task<bool> MemberUpdateStatusTask(MemberUpdateStatusTaskModel model, string token);
    }
}
