using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public interface IClubHistoryService
    {
        public Task<PagingResult<ViewClubHistory>> GetByContitions(string token, int clubId, ClubHistoryRequestModel request);
        public Task InsertForNewTerm(string token, int clubId, TermInsertModel termModel);

        // ???
        public Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request);

        // Tien Anh
        public Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model);
    }
}
