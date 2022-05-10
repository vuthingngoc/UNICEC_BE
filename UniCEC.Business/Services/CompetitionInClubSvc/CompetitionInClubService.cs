using System;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

namespace UniCEC.Business.Services.CompetitionInClubSvc
{
    public class CompetitionInClubService : ICompetitionInClubService
    {
        private ICompetitionInClubRepo _competitionInClubRepo;

        //check Infomation Member -> is Leader
        private IClubHistoryRepo _clubHistoryRepo;

        //change Status of Competition
        private ICompetitionRepo _competitionRepo;

        public CompetitionInClubService(ICompetitionInClubRepo competitionInClubRepo, ICompetitionRepo competitionRepo, IClubHistoryRepo clubHistoryRepo)                               
        {
            _competitionInClubRepo = competitionInClubRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionRepo = competitionRepo;
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
                bool roleLeader = false;
                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = model.UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);

                //------------ Check Mem in that club
                if (infoClubMem != null)
                {
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }
                }
                //------------ Check Role Member Is Leader 
                if (roleLeader)
                {
                    //------------------------------------check-club-id-create-competition-duplicate
                    bool checkCreateCompetitionInClub = await _competitionInClubRepo.CheckDuplicateCreateCompetition(model.ClubId, model.CompetitionId);
                    if (checkCreateCompetitionInClub)
                    {
                        CompetitionInClub competitionInClub = new CompetitionInClub();
                        competitionInClub.ClubId = model.ClubId;
                        competitionInClub.CompetitionId = model.CompetitionId;

                        int result = await _competitionInClubRepo.Insert(competitionInClub);
                        if (result > 0)
                        {
                            //đổi status của competition 
                            Competition comp = await _competitionRepo.Get(model.CompetitionId);                          
                            comp.Status = CompetitionStatus.HappeningSoon;
                            await _competitionRepo.Update();
                            //
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

        public Task<bool> Update(ViewCompetitionInClub model)
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
