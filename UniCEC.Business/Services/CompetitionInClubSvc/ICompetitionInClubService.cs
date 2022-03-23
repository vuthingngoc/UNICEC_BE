using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

namespace UniCEC.Business.Services.CompetitionInClubSvc
{
    public interface ICompetitionInClubService
    {
        public Task<PagingResult<ViewCompetitionInClub>> GetAll(PagingRequest request);
        public Task<ViewCompetitionInClub> GetByCompetitionInClubId(int id);
        public Task<ViewCompetitionInClub> Insert(CompetitionInClubInsertModel competitionInClub);
        public Task<bool> Update(ViewCompetitionInClub competitionInClub);
        public Task<bool> Delete(int id);
    }
}
