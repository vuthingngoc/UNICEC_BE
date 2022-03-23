using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
{
    public interface IMemberTakesActivityService
    {
        public Task<PagingResult<ViewMemberTakesActivity>> GetAll(PagingRequest request);
        public Task<ViewMemberTakesActivity> GetByMemberTakesActivityId(int id);
        public Task<ViewMemberTakesActivity> Insert(MemberTakesActivityInsertModel memberTakesActivity);
        public Task<bool> Update(ViewMemberTakesActivity memberTakesActivity);
        public Task<bool> Delete(int id);
    }
}
