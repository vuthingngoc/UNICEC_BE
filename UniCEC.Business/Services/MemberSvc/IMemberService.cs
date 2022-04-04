using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public interface IMemberService
    {
        public Task<PagingResult<ViewMember>> GetAllPaging(PagingRequest request);
        public Task<ViewMember> GetByMemberId(int id);
        public Task<ViewMember> Insert(MemberInsertModel member);
        public Task<bool> Update(MemberUpdateModel member);
        public Task<bool> Delete(int id);
    }
}
