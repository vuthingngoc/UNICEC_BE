using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public interface IMemberService
    {
        public Task<PagingResult<ViewMember>> GetByClub(string token, int clubId, PagingRequest request);
        public Task<ViewDetailMember> GetByMemberId(string token, int id, int clubId);
        public Task<List<ViewMember>> GetLeadersByClub(int clubId);
        public Task<int> GetQuantityNewMembersByClub(string token, int clubId);
        public Task<ViewDetailMember> Insert(string token, MemberInsertModel member);
        public Task Update(string token, MemberUpdateModel member);
        public Task Delete(string token, int clubId, int id);
    }
}
