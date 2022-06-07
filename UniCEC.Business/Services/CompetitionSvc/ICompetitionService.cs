using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionInDepartment;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;
using UniCEC.Data.ViewModels.Entities.Influencer;
using UniCEC.Data.ViewModels.Entities.InfluencerInComeptition;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {
        public Task<PagingResult<ViewCompetition>> GetAllPaging(PagingRequest request);
        public Task<ViewDetailCompetition> GetById(int id);

        //-----------------------------------------------------Leader 
        public Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel competition, string token);
        public Task<bool> LeaderUpdate(LeaderUpdateCompOrEventModel competition, string token);
        public Task<bool> LeaderDelete(LeaderDeleteCompOrEventModel model, string token);
        public Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token, IFormFile file);
        public Task<List<ViewCompetitionInDepartment>> AddCompetitionInDepartment(CompetitionInDepartmentInsertModel model, string token);
        public Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token);

        //Influencer
        public Task<List<ViewInfluencerInCompetition>> AddInfluencerInCompetition(InfluencerInComeptitionInsertModel model, string token);
        public Task<bool> DeleteInluencerInCompetition(InfluencerInCompetitionDeleteModel model, string token);

        //Competition - Manager
        public Task<ViewCompetitionManager> AddMemberInCompetitionManager(CompetitionManagerInsertModel model, string token);

        public Task<bool> UpdateMemberInCompetitionManager(CompetitionManagerUpdateModel model, string token);


        //-----------------------------------------------------Sponsor       
        public Task<ViewSponsorInCompetition> AddSponsorCollaborate(SponsorInCompetitionInsertModel sponsorInCompetition, string token);
        public Task<bool> SponsorDenyInCompetition(SponsorInCompetitionDeleteModel model, string token);


        //-----------------------------------------------------Get       
        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request);
        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTop3CompOrEve(int? clubId, bool? Event, CompetitionStatus? status, CompetitionScopeStatus? scope);
        //Get All Manager in competition manager
        public Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel model, string token);

    }
}
