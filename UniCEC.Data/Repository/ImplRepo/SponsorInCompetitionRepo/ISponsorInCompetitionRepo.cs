using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public interface ISponsorInCompetitionRepo : IRepository<SponsorInCompetition>
    {
        //check-sponsor-id-create-competition-or-event-duplicate
        public Task<SponsorInCompetition> CheckSponsorInCompetition(int sponsorId, int competitionId);
        //
        public Task<List<ViewSponsorInComp>> GetListSponsor_In_Competition(int competitionId);
        //
        public Task DeleteSponsorInCompetition(int sponsorInCompetitionId);
    }
}
