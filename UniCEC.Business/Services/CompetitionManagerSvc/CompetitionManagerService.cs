using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.CompetitionManagerSvc
{
    public class CompetitionManagerService : ICompetitionManagerService
    {
        ICompetitionManagerRepo _competitionManagerRepo;
        //Add 
        private ICompetitionRepo _competitionRepo;
        private IClubRepo _clubRepo;
        private ITermRepo _termRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionInClubRepo _competitionInClubRepo;
        private ICompetitionRoleRepo _competitionRoleRepo;
        private JwtSecurityTokenHandler _tokenHandler;

        public CompetitionManagerService(ICompetitionManagerRepo competitionManagerRepo,
                                         IClubRepo clubRepo,
                                         ITermRepo termRepo,
                                         IMemberRepo memberRepo,
                                         ICompetitionInClubRepo competitionInClubRepo,
                                         ICompetitionRepo competitionRepo,
                                         ICompetitionRoleRepo competitionRoleRepo)
        {
            _competitionManagerRepo = competitionManagerRepo;
            _clubRepo = clubRepo;
            _termRepo = termRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _competitionRepo = competitionRepo;
            _competitionRoleRepo = competitionRoleRepo;
        }
        //Get All Manager In Competition
        public async Task<PagingResult<ViewCompetitionManager>> GetAllManagerCompOrEve(CompetitionManagerRequestModel request, string token)
        {
            try
            {
                if (request.CompetitionId == 0
                  || request.ClubId == 0)
                    throw new ArgumentNullException("|| Competition Id Null" + " ClubId Null");

                bool Check = await CheckCompetitionManager(token, request.CompetitionId, request.ClubId);
                if (Check)
                {
                    PagingResult<ViewCompetitionManager> result = await _competitionRepo.GetAllManagerCompOrEve(request);
                    if (result == null) throw new NullReferenceException();
                    return result;
                }//end if check
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

        public async Task<ViewCompetitionManager> AddMemberInCompetitionManager(CompetitionManagerInsertModel model, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");
                if (model.CompetitionId == 0
                        || model.ClubId == 0
                        || model.MemberId == 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null ||Member Id Null");

                bool check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (check)
                {
                    //------------- CHECK Id Member
                    Member mem = await _memberRepo.Get(model.MemberId);
                    if (mem != null)
                    {
                        //------------- CHECK Id Member in club 
                        if (mem.ClubId == model.ClubId)
                        {

                            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(model.ClubId);
                            if (CurrentTermOfCLub != null)
                            {
                                //------------- CHECK Id Member the same as manager                   
                                GetMemberInClubModel conditions = new GetMemberInClubModel()
                                {
                                    UserId = UserId,
                                    ClubId = model.ClubId,
                                    TermId = CurrentTermOfCLub.Id
                                };
                                ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
                                if (infoClubMem.Id != model.MemberId)
                                {
                                    ViewCompetitionInClub cic = await _competitionInClubRepo.GetCompetitionInClub(model.ClubId, model.CompetitionId);
                                    CompetitionManager competitionManager = new CompetitionManager()
                                    {
                                        CompetitionInClubId = cic.Id,// id này là member thuộc club of club leader add 
                                        CompetitionRoleId = 1, //auto role lowest in competition manager
                                        MemberId = mem.Id,
                                        Fullname = mem.User.Fullname,
                                    };

                                    int result = await _competitionManagerRepo.Insert(competitionManager);
                                    if (result > 0)
                                    {
                                        CompetitionManager cpm = await _competitionManagerRepo.Get(result);

                                        return TransferViewCompetitionManager(cpm);
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Add Failed");
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Member has already joined");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Term of ClubId is End");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Member is not in club");
                        }
                    }
                    //
                    else
                    {
                        throw new ArgumentException("Member not found");
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

        public async Task<bool> UpdateMemberInCompetitionManager(CompetitionManagerUpdateModel model, string token)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");
                if (model.CompetitionId == 0
                        || model.ClubId == 0
                        || model.MemberId == 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null ||Member Id Null");

                bool check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (check)
                {
                    //------------- CHECK Id Member
                    Member mem = await _memberRepo.Get(model.MemberId);
                    if (mem != null)
                    {
                        //------------- CHECK Id Member In Competition Manager of this Competition Id
                        CompetitionManager cm = await _competitionManagerRepo.GetMemberInCompetitionManager(model.CompetitionId, model.MemberId, model.ClubId);
                        if (cm != null)
                        {
                            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(model.ClubId);
                            if (CurrentTermOfCLub != null)
                            {
                                //------------- CHECK Id Member the same as manager                   
                                GetMemberInClubModel conditions = new GetMemberInClubModel()
                                {
                                    UserId = UserId,
                                    ClubId = model.ClubId,
                                    TermId = CurrentTermOfCLub.Id
                                };
                                ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
                                if (infoClubMem.Id != model.MemberId)
                                {
                                    //------------- CHECK Competition Role
                                    CompetitionRole competitionRole = await _competitionRoleRepo.Get(model.RoleCompetitionId);
                                    if (competitionRole != null)
                                    {
                                        //Manager update role for this member
                                        cm.CompetitionRoleId = model.RoleCompetitionId;// auto role Manager
                                        await _competitionManagerRepo.Update();
                                        return true;
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Competition Role not have is system");
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Member id is the same you");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Member is not in Competition Manager of this Competition or Event");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Term of ClubId is End");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Member not found");
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> CheckCompetitionManager(string Token, int CompetitionId, int ClubId)
        {
            int UserId = DecodeToken(Token, "Id");

            //------------- CHECK Competition is have in system or not
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition != null)
            {
                //------------- CHECK Club in system
                Club club = await _clubRepo.Get(ClubId);
                if (club != null)
                {
                    ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(ClubId);
                    if (CurrentTermOfCLub != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = ClubId,
                            TermId = CurrentTermOfCLub.Id
                        };
                        ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
                        //------------- CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK User is in CompetitionManger table                
                            CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.Id, ClubId);
                            if (isAllow != null)
                            {
                                //------------- CHECK Role Is Manger
                                if (isAllow.CompetitionRoleId == 1)
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new UnauthorizedAccessException("Only role Manager can do this action");
                                }
                            }
                            else
                            {
                                throw new UnauthorizedAccessException("You do not in Competition Manager ");
                            }
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("You are not member in Club");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Term of ClubId is End");
                    }
                }
                else
                {
                    throw new ArgumentException("Club is not found");
                }
            }
            else
            {
                throw new ArgumentException("Competition or Event not found ");
            }
        }

        private ViewCompetitionManager TransferViewCompetitionManager(CompetitionManager competitionManager)
        {
            return new ViewCompetitionManager()
            {
                Id = competitionManager.Id,
                CompetitionRoleId = competitionManager.CompetitionRoleId,
                CompetitionInClubId = competitionManager.CompetitionInClubId,
                MemberId = competitionManager.MemberId,
                FullName = competitionManager.Fullname,
            };
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }


    }
}


