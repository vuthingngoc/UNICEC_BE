using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.CompetitionInClub;

namespace UNICS.Business.Services.CompetitionInClubSvc
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
