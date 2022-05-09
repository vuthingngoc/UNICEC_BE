using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo
{
    public interface IClubHistoryRepo : IRepository<ClubHistory>
    {
        public Task<PagingResult<ViewClubHistory>> GetAll(int clubId, PagingRequest request);
        public Task<ViewClubHistory> GetById(int id);
        public Task<PagingResult<ViewClubHistory>> GetByConditions(ClubHistoryRequestModel request);
        public Task<List<int>> GetIdsByMember(int memberId);
        public Task<int> GetCurrentTermByClub(int clubId);
        public Task<bool> UpdateMemberRole(int memberId, int clubRoleId);
        public Task<bool> DeleteMember(int memberId);

        // Tien Anh 
        public Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request);
        public Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId);

        //check mem in club
        public Task<bool> CheckMemberInClub(int clubId, int memberId, int termId);

        //get infomation of mem in club
        public Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model);
    }
}
