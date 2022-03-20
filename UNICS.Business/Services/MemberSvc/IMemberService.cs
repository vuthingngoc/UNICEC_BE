using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Member;

namespace UNICS.Business.Services.MemberSvc
{
    public interface IMemberService
    {
        public Task<PagingResult<ViewMember>> GetAll(PagingRequest request);
        public Task<ViewMember> GetByMemberId(int id);
        public Task<ViewMember> Insert(MemberInsertModel member);
        public Task<bool> Update(MemberUpdateModel member);
        public Task<bool> Delete(int id);
    }
}
