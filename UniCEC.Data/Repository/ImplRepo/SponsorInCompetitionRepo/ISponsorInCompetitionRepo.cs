using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public interface ISponsorInCompetitionRepo : IRepository<SponsorInCompetition>
    {
        //check-sponsor-id-create-competition-or-event-duplicate
        public Task<bool> CheckDuplicateCreateCompetitionOrEvent(int sponsorId, int competitionId);
    }
}
