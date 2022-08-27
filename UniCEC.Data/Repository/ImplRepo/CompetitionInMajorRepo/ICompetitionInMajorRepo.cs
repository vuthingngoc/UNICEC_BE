using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInMajorRepo
{
    public interface ICompetitionInMajorRepo : IRepository<CompetitionInMajor> 
    {
        public Task<List<ViewMajorInComp>> GetListMajorInCompetition(int competitionId);

        //public Task<List<int>> GetListMajorIdInCompetition(int competitionId);

        public Task<CompetitionInMajor> GetMajorInCompetition(int majorId, int competitionId);

        public Task DeleteCompetitionInMajor(int competitionInMajorId);

        public Task DeleteAllCompetitionInMajor(int competitionId);
    }
}
