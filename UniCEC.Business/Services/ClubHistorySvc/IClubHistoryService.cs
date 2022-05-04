using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public interface IClubHistoryService
    {
        public Task<PagingResult<ViewClubHistory>> GetAllPaging(int clubId, string token, PagingRequest request);
        public Task<ViewClubHistory> GetByClubHistory(int id);
        public Task<PagingResult<ViewClubHistory>> GetByContitions(ClubHistoryRequestModel request);
        public Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request);
        public Task<ViewClubHistory> Insert(ClubHistoryInsertModel clubHistory);
        public Task Update(ClubHistoryUpdateModel clubHistory);
        public Task Delete(int memberId);
        public Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model);
    }
}
