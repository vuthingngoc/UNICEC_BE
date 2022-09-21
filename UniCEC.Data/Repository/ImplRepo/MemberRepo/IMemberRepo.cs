using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public interface IMemberRepo : IRepository<Member>
    {
        public Task<PagingResult<ViewMember>> GetMembersByClub(int clubId, MemberStatus status, PagingRequest request);
        public Task<List<Member>> GetMembersByClub(int clubId, string searchName, int? roleId);
        public Task<List<ViewDetailMember>> GetMemberInfoByClub(int userId, int? clubId);
        public Task<PagingResult<ViewMember>> GetByConditions(MemberRequestModel request);
        public Task<ViewDetailMember> GetDetailById(int memberId, MemberStatus? status);
        public Task<ViewMember> GetById(int memberId);
        public Task<int> GetClubIdByMember(int memberId); // use to check role
        public Task<List<ViewIntroClubMember>> GetLeadersByClub(int clubId);
        public Task<int> GetQuantityNewMembersByClub(int clubId);
        public Task<int> GetTotalMembersByClub(int clubId);
        public Task<bool> CheckExistedMemberInClub(int userId, int clubId);   
        public Task<int> GetRoleMemberInClub(int userId, int clubId);
        public Task<int> GetIdByUser(int userId, int clubId);
        public int CheckValidLeader(int userId, int universityId); // use when insert club
        public Task UpdateMemberRole(int memberId, int clubRoleId);
        //public Task DeleteMember(int memberId);
        public Task UpdateStatusDeletedClub(int clubId);
        public Task<List<int>> GetUserIdsByMembers(List<int> memberIds);
        //TA
        public Task<ViewBasicInfoMember> GetBasicInfoMember(GetMemberInClubModel model);      
        public Task<Member> IsMemberInListClubCompetition(List<int> List_ClubId_In_Competition, User studentInfo);
        public Task<Member> GetLeaderByClub(int clubId);
        public Task<Member> GetLeaderClubOwnerByCompetition(int competitionId);
        public Task<bool> CheckExistedMemberInClubWhenInsert(int userId, int clubId);
        public Task<int> GetIdByUserWhenInsert(int userId, int clubId); 
    }
}
