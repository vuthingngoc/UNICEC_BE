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
       
        public Task<ViewDetailCompetition> InsertCompetitionOrEvent(LeaderInsertCompOrEventModel competition, string token);
       
        public Task<bool> DeleteCompetitionOrEvent(LeaderDeleteCompOrEventModel model, string token);

        //----------------------------Update
        //FE-Mobile use

        //State Draft
        public Task<bool> UpdateCompetitionOrEvent(LeaderUpdateCompOrEventModel competition, string token);

        //State Approve -> Pendding Review có comment
        public Task<bool> UpdateConstraintBeforePublish(UpdateConstraintBeforePublishModel model, string token);

        //State Not Change
        public Task<bool> UpdateBeforePublish(UpdateBeforePublishModel model, string token);

        //State Publish - Register - UpComing
        public Task<bool> UpdateBeforeCeremonyTime(UpdateBeforeCeremonyModel model, string token);


        //BE  use to test
        public Task<bool> UpdateBE(LeaderUpdateCompOrEventModel competition, string token);

        //--------------------------------------------------Club Collaborate
        public Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token);

        //--------------------------------------------------Competition In Major

        //Add
        //public Task<List<ViewCompetitionInMajor>> AddCompetitionInDepartment(CompetitionInMajorInsertModel model, string token);
        //Delete


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
      
    }
}
