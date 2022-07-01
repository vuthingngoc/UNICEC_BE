using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo
{
    public interface ICompetitionInClubRepo : IRepository<CompetitionInClub>
    {
        
        public Task<List<ViewClubInComp>> GetListClubInCompetition(int competitionId);
        public Task<ViewCompetitionInClub> GetCompetitionInClub(int clubId, int competitionId);
        public Task DeleteCompetitionInClub(int competitionInClubId);
        
        // Nhat
        public Task<int> GetTotalEventOrganizedByClub(int clubId);
    }
}
