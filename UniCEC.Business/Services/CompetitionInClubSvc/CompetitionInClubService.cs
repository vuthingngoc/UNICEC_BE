using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
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

        //
        private IClubRepo _clubRepo;



        public CompetitionInClubService(ICompetitionInClubRepo competitionInClubRepo, ICompetitionRepo competitionRepo, IClubHistoryRepo clubHistoryRepo, IClubRepo clubRepo)
        {
            _competitionInClubRepo = competitionInClubRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionRepo = competitionRepo;
            _clubRepo = clubRepo;
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

        public async Task<ViewCompetitionInClub> Insert(CompetitionInClubInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var UniversityIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));



                int UserId = Int32.Parse(UserIdClaim.Value);

                int UniversityId = Int32.Parse(UniversityIdClaim.Value);

                bool roleLeader = false;

                if (model.ClubIdCollaborate == 0
                   || model.CompetitionId == 0
                   || model.ClubId == 0
                   || model.TermId == 0)
                    throw new ArgumentNullException("Club Id Collaborate Null || Competition Id Null || Club Id Null || Term Id Null ");

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                //------------ Check 2 club are the same 
                if (model.ClubIdCollaborate != model.ClubId)
                {
                    //------------ Check Mem in that club
                    if (infoClubMem != null)
                    {
                        if (infoClubMem.ClubRoleName.Equals("Leader"))
                        {
                            roleLeader = true;
                        }
                        //------------ Check Role Member Is Leader 
                        if (roleLeader)
                        {
                            //------------------------------------check-club-id-create-competition-or-event-duplicate
                            //true  -> có nghĩa là nó chưa được tạo -> kh thể add được 
                            //false -> có nghĩa là nó chưa được tạo -> add được (do đây là add thêm club collaborate)
                            bool checkCreateCompetitionInClub = await _competitionInClubRepo.CheckDuplicateCreateCompetitionOrEvent(model.ClubId, model.CompetitionId);
                            if (checkCreateCompetitionInClub == false)
                            {
                                //---------------Check Club-Id-Collaborate----------
                                //check club Id Collaborate has in system
                                Club club = await _clubRepo.Get(model.ClubIdCollaborate);
                                if (club != null)
                                {
                                    //
                                    bool checkClubIn_Out = false;
                                    Competition competition = await _competitionRepo.Get(model.CompetitionId);

                                    //public == false just for club inside University 
                                    if (competition.Public == false)
                                    {
                                        if (club.UniversityId == UniversityId)
                                        {
                                            checkClubIn_Out = true;
                                        }
                                        else
                                        {
                                            checkClubIn_Out = false;
                                        }
                                    }
                                    //public == true can join
                                    else
                                    {
                                        checkClubIn_Out = true;
                                    }
                                    if (checkClubIn_Out)
                                    {
                                        CompetitionInClub competitionInClub = new CompetitionInClub();
                                        competitionInClub.ClubId = model.ClubIdCollaborate;
                                        competitionInClub.CompetitionId = model.CompetitionId;

                                        int result = await _competitionInClubRepo.Insert(competitionInClub);
                                        if (result > 0)
                                        {
                                            CompetitionInClub cic = await _competitionInClubRepo.Get(result);

                                            return TransferView(cic);
                                        }//end result
                                        else
                                        {
                                            throw new ArgumentException("Add Competition Or Event Failed");
                                        }
                                    }//end check ClubIn_Out
                                    else
                                    {
                                        throw new ArgumentException("Club collaborate not in University");
                                    }
                                }//end check club in system
                                else
                                {
                                    throw new ArgumentException("Club collaborate not found in system");
                                }
                            }//end check exsit Competition Or Event
                            else
                            {
                                throw new ArgumentException("Competition or Event not found");
                            }
                        }//end role leader
                        else
                        {
                            throw new UnauthorizedAccessException("You do not a role Leader to add Club in this Competititon");
                        }
                    }//end not member in club
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't member in Club");
                    }
                }//end check 2 club are the same 
                else
                {
                    throw new ArgumentException("Club already join");
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
