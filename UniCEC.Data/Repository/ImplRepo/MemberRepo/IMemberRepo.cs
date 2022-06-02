using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public interface IMemberRepo : IRepository<Member>
    {
        public Task<PagingResult<ViewMember>> GetMembersByClub(int clubId, int? termId, MemberStatus? status, PagingRequest request);
        public Task<ViewBasicInfoMember> GetBasicInfoMember(GetMemberInClubModel model);
        public Task<ViewDetailMember> GetById(int memberId);
        public Task<List<ViewMember>> GetLeadersByClub(int clubId);
        public Task<int> GetQuantityNewMembersByClub(int clubId);
        public Task<bool> CheckExistedMemberInClub(int userId, int clubId);
        public Task<int> GetRoleMemberInClub(int userId, int clubId);
    }
}
