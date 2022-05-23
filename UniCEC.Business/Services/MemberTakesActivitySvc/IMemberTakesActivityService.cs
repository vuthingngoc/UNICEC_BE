using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
{
    public interface IMemberTakesActivityService
    {
        public Task<PagingResult<ViewMemberTakesActivity>> GetAllPaging(PagingRequest request);
        public Task<ViewMemberTakesActivity> GetByMemberTakesActivityId(int id);
        public Task<ViewMemberTakesActivity> Insert(MemberTakesActivityInsertModel memberTakesActivity, string token);

        //------------------------------------------------sau này sẽ có comment kèm theo
        public Task<bool> Update(SubmitMemberTakesActivity model, string token);
        public Task<bool> ApprovedOrRejectedTask(ConfirmMemberTakesActivity model, string token);

        public Task<PagingResult<ViewMemberTakesActivity>> GetAllTaskesByConditions(MemberTakesActivityRequestModel request);

    }
}
