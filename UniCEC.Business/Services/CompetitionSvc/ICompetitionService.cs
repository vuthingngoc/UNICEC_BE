using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionInMajor;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {
        //--------------------------------------------------UnAuthorize
        public Task<PagingResult<ViewCompetition>> GetCompOrEveUnAuthorize(CompetitionUnAuthorizeRequestModel request);

        //--------------------------------------------------Admin University
        //Admin University get Competition With State Pending Review
        public Task<PagingResult<ViewCompetition>> GetCompetitionByAdminUni(AdminUniGetCompetitionRequestModel request, string token);

        //Admin University change state Approve or Draft
        public Task<bool> ChangeStateByAdminUni(AdminUniUpdateCompetitionStatusModel model, string token);

        //---------------------------------------------------CRUD-Competition
        public Task<ViewDetailCompetition> InsertCompetitionOrEvent(LeaderInsertCompOrEventModel competition, string token);

        //update Status
        public Task<bool> CompetitionStatusUpdate(CompetitionStatusUpdateModel model, string token);
        public Task<bool> UpdateCompetitionByState(LeaderUpdateCompOrEventModel model, string token);
        public Task<bool> CompetitionUpdateStatusAfterPending(UpdateCompetitionWithStatePendingModel model, string token);


        //BE  use to test
        public Task<bool> UpdateBE(LeaderUpdateCompOrEventModel competition, string token);


        //--------------------------------------------------Club Collaborate
        public Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token);

        public Task<bool> DeleteClubCollaborate(int competitionInClubId, int clubId, string token);

        //--------------------------------------------------Competition In Major  
        public Task<List<ViewCompetitionInMajor>> AddCompetitionInMajor(CompetitionInMajorInsertModel model, string token);

        public Task<bool> DeleteMajorInCompetition(CompetitionInMajorDeleteModel model, string token);

        //--------------------------------------------------Member In Competition
        public Task<PagingResult<ViewMemberInCompetition>> GetAllManagerCompOrEve(MemberInCompetitionRequestModel request, string token);

        public Task<ViewMemberInCompetition> AddMemberInCompetition(MemberInCompetitionInsertModel model, string token);

        public Task<bool> UpdateMemberInCompetition(MemberInCompetitionUpdateModel model, string token);


        //-----------------------------------------------------Get       
        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request, string token);

        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewTopCompetition>> GetTopCompOrEve(int clubId, bool? Event/*, CompetitionStatus? status*/, CompetitionScopeStatus? scope, int top);

        //Get
        public Task<ViewDetailCompetition> GetById(int CompetitionId);

        //Get Competition that Student join
        public Task<PagingResult<ViewCompetition>> GetCompsOrEvesStudentJoin(GetStudentJoinCompOrEve request,string token);
        public Task<ViewCompetition> GetCompOrEveStudentJoin(int competitionId, string token);

        //Get Competition that Student is Assign Task
        public Task<PagingResult<ViewCompetition>> GetCompOrEveStudentIsAssignedTask(PagingRequest request, int clubId, string searchName, bool? isEvent,string token);

        public Task<ViewProcessCompetitionOrEventOfClub> GetNumberOfCompetitionOrEventInClubWithStatus(int clubId, string token);


        ////-----------------------------------------State Draft
        //public Task<bool> UpdateCompetitionOrEvent(LeaderUpdateCompOrEventModel competition, string token);

        ////-----------------------------------------State Approve 
        //public Task<bool> UpdateConstraintCompetitionWithStateApprove(UpdateConstraintCompetitionWithStateApproveModel model, string token);     
        //public Task<bool> UpdateCompetitionWithStateApprove(UpdateCompetitionWithStateApproveModel model, string token);

        ////-----------------------------------------State Publish - Register - UpComing
        //public Task<bool> UpdateBeforeStateCeremonyTime(UpdateBeforeStateCeremonyModel model, string token);

        ////-----------------------------------------State Pending
        //public Task<bool> UpdateCompetitionWithStatePending(UpdateCompetitionWithStatePendingModel model, string token);

    }
}
