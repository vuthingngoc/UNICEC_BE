using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public interface ISponsorInCompetitionRepo : IRepository<SponsorInCompetition>
    {
        //check-sponsor-id-create-competition-or-event-duplicate
        public Task<SponsorInCompetition> CheckSponsorInCompetition(int sponsorId, int competitionId, int userId);
        
        //retrun View Detail
        public Task<List<ViewSponsorInComp>> GetListSponsor_In_Competition(int competitionId);
        //get all
        public Task<PagingResult<ViewSponsorInCompetition>> GetListSponsor_In_Competition(SponsorApplyRequestModel request);
        public Task DeleteSponsorInCompetition(int sponsorInCompetitionId);
    }
}
