using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MemberTakesActivity;

namespace UNICS.Business.Services.MemberTakesActivitySvc
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
