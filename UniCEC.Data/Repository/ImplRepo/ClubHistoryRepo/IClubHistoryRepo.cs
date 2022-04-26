using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo
{
    public interface IClubHistoryRepo : IRepository<ClubHistory>
    {
        Task<PagingResult<ClubHistory>> GetByConditions(ClubHistoryRequestModel request);
        Task<List<int>> GetIdsByMember(int memberID);
        //Task<PagingResult<ViewClubMember>> GetMemberByClub(int clubId, int termId);
        Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId);

    }
}
