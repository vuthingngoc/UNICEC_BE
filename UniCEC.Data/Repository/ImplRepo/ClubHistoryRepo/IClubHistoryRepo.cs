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
        Task<PagingResult<ClubHistory>> GetByConditions(ClubHistoryRequestModel request);
        Task<List<int>> GetIdsByMember(int memberID);
        Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request);
        Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId);

    }
}
