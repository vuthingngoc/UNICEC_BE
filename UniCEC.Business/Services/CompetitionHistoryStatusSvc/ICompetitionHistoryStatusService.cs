using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;

namespace UniCEC.Business.Services.CompetitionHistorySvc
{
    public interface ICompetitionHistoryStatusService
    {
        public Task<List<ViewCompetitionHistoryStatus>> GetAllHistoryOfCompetition(int competitionId, int clubId, string token);
    }
}
