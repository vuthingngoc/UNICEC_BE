using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

namespace UniCEC.Business.Services.CompetitionInClubSvc
{
    public class CompetitionInClubService : ICompetitionInClubService
    {
        private ICompetitionInClubRepo _competitionInClubRepo;

        public CompetitionInClubService(ICompetitionInClubRepo competitionInClubRepo)
        {
            _competitionInClubRepo = competitionInClubRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewCompetitionInClub>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionInClub> GetByCompetitionInClubId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionInClub> Insert(CompetitionInClubInsertModel competitionInClub)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewCompetitionInClub competitionInClub)
        {
            throw new NotImplementedException();
        }
    }
}
