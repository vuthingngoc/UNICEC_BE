using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo
{
    public interface ICompetitionHistoryRepo : IRepository<CompetitionHistory>
    {
        public Task<List<ViewCompetitionHistory>> GetAllHistoryOfCompetition(int competitionId);

        public Task<CompetitionHistory> GetNearestStateAfterPending(int competitionId);
    }
}
