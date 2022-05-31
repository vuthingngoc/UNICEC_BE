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
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public interface ICompetitionService
    {
        public Task<PagingResult<ViewCompetition>> GetAllPaging(PagingRequest request);
        public Task<ViewDetailCompetition> GetById(int id);

        //-----------------------------------------------------Leader 
        public Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel competition, string token, IFormFile file);
        public Task<bool> LeaderUpdate(LeaderUpdateCompOrEventModel competition, string token);
        public Task<bool> LeaderDelete(LeaderDeleteCompOrEventModel model, string token);
        public Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token, IFormFile file);
        public Task<List<ViewCompetitionInDepartment>> AddCompetitionInDepartment(CompetitionInDepartmentInsertModel model, string token);
        public Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel competitionInClub, string token);


        //-----------------------------------------------------Sponsor 
        //public Task<ViewDetailCompetition> SponsorInsert(SponsorInsertCompOrEventModel competition, string token);
        //public Task<bool> SponsorUpdate(SponsorUpdateCompOrEvent competition, string token);
        //public Task<bool> SponsorDelete(SponsorDeleteCompOrEventModel model, string token);
        public Task<ViewSponsorInCompetition> AddSponsorCollaborate(SponsorInCompetitionInsertModel sponsorInCompetition, string token);

        //Get EVENT or COMPETITION by conditions
        public Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request);
        //Get top 3 EVENT or COMPETITION by Status
        public Task<List<ViewCompetition>> GetTop3CompOrEve(int? ClubId, bool? Event, CompetitionStatus? Status, bool? Public);

    }
}
