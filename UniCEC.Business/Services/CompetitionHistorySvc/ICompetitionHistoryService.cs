using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;

namespace UniCEC.Business.Services.CompetitionHistorySvc
{
    public interface ICompetitionHistoryService
    {
        public Task<List<ViewCompetitionHistory>> GetAllHistoryOfCompetition(int competitionId, int clubId, string token);
    }
}
