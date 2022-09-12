using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public interface IMemberService
    {
        public Task<PagingResult<ViewMember>> GetByClub(string token, int clubId, MemberStatus? status, PagingRequest request);
        public Task<List<ViewDetailMember>> GetMemberInfoByClub(string token, int? clubId);
        public Task<PagingResult<ViewMember>> GetByConditions(string token, MemberRequestModel request);
        public Task<ViewDetailMember> GetByMemberId(string token, int id);
        public Task<List<ViewIntroClubMember>> GetLeadersByClub(string token, int clubId);
        public Task<int> GetQuantityNewMembersByClub(string token, int clubId);
        public Task<ViewMember> Insert(string token, int clubId);
        public Task ConfirmMember(string token, ConfirmMemberModel model);
        public Task Update(string token, MemberUpdateModel member);
        public Task Delete(string token, int id);
        //TA
        public Task<List<ViewMember>> GetMembersByClub(string token, int clubId, string searchName, int? RoleId);

    }
}
