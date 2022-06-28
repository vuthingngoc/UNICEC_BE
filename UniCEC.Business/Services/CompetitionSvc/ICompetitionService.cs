using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {      
        

        
        public Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel competition, string token);
       
        public Task<bool> LeaderDelete(LeaderDeleteCompOrEventModel model, string token);

        //public Task<bool> LeaderDeleteSponsorInCompetition(SponsorInCompetitionDeleteModel model, string token);
        
        //FE-Mobile use
        public Task<bool> LeaderUpdate(LeaderUpdateCompOrEventModel competition, string token);
        //BE  use to test
        public Task<bool> UpdateBE(LeaderUpdateCompOrEventModel competition, string token);
            
        //public Task<List<ViewCompetitionInMajor>> AddCompetitionInDepartment(CompetitionInMajorInsertModel model, string token);
        public Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token);

        //--------------------------------------------------Influencer
        //public Task<List<ViewInfluencerInCompetition>> AddInfluencerInCompetition(InfluencerInComeptitionInsertModel model, string token);
        //public Task<bool> DeleteInluencerInCompetition(InfluencerInCompetitionDeleteModel model, string token);

        //--------------------------------------------------Member In Competition
        public Task<PagingResult<ViewMemberInCompetition>> GetAllManagerCompOrEve(MemberInCompetitionRequestModel request, string token);

        public Task<ViewMemberInCompetition> AddMemberInCompetition(MemberInCompetitionInsertModel model, string token);

        public Task<bool> UpdateMemberInCompetition(MemberInCompetitionUpdateModel model, string token);


        //-----------------------------------------------------Get       
        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request);
        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTop3CompOrEve(int? clubId, bool? Event, CompetitionStatus? status, CompetitionScopeStatus? scope);
             
        //Get
        public Task<ViewDetailCompetition> GetById(int CompetitionId);
        


        //Comment
        //Competition - Manager
        //Get All Manager in competition manager
        //public Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel model, string token);
        //public Task<ViewCompetitionManager> AddMemberInCompetitionManager(CompetitionManagerInsertModel model, string token);
        //public Task<bool> UpdateMemberInCompetitionManager(CompetitionManagerUpdateModel model, string token);

        //Competition - Entity
        //public Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token);
    }
}
