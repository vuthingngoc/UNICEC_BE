using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {
        public Task<PagingResult<ViewCompetition>> GetAllPaging(PagingRequest request);
        public Task<ViewCompetition> GetById(int id);
        //-----------------------------------------------------Leader 
        public Task<ViewCompetition> LeaderInsert(LeaderInsertCompOrEventModel competition);
        public Task<bool> LeaderUpdate(CompetitionUpdateModel competition);
        public Task<bool> LeaderDelete(CompetitionDeleteModel model);

        //-----------------------------------------------------Sponsor 
        public Task<ViewCompetition> SponsorInsert(SponsorInsertCompOrEventModel competition);//string Token);
        public Task<bool> SponsorUpdate(CompetitionUpdateModel competition);
        public Task<bool> SponsorDelete(CompetitionDeleteModel model);

        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request);
        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTop3CompOrEve(int? ClubId, bool? Event, CompetitionStatus? Status, bool? Public);

    }
}
