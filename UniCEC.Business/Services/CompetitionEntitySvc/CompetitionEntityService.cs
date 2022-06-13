using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.CompetitionEntitySvc
{
    public class CompetitionEntityService : ICompetitionEntityService
    {
       
        private ICompetitionEntityRepo _competitionEntityRepo;
        //Add 
        private ICompetitionRepo _competitionRepo;
        private IFileService _fileService;
        private IClubRepo _clubRepo;
        private ITermRepo _termRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IMemberRepo _memberRepo;
        private JwtSecurityTokenHandler _tokenHandler;


        public CompetitionEntityService(ICompetitionEntityRepo competitionEntityRepo,
                                        ICompetitionRepo competitionRepo,
                                        IFileService fileService,
                                        IClubRepo clubRepo,
                                        ITermRepo termRepo,
                                        ICompetitionManagerRepo competitionManagerRepo,
                                        IMemberRepo memberRepo) 
        {
            _competitionEntityRepo = competitionEntityRepo;
            _competitionRepo = competitionRepo;
            _fileService = fileService;
            _clubRepo = clubRepo;
            _termRepo = termRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _memberRepo = memberRepo;   
        }


        public async Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                  || model.ClubId == 0)
                    throw new ArgumentNullException("|| Competition Id Null" +
                                                     " ClubId Null");

                bool Check = await CheckConditions(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //------------- CHECK Status Competition
                    Competition c = await _competitionRepo.Get(model.CompetitionId);
                    if (c.Status != CompetitionStatus.Happening && c.Status != CompetitionStatus.Ending && c.Status != CompetitionStatus.Canceling)
                    {

                        //------------ Insert Competition-Entities-----------
                        string Url = await _fileService.UploadFile(model.Base64String);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {
                            CompetitionId = model.CompetitionId,
                            Name = model.Name,
                            ImageUrl = Url
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);

                        if (id > 0)
                        {
                            CompetitionEntity entity = await _competitionEntityRepo.Get(id);

                            //get IMG from Firebase                        
                            string imgUrl;

                            try
                            {
                                imgUrl = await _fileService.GetUrlFromFilenameAsync(entity.ImageUrl);
                            }
                            catch (Exception ex)
                            {
                                imgUrl = "";
                            }

                            return new ViewCompetitionEntity()
                            {
                                Id = entity.Id,
                                Name = entity.Name,
                                CompetitionId = entity.CompetitionId,
                                ImageUrl = imgUrl,
                            };
                        }
                        return null;
                    }
                    else
                    {
                        throw new ArgumentException("Can't Update Competition when it has Status Happenning or Ending or Canceling");
                    }
                }
                //end if check
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


        private async Task<bool> CheckConditions(string Token, int CompetitionId, int ClubId)
        {
            //
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
                            //------------- CHECK is in CompetitionManger table                
                            CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.Id, ClubId);
                            if (isAllow != null)
                            {
                                return true;
                            }
                            else
                            {
                                throw new UnauthorizedAccessException("You do not in Competition Manager");
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

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

    }
}

