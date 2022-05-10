using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public interface IMemberService
    {
        public Task<PagingResult<ViewMember>> GetAllPaging(int clubId, PagingRequest request);
        public Task<ViewMember> GetByMemberId(int id);
        public Task<List<ViewMember>> GetLeadersByClub(int clubId);
        public Task<int> GetQuantityNewMembersByClub(int clubId);
        public Task<ViewMember> Insert(MemberInsertModel member);
        public Task Update(MemberUpdateModel member);
        public Task Delete(int id);
    }
}
