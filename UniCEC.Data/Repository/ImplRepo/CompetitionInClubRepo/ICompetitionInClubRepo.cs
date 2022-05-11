using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public interface ICompetitionInClubRepo : IRepository<CompetitionInClub>
    {
        //check-club-id-create-competition-duplicate
        public Task<bool> CheckDuplicateCreateCompetition(int clubId, int competitionId);

        // Nhat
        public Task<int> GetTotalEventOrganizedByClub(int clubId);
    }
}
