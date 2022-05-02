using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
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

        public Task<PagingResult<ViewCompetitionInClub>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewCompetitionInClub> GetByCompetitionInClubId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewCompetitionInClub> Insert(CompetitionInClubInsertModel model)
        {
            try
            {
                CompetitionInClub competitionInClub = new CompetitionInClub();
                competitionInClub.ClubId = model.ClubId;
                competitionInClub.CompetitionId = model.CompetitionId;
                //------------------------------------check-club-id-create-competition-duplicate
                bool checkCreateCompetitionInClub = await _competitionInClubRepo.CheckDuplicateCreateCompetition(model.ClubId, model.CompetitionId);
                if (checkCreateCompetitionInClub)
                {

                    int result = await _competitionInClubRepo.Insert(competitionInClub);
                    if (result > 0)
                    {
                        return TransferView(competitionInClub);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> Update(ViewCompetitionInClub competitionInClub)
        {
            throw new NotImplementedException();
        }

        private ViewCompetitionInClub TransferView(CompetitionInClub competitionInClub)
        {
            return new ViewCompetitionInClub()
            {
                Id = competitionInClub.Id,
                ClubId = competitionInClub.ClubId,
                CompetitionId = competitionInClub.CompetitionId,
            };
        }
    }
}
