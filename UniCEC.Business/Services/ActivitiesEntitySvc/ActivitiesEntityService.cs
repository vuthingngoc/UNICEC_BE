using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ActivitiesEntityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Entities.ActivitiesEntity;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.ActivitiesEntitySvc
{
    public class ActivitiesEntityService : IActivitiesEntityService
    {
        private IActivitiesEntityRepo _activitiesEntityRepo;
        //Add
        private ICompetitionActivityRepo _competitionActivityRepo;
        private ICompetitionRepo _competitionRepo;
        private IFileService _fileService;
        private IClubRepo _clubRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IMemberRepo _memberRepo;
        private DecodeToken _decodeToken;


        public ActivitiesEntityService(IActivitiesEntityRepo activitiesEntityRepo,
                                       ICompetitionActivityRepo competitionActivityRepo,
                                       ICompetitionRepo competitionRepo,
                                       IFileService fileService,
                                       IClubRepo clubRepo,
                                       ICompetitionManagerRepo competitionManagerRepo,
                                       IMemberRepo memberRepo)
        {
            _activitiesEntityRepo = activitiesEntityRepo;
            _competitionActivityRepo = competitionActivityRepo;
            _competitionRepo = competitionRepo;
            _fileService = fileService;
            _clubRepo = clubRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _memberRepo = memberRepo;
            _decodeToken = new DecodeToken();
        }

        //public async Task<ViewActivitiesEntity> AddActivitiesEntity(ActivitiesEntityInsertModel model, string token)
        //{
        //    try
        //    {
        //        if (model.CompetitionActivityId == 0
        //          || model.ClubId == 0)
        //            throw new ArgumentNullException("|| Competition Acitvity Id Null || ClubId Null");

        //        CompetitionActivity ca = await _competitionActivityRepo.Get(model.CompetitionActivityId);

        //        //Check Competition Activity Existed
        //        if (ca == null) throw new ArgumentException("Competition Activity is not found");

        //        //
        //        Competition c = await _competitionRepo.Get(ca.CompetitionId);

        //        bool Check = await CheckConditions(token, c.Id, model.ClubId);
        //        if (Check)
        //        {
        //            //------------ Insert Activities-Entities-----------
        //            string Url = await _fileService.UploadFile(model.Base64String);
        //            ActivitiesEntity ActivitiesEntity = new ActivitiesEntity()
        //            {
        //                CompetitionActivityId = model.CompetitionActivityId,
        //                Name = model.Name,
        //                ImageUrl = Url
        //            };

        //            int id = await _activitiesEntityRepo.Insert(ActivitiesEntity);

        //            if (id > 0)
        //            {
        //                ActivitiesEntity entity = await _activitiesEntityRepo.Get(id);

        //                //get IMG from Firebase                        
        //                string imgUrl;

        //                try
        //                {
        //                    imgUrl = await _fileService.GetUrlFromFilenameAsync(entity.ImageUrl);
        //                }
        //                catch (Exception ex)
        //                {
        //                    imgUrl = "";
        //                }

        //                return new ViewActivitiesEntity()
        //                {
        //                    Id = entity.Id,
        //                    Name = entity.Name,
        //                    CompetitionActivityId = entity.CompetitionActivityId,
        //                    ImageUrl = imgUrl,
        //                };
        //            }
        //            return null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //private async Task<bool> CheckConditions(string Token, int CompetitionId, int ClubId)
        //{
        //    //
        //    int UserId = _decodeToken.Decode(Token, "Id");

        //    //------------- CHECK Competition is have in system or not
        //    Competition competition = await _competitionRepo.Get(CompetitionId);
        //    if (competition != null)
        //    {
        //        //------------- CHECK Club in system
        //        Club club = await _clubRepo.Get(ClubId);
        //        if (club != null)
        //        {
        //            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(ClubId);
        //            if (CurrentTermOfCLub != null)
        //            {
        //                GetMemberInClubModel conditions = new GetMemberInClubModel()
        //                {
        //                    UserId = UserId,
        //                    ClubId = ClubId,
        //                    TermId = CurrentTermOfCLub.Id
        //                };
        //                ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
        //                //------------- CHECK Mem in that club
        //                if (infoClubMem != null)
        //                {
        //                    //------------- CHECK is in CompetitionManger table                
        //                    CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.Id, ClubId);
        //                    if (isAllow != null)
        //                    {
        //                        return true;
        //                    }
        //                    else
        //                    {
        //                        throw new UnauthorizedAccessException("You do not in Competition Manager");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new UnauthorizedAccessException("You are not member in Club");
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Term of ClubId is End");
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Club is not found");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Competition or Event not found ");
        //    }
        //}

        //private int DecodeToken(string token, string nameClaim)
        //{
        //    if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
        //    var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
        //    return Int32.Parse(claim.Value);
        //}
    }
}
