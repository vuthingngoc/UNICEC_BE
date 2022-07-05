using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
{
    public interface IMemberTakesActivityService
    {
        
              
        //public Task<bool> ApprovedOrRejectedTask(ConfirmMemberTakesActivity model, string token);

        //----- Refactor code
        //public Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksByConditions(MemberTakesActivityRequestModel request, string token);

        //public Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksMemberByConditions(MemberTakesActivityRequestModel request, string token);

        //public Task<ViewDetailMemberTakesActivity> GetByMemberTakesActivityId(int memberTakesActivityId, int clubId, string token);

        //public Task<ViewDetailMemberTakesActivity> Insert(MemberTakesActivityInsertModel memberTakesActivity, string token);

        //public Task<bool> RemoveMemberTakeActivity(RemoveMemberTakeActivityModel model, string token);

        //public Task<bool> SubmitTask(SubmitMemberTakesActivity model, string token);
    }
}
