using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public interface IClubHistoryService
    {
        public Task<PagingResult<ViewClubHistory>> GetAllPaging(PagingRequest request);
        public Task<ViewClubHistory> GetByClubHistory(int id);
        public Task<PagingResult<ViewClubHistory>> GetByContitions(ClubHistoryRequestModel request);
        //public Task<ViewClubMember> GetMemberByClub(int clubId, int termId);
        public Task<ViewClubHistory> Insert(ClubHistoryInsertModel clubHistory);
        public Task Update(ClubHistoryUpdateModel clubHistory);
        public Task Delete(int memberId);
    }
}
