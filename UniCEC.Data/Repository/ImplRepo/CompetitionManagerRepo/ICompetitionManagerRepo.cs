using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo
{
    public interface ICompetitionManagerRepo : IRepository<CompetitionManager>
    {
        public Task<CompetitionManager> GetCompetitionManager(int CompetitionId, int ClubId, int MemberId);
    }
}
