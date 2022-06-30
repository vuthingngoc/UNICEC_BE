using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo
{
    public interface ICompetitionHistoryStatusRepo : IRepository<CompetitionHistoryStatus>
    {
        public Task<List<ViewCompetitionHistoryStatus>> GetAllHistoryOfCompetition(int competitionId);
    }
}
