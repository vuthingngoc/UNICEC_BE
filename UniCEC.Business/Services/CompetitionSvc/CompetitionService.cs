using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Services.SeedsWalletSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInMajorRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.UniversityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionInMajor;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public class CompetitionService : ICompetitionService
    {
        private ICompetitionRepo _competitionRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionInClubRepo _competitionInClubRepo;
        private IClubRepo _clubRepo;
        private IParticipantRepo _participantRepo;
        private ICompetitionTypeRepo _competitionTypeRepo;
        private IFileService _fileService;
        private ICompetitionEntityRepo _competitionEntityRepo;
        private ICompetitionRoleRepo _competitionRoleRepo;
        private IUserRepo _userRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private IMajorRepo _majorRepo;
        private ICompetitionInMajorRepo _competitionInMajorRepo;
        private ICompetitionHistoryRepo _competitionHistoryRepo;
        private ICompetitionRoundRepo _competitionRoundRepo;
        private IMatchRepo _matchRepo;
        private DecodeToken _decodeToken;
        private readonly IConfiguration _configuration;
        private ISeedsWalletService _seedsWalletService;
        private IUniversityRepo _universityRepo;
        private INotificationService _notificationService;

        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IMemberRepo memberRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  IClubRepo clubRepo,
                                  IParticipantRepo participantRepo,
                                  ICompetitionTypeRepo competitionTypeRepo,
                                  ICompetitionEntityRepo competitionEntityRepo,
                                  IMemberInCompetitionRepo memberInCompetitionRepo,
                                  IConfiguration configuration,
                                  IUserRepo userRepo,
                                  IMajorRepo majorRepo,
                                  ICompetitionInMajorRepo competitionInMajorRepo,
                                  ICompetitionHistoryRepo competitionHistoryRepo,
                                  ICompetitionRoleRepo competitionRoleRepo,
                                  ICompetitionRoundRepo competitionRoundRepo,
                                  IMatchRepo matchRepo,
                                  ISeedsWalletService seedsWalletService,
                                  IUniversityRepo universityRepo,
                                  IFileService fileService,
                                  INotificationService notificationService)
        {
            _competitionRepo = competitionRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _clubRepo = clubRepo;
            _participantRepo = participantRepo;
            _competitionTypeRepo = competitionTypeRepo;
            _fileService = fileService;
            _competitionEntityRepo = competitionEntityRepo;
            _configuration = configuration;
            _userRepo = userRepo;
            _majorRepo = majorRepo;
            _decodeToken = new DecodeToken();
            _competitionRoleRepo = competitionRoleRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionInMajorRepo = competitionInMajorRepo;
            _competitionHistoryRepo = competitionHistoryRepo;
            _competitionRoundRepo = competitionRoundRepo;
            _matchRepo = matchRepo;
            _seedsWalletService = seedsWalletService;
            _universityRepo = universityRepo;
            _notificationService = notificationService;
        }

        private async Task<string> GetUrlImageClub(string imageUrl, int clubId)
        {
            string fullPathImage = await _fileService.GetUrlFromFilenameAsync(imageUrl) ?? "";
            if (!string.IsNullOrEmpty(imageUrl) && !imageUrl.Equals(fullPathImage)) // for old data save filename in  db
            {
                Club club = await _clubRepo.Get(clubId);
                club.Image = fullPathImage;
                await _clubRepo.Update();
            }

            return fullPathImage;
        }

        private async Task<string> GetUrlImageCompEntity(string imageUrl, int competitionEntityId)
        {
            string fullPathImage = await _fileService.GetUrlFromFilenameAsync(imageUrl) ?? "";
            if (!string.IsNullOrEmpty(imageUrl) && !imageUrl.Equals(fullPathImage)) // for old data save filename in  db
            {
                CompetitionEntity competitionEntity = await _competitionEntityRepo.Get(competitionEntityId);
                competitionEntity.ImageUrl = fullPathImage;
                await _competitionEntityRepo.Update();
            }

            return fullPathImage;
        }


        public async Task<ViewProcessCompetitionOrEventOfClub> GetNumberOfCompetitionOrEventInClubWithStatus(int clubId, string token)
        {
            try
            {
                //------------- CHECK Club in system
                Club club = await _clubRepo.Get(clubId);
                if (club == null) throw new ArgumentException("Club in not found");

                //------------- CHECK Is Member in Club
                int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), club.Id);
                Member member = await _memberRepo.Get(memberId);
                if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

                //competition
                int numberOfCompetitionRegistering = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Register, clubId, false);
                int numberOfCompetitionUpComing = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.UpComing, clubId, false);

                //field diễn ra (Start + Ongoing + End + Finish)  
                int numberOfCompetitionOnGoing = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.OnGoing, clubId, false);
                int numberOfCompetitionStart = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Start, clubId, false);
                int numberOfCompetitionEnd = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.End, clubId, false);
                int numberOfCompetitionFinish = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Finish, clubId, false);

                int numberOfCompetitionResult = numberOfCompetitionStart + numberOfCompetitionOnGoing + numberOfCompetitionEnd + numberOfCompetitionFinish;

                int numberOfCompetitionCompleted = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Complete, clubId, false);

                //event
                int numberOfEventRegistering = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Register, clubId, true);
                int numberOfEventUpComing = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.UpComing, clubId, true);

                //field diễn ra (Start + Ongoing + End + Finish)  
                int numberOfEventOnGoing = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.OnGoing, clubId, true);
                int numberOfEventStart = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Start, clubId, true);
                int numberOfEventEnd = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.End, clubId, true);
                int numberOfEventFinish = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Finish, clubId, true);

                int numberOfEventResult = numberOfEventOnGoing + numberOfEventStart + numberOfEventEnd + numberOfEventFinish;

                int numberOfEventCompleted = await _competitionRepo.GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus.Complete, clubId, true);

                ViewProcessCompetitionOrEventOfClub view = new ViewProcessCompetitionOrEventOfClub()
                {
                    ClubId = clubId,
                    numberCompetitionOfRegistering = numberOfCompetitionRegistering,
                    numberCompetitionOfUpComing = numberOfCompetitionUpComing,
                    numberCompetitionOfOnGoing = numberOfCompetitionResult,
                    numberCompetitionOfCompleted = numberOfCompetitionCompleted,
                    numberEventOfRegistering = numberOfEventRegistering,
                    numberEventOfUpComing = numberOfEventUpComing,
                    numberEventOfOnGoing = numberOfEventResult,
                    numberEventOfCompleted = numberOfEventCompleted,
                };
                return view;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagingResult<ViewCompetition>> GetCompOrEveStudentIsAssignedTask(PagingRequest request, int clubId, string searchName, bool? isEvent, string token)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEveStudentIsAssignedTask(request, clubId, searchName, isEvent, _decodeToken.Decode(token, "Id"));
                if (result == null) throw new NullReferenceException();
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewCompetition> GetCompOrEveStudentJoin(int competitionId, string token)
        {
            ViewCompetition result = await _competitionRepo.GetCompOrEveStudentJoin(competitionId, _decodeToken.Decode(token, "Id"));

            foreach (ViewClubInComp viewClub in result.ClubInCompetition)
            {
                //get IMG from Firebase                        
                string imgClub;
                try
                {
                    if (viewClub.Image.Contains("https"))
                    {
                        imgClub = viewClub.Image;
                    }
                    else
                    {
                        imgClub = await _fileService.GetUrlFromFilenameAsync(viewClub.Image);
                    }
                }
                catch (Exception)
                {
                    imgClub = "";
                }

                viewClub.Image = imgClub;
            }

            //List Competition Entity
            List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

            List<CompetitionEntity> CompetitionEntities = await _competitionEntityRepo.GetListCompetitionEntity(result.Id);

            if (CompetitionEntities != null)
            {
                foreach (CompetitionEntity competitionEntity in CompetitionEntities)
                {
                    //get IMG from Firebase                        
                    string imgUrl_CompetitionEntity;
                    try
                    {
                        if (competitionEntity.ImageUrl.Contains("https"))
                        {
                            imgUrl_CompetitionEntity = competitionEntity.ImageUrl;
                        }
                        else
                        {
                            imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
                        }

                    }
                    catch (Exception)
                    {
                        imgUrl_CompetitionEntity = "";
                    }

                    ViewCompetitionEntity viewCompetitionEntity = new ViewCompetitionEntity()
                    {
                        Id = competitionEntity.Id,
                        CompetitionId = competitionEntity.CompetitionId,
                        Name = competitionEntity.Name,
                        Description = competitionEntity.Description,
                        Email = competitionEntity.Email,
                        EntityTypeId = competitionEntity.EntityTypeId,
                        EntityTypeName = competitionEntity.EntityType.Name,
                        Website = competitionEntity.Website,
                        ImageUrl = imgUrl_CompetitionEntity,
                    };
                    //
                    ListView_CompetitionEntities.Add(viewCompetitionEntity);
                }
            }
            result.CompetitionEntities = ListView_CompetitionEntities;
            return result;
        }

        public async Task<PagingResult<ViewCompetition>> GetCompsOrEvesStudentJoin(GetStudentJoinCompOrEve request, string token)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionRepo.GetCompsOrEvesStudentJoin(request, _decodeToken.Decode(token, "Id"));
                if (result == null) throw new NullReferenceException();

                foreach (ViewCompetition item in result.Items)
                {
                    //Add image club
                    foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                    {
                        //get IMG from Firebase                        
                        //string imgClub;

                        //try
                        //{
                        //    if (viewClub.Image.Contains("https"))
                        //    {
                        //        imgClub = viewClub.Image;
                        //    }
                        //    else
                        //    {
                        //        imgClub = await _fileService.GetUrlFromFilenameAsync(viewClub.Image);
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    imgClub = "";
                        //}

                        viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                    }


                    //List Competition Entity
                    //List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

                    List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                    if (competitionEntities != null)
                    {
                        foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                        {
                            //get IMG from Firebase                        
                            //string imgUrl_CompetitionEntity;
                            //try
                            //{
                            //    if (competitionEntity.ImageUrl.Contains("https"))
                            //    {
                            //        imgUrl_CompetitionEntity = competitionEntity.ImageUrl;
                            //    }
                            //    else
                            //    {
                            //        imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
                            //    }

                            //}
                            //catch (Exception)
                            //{
                            //    imgUrl_CompetitionEntity = "";
                            //}
                            competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);

                            //ViewCompetitionEntity viewCompetitionEntity = new ViewCompetitionEntity()
                            //{
                            //    Id = competitionEntity.Id,
                            //    CompetitionId = competitionEntity.CompetitionId,
                            //    Name = competitionEntity.Name,
                            //    Description = competitionEntity.Description,
                            //    Email = competitionEntity.Email,
                            //    EntityTypeId = competitionEntity.EntityTypeId,
                            //    EntityTypeName = competitionEntity.EntityType.Name,
                            //    Website = competitionEntity.Website,
                            //    ImageUrl = imgUrl_CompetitionEntity,
                            //};
                            //
                            //ListView_CompetitionEntities.Add(viewCompetitionEntity);
                        }

                        item.CompetitionEntities = competitionEntities;
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PagingResult<ViewCompetition>> GetCompOrEveUnAuthorize(CompetitionUnAuthorizeRequestModel request)
        {
            try
            {
                List<CompetitionStatus> listCompetitionStatus = new List<CompetitionStatus>();
                listCompetitionStatus.Add(CompetitionStatus.Register); // register
                listCompetitionStatus.Add(CompetitionStatus.Publish); // publish

                PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEveUnAuthorize(request, listCompetitionStatus);
                if (result == null) throw new NullReferenceException();
                ////Không trả hình ảnh khi kh có giá trị entities
                //if (!request.getEntities.HasValue)
                //{
                foreach (ViewCompetition item in result.Items)
                {

                    //Add image club
                    foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                    {
                        viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                    }

                    //List Competition Entity
                    //List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

                    List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                    if (competitionEntities != null)
                    {
                        foreach (var competitionEntity in competitionEntities)
                        {
                            competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);
                            //_fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl) ?? "";
                        }
                        item.CompetitionEntities = competitionEntities;
                    }
                }

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagingResult<ViewCompetition>> GetCompetitionByAdminUni(AdminUniGetCompetitionRequestModel request, string token)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEveByAdminUni(request, _decodeToken.Decode(token, "UniversityId"));
                if (result == null) throw new NullReferenceException();

                if (!request.getEntities.HasValue)
                {
                    foreach (ViewCompetition item in result.Items)
                    {
                        //Add image club
                        foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                        {
                            //viewClub.Image = imgClub;
                            viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                        }
                        List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                        if (competitionEntities != null)
                        {
                            foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                            {
                                competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);
                            }
                            item.CompetitionEntities = competitionEntities;
                        }
                    }
                }

                //Không có value là lấy hình
                if (request.getEntities.Equals(true))
                {
                    foreach (ViewCompetition item in result.Items)
                    {
                        //Add image club
                        foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                        {
                            //get IMG from Firebase                        
                            //string imgClub;

                            //try
                            //{
                            //    if (viewClub.Image.Contains("https"))
                            //    {
                            //        imgClub = viewClub.Image;
                            //    }
                            //    else
                            //    {
                            //        imgClub = await _fileService.GetUrlFromFilenameAsync(viewClub.Image);
                            //    }
                            //}
                            //catch (Exception)
                            //{
                            //    imgClub = "";
                            //}

                            viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                        }

                        //List Competition Entity
                        //List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

                        List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                        if (competitionEntities != null)
                        {
                            foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                            {
                                //get IMG from Firebase                        
                                //string imgUrl_CompetitionEntity;
                                //try
                                //{
                                //    if (competitionEntity.ImageUrl.Contains("https"))
                                //    {
                                //        imgUrl_CompetitionEntity = competitionEntity.ImageUrl;
                                //    }
                                //    else
                                //    {
                                //        imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
                                //    }

                                //}
                                //catch (Exception)
                                //{
                                //    imgUrl_CompetitionEntity = "";
                                //}
                                competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);

                                //ViewCompetitionEntity viewCompetitionEntity = new ViewCompetitionEntity()
                                //{
                                //    Id = competitionEntity.Id,
                                //    CompetitionId = competitionEntity.CompetitionId,
                                //    Name = competitionEntity.Name,
                                //    Description = competitionEntity.Description,
                                //    Email = competitionEntity.Email,
                                //    EntityTypeId = competitionEntity.EntityTypeId,
                                //    EntityTypeName = competitionEntity.EntityType.Name,
                                //    Website = competitionEntity.Website,
                                //    ImageUrl = imgUrl_CompetitionEntity,
                                //};
                                //
                                //ListView_CompetitionEntities.Add(viewCompetitionEntity);
                            }

                            item.CompetitionEntities = competitionEntities;
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get
        public async Task<ViewDetailCompetition> GetById(int id)
        {
            //
            Competition comp = await _competitionRepo.Get(id);
            //
            if (comp == null) throw new NullReferenceException();

            if (comp.Status == CompetitionStatus.Register || comp.Status == CompetitionStatus.Publish)
            {
                comp.View += 1;
                await _competitionRepo.Update();
            }
            return await TransferViewDetailCompetition(comp);

        }

        //Get top 3 EVENT or COMPETITION by Status

        public async Task<List<ViewTopCompetition>> GetTopCompOrEve(int ClubId, bool? Event/*, CompetitionStatus? Status*/, CompetitionScopeStatus? Scope, int Top)
        {

            List<ViewTopCompetition> result = await _competitionRepo.GetTopCompOrEve(ClubId, Event/*, Status*/, Scope, Top);


            //foreach (ViewTopCompetition item in result)
            //{

            //    //Add image club
            //    foreach (ViewClubInComp viewClub in item.ClubInCompetition)
            //    {
            //        //get IMG from Firebase                        
            //        string imgClub;
            //        try
            //        {
            //            if (viewClub.Image.Contains("https"))
            //            {
            //                imgClub = viewClub.Image;
            //            }
            //            else
            //            {
            //                imgClub = await _fileService.GetUrlFromFilenameAsync(viewClub.Image);
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            imgClub = "";
            //        }

            //        viewClub.Image = imgClub;
            //    }

            ////List Competition Entity
            //List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

            //List<CompetitionEntity> CompetitionEntities = await _competitionEntityRepo.GetListCompetitionEntity(item.Id);

            //if (CompetitionEntities != null)
            //{
            //    foreach (CompetitionEntity competitionEntity in CompetitionEntities)
            //    {
            //        //get IMG from Firebase                        
            //        string imgUrl_CompetitionEntity;
            //        try
            //        {
            //            if (competitionEntity.ImageUrl.Contains("https"))
            //            {
            //                imgUrl_CompetitionEntity = competitionEntity.ImageUrl;
            //            }
            //            else
            //            {
            //                imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
            //            }

            //        }
            //        catch (Exception)
            //        {
            //            imgUrl_CompetitionEntity = "";
            //        }

            //        ViewCompetitionEntity viewCompetitionEntity = new ViewCompetitionEntity()
            //        {
            //            Id = competitionEntity.Id,
            //            CompetitionId = competitionEntity.CompetitionId,
            //            Name = competitionEntity.Name,
            //            Description = competitionEntity.Description,
            //            Email = competitionEntity.Email,
            //            EntityTypeId = competitionEntity.EntityTypeId,
            //            EntityTypeName = competitionEntity.EntityType.Name,
            //            Website = competitionEntity.Website,
            //            ImageUrl = imgUrl_CompetitionEntity,
            //        };
            //        //
            //        ListView_CompetitionEntities.Add(viewCompetitionEntity);
            //    }
            //}

            //item.CompetitionEntities = ListView_CompetitionEntities;
            //}
            if (result == null) throw new NullReferenceException();
            return result;
        }

        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request, string token)
        {
            //check trường hợp nếu có University và Club thì check xem trong trường đó có Club đó kh 
            if (request.UniversityId.HasValue && request.ClubId.HasValue)
            {
                //
                bool isExisted = await _universityRepo.CheckExistedUniversity(request.UniversityId.Value);
                if (!isExisted) throw new ArgumentException("University not in system");
                //
                isExisted = await _clubRepo.CheckExistedClub(request.ClubId.Value);
                if (!isExisted) throw new ArgumentException("Club not in system");
                //
                bool check = await _competitionRepo.CheckClubBelongToUniversity(request.ClubId.Value, request.UniversityId.Value);

                if (check == false) throw new ArgumentException("Club not belong to university");

            }

            PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEve(request, _decodeToken.Decode(token, "UniversityId"));
            if (result == null) throw new NullReferenceException();

            if (!request.GetEntities.HasValue)
            {
                foreach (ViewCompetition item in result.Items)
                {
                    //Add image club
                    foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                    {
                        //viewClub.Image = imgClub;
                        viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                    }
                    List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                    if (competitionEntities != null)
                    {
                        foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                        {
                            competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);
                        }
                        item.CompetitionEntities = competitionEntities;
                    }
                }
            }

            //Nếu kh có truyền getEntities thì kh show hình
            if (request.GetEntities.HasValue.Equals(true))
            {
                foreach (ViewCompetition item in result.Items)
                {
                    //Add image club
                    foreach (ViewClubInComp viewClub in item.ClubInCompetition)
                    {
                        //get IMG from Firebase                        
                        //string imgClub;

                        //try
                        //{
                        //    if (viewClub.Image.Contains("https"))
                        //    {
                        //        imgClub = viewClub.Image;
                        //    }
                        //    else
                        //    {
                        //        imgClub = await _fileService.GetUrlFromFilenameAsync(viewClub.Image);
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    imgClub = "";
                        //}

                        //viewClub.Image = imgClub;
                        viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
                    }
                    List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(item.Id);

                    if (competitionEntities != null)
                    {
                        foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                        {
                            competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);
                        }
                        item.CompetitionEntities = competitionEntities;
                    }
                }
            }
            return result;
        }

        public async Task<ViewDetailCompetition> InsertCompetitionOrEvent(LeaderInsertCompOrEventModel model, string token)
        {
            try
            {
                int UserId = _decodeToken.Decode(token, "Id");
                int UniversityId = _decodeToken.Decode(token, "UniversityId");

                DateTime localTime = new LocalTime().GetLocalTime().DateTime;
                double percentPoint = Double.Parse(_configuration.GetSection("StandardDepositedPoint:Difference").Value);

                if (string.IsNullOrEmpty(model.Name)
                    || string.IsNullOrEmpty(model.Content)
                    || string.IsNullOrEmpty(model.Address)
                    || string.IsNullOrEmpty(model.AddressName)
                    || model.MinTeamOrParticipant < 0
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint < 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null || MinTeamOfParticipant > 0 or Not Null" +
                                                    "||StartTimeRegister Null ||EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint > 0 or Not Null  || ClubId Null");

                //------------- CHECK Club in system
                Club club = await _clubRepo.Get(model.ClubId);
                if (club == null) throw new ArgumentException("Club in not found");

                //------------- CHECK Is Member in Club
                int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                Member member = await _memberRepo.Get(memberId);
                if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

                //------------ Check Role Member Is Leader Of Club
                if (member.ClubRoleId != 1) throw new UnauthorizedAccessException("You do not a role Leader to insert this Competititon");

                //----------------------------------------------------------------------------CREATE COMPETITION
                //------------ Check Date
                //StartTimeRegister == local time
                bool checkDate = CheckDate(localTime, model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime, false);
                if (checkDate == false) throw new ArgumentException("Date not suitable");

                //Check Competition Type 
                CompetitionType ct = await _competitionTypeRepo.Get(model.CompetitionTypeId);
                if (ct == null) throw new ArgumentException("Competition Type Id not have in System");

                //------------ Check FK

                // MajorId
                bool insertMajor;
                if (model.ListMajorId.Count > 0)
                {
                    //TH1: InterUniversity
                    if (model.Scope == CompetitionScopeStatus.InterUniversity)
                    {
                        bool Check = await _majorRepo.CheckMajor(model.ListMajorId);
                        if (Check)
                        {
                            insertMajor = true;
                        }
                        else
                        {
                            throw new ArgumentException("Major not have in System");
                        }
                    }
                    //TH2: University-Club
                    else
                    {
                        bool Check = await _majorRepo.CheckMajorBelongToUni(model.ListMajorId, UniversityId);
                        if (Check)
                        {
                            insertMajor = true;
                        }
                        else
                        {
                            throw new ArgumentException("Major Id not have in University");
                        }
                    }
                }
                else
                {
                    insertMajor = false;
                }

                //List Image 
                bool insertImage;
                if (model.ListImage.Count > 0)
                {

                    foreach (AddImageModel image in model.ListImage)
                    {

                        if (string.IsNullOrEmpty(image.Base64StringImg))
                            throw new ArgumentNullException("Image is NULL");
                    }
                    insertImage = true;
                }
                else
                {
                    insertImage = false;
                }

                //List Influencer 
                bool insertInfluencer;
                if (model.ListInfluencer.Count > 0)
                {

                    foreach (AddInfluencerModel influencer in model.ListInfluencer)
                    {

                        if (string.IsNullOrEmpty(influencer.Base64StringImg) || string.IsNullOrEmpty(influencer.Name))
                            throw new ArgumentNullException("Image of Influencer is NULL || Influencer name is NULL");
                    }
                    insertInfluencer = true;
                }
                else
                {
                    insertInfluencer = false;
                }


                //List Sponsor
                bool insertSponsor;
                if (model.ListSponsor.Count > 0)
                {

                    foreach (AddSponsorModel sponsor in model.ListSponsor)
                    {
                        if (string.IsNullOrEmpty(sponsor.Base64StringImg)
                           || string.IsNullOrEmpty(sponsor.Name)
                           || string.IsNullOrEmpty(sponsor.Email))
                            //|| string.IsNullOrEmpty(sponsor.Description)
                            //|| string.IsNullOrEmpty(sponsor.Website))
                            throw new ArgumentNullException("Image of Sponsor is NULL || Name is NULL || Email is NULL");
                    }
                    insertSponsor = true;
                }
                else
                {
                    insertSponsor = false;
                }



                //----------------------------------------------------------- Insert Competition
                //ở trong trường hợp này phân biệt EVENT - COMPETITION
                //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                Competition competition = new Competition();
                competition.CompetitionTypeId = model.CompetitionTypeId;
                competition.UniversityId = _decodeToken.Decode(token, "UniversityId");
                competition.AddressName = model.AddressName;
                competition.Address = model.Address;
                competition.Name = model.Name;
                if (model.IsEvent)
                {
                    competition.NumberOfTeam = 0;// 
                }
                else
                {
                    competition.NumberOfTeam = -1; // Offical Team is shown when Competition is in starting time
                }

                //Create Competition
                if (model.MaxNumberMemberInTeam.HasValue && model.MinNumberMemberInTeam.HasValue && model.IsEvent == false)
                {
                    bool checkMinMax = CheckMaxMin((int)model.MaxNumberMemberInTeam, (int)model.MinNumberMemberInTeam, model.NumberOfParticipations);
                    if (checkMinMax)
                    {
                        //--MaxMemberInTeam
                        competition.MaxNumber = model.MaxNumberMemberInTeam.Value;
                        //--MinMemberInTeam
                        competition.MinNumber = model.MinNumberMemberInTeam.Value;
                    }
                }
                //Create Event
                else
                {
                    //--MaxMemberInTeam
                    competition.MaxNumber = 0;
                    //--MinMemberInTeam
                    competition.MinNumber = 0;
                }
                competition.NumberOfParticipation = model.NumberOfParticipations;
                competition.RequiredMin = model.MinTeamOrParticipant;
                competition.CreateTime = localTime;
                competition.StartTimeRegister = model.StartTimeRegister;
                competition.EndTimeRegister = model.EndTimeRegister;
                competition.CeremonyTime = model.StartTime.AddMinutes(-30); // auto  30 minutes để state là Start
                competition.StartTime = model.StartTime;
                competition.EndTime = model.EndTime;
                competition.Content = model.Content;
                competition.Fee = model.Fee;
                competition.SeedsPoint = model.SeedsPoint;
                competition.SeedsDeposited = Math.Round(model.SeedsPoint * percentPoint, 0); // 20%
                competition.SeedsCode = await CheckExistCode();
                //isSponsor
                if (insertSponsor) competition.IsSponsor = true;
                if (insertSponsor == false) competition.IsSponsor = false;
                competition.Status = CompetitionStatus.Draft; // status draft
                competition.Scope = model.Scope; // change to Scope  1.InterUniversity, 2.University 3.Club
                competition.View = 0; // auto

                int competitionId = await _competitionRepo.Insert(competition);
                if (competitionId <= 0) throw new ArgumentException("Add Competition Or Event Failed");
                Competition comp = await _competitionRepo.Get(competitionId);

                //------------ Insert Competition-In-Major  
                if (insertMajor)
                {
                    foreach (int majorId in model.ListMajorId)
                    {
                        CompetitionInMajor competitionInMajor = new CompetitionInMajor()
                        {
                            MajorId = majorId,
                            CompetitionId = comp.Id
                        };
                        await _competitionInMajorRepo.Insert(competitionInMajor);
                    }
                }

                //------------Insert Image Entity
                if (insertImage)
                {
                    foreach (AddImageModel Image in model.ListImage)
                    {
                        string Url = await _fileService.UploadFile(Image.Base64StringImg);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {
                            CompetitionId = comp.Id,
                            Name = Image.Name,
                            ImageUrl = Url,
                            EntityTypeId = 1, //1 là image
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);
                    }
                }

                //------------ Insert Influencer
                if (insertInfluencer)
                {
                    foreach (AddInfluencerModel influencer in model.ListInfluencer)
                    {
                        string Url = await _fileService.UploadFile(influencer.Base64StringImg);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {
                            CompetitionId = comp.Id,
                            Name = influencer.Name,
                            ImageUrl = Url,
                            EntityTypeId = 2, //2 là influencer
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);
                    }
                }

                //------------ Insert Sponsor
                if (insertSponsor)
                {
                    foreach (AddSponsorModel sponsor in model.ListSponsor)
                    {
                        string Url = await _fileService.UploadFile(sponsor.Base64StringImg);
                        CompetitionEntity competitionEntity = new CompetitionEntity()
                        {

                            CompetitionId = comp.Id,
                            Name = sponsor.Name,
                            ImageUrl = Url,
                            EntityTypeId = 3, //3 là Sponsor
                            Description = sponsor.Description,
                            Website = sponsor.Website,
                            Email = sponsor.Email,
                        };

                        int id = await _competitionEntityRepo.Insert(competitionEntity);
                    }
                }

                //------------ Insert Competition-In-Club
                CompetitionInClub competitionInClub = new CompetitionInClub();
                competitionInClub.ClubId = model.ClubId;
                competitionInClub.CompetitionId = comp.Id;
                competitionInClub.IsOwner = true;
                int compInClub_Id = await _competitionInClubRepo.Insert(competitionInClub);

                //------------ Insert Member In Competition
                MemberInCompetition mic = new MemberInCompetition()
                {
                    CompetitionId = competitionId,
                    MemberId = member.Id,
                    CompetitionRoleId = 1,
                    Status = true,
                };
                int micId = await _memberInCompetitionRepo.Insert(mic);


                //----------- InsertCompetition History
                CompetitionHistory chim = new CompetitionHistory()
                {
                    CompetitionId = comp.Id,
                    ChangerId = member.Id,
                    ChangeDate = localTime,
                    Description = "Tạo" + ((model.IsEvent == true) ? " Sự Kiện" : " Cuộc Thi"), // auto des when create Competition
                    Status = CompetitionStatus.Draft,   // when create status draft
                };
                int result = await _competitionHistoryRepo.Insert(chim);
                if (result == 0) throw new ArgumentException("Add Competition History Failed");



                ViewDetailCompetition viewDetailCompetition = await TransferViewDetailCompetition(comp);
                return viewDetailCompetition;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCompetitionByState(LeaderUpdateCompOrEventModel model, string token)
        {
            try
            {
                if (model.Id == 0 || model.ClubId == 0) throw new ArgumentNullException("Competition Id Null  || ClubId Null");

                if (string.IsNullOrEmpty(model.Name)
                  || string.IsNullOrEmpty(model.Content)
                  || string.IsNullOrEmpty(model.Address)
                  || string.IsNullOrEmpty(model.AddressName)
                  || model.MinTeamOrParticipant < 0
                  || model.CompetitionTypeId == 0
                  || model.NumberOfParticipant == 0
                  || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.SeedsPoint < 0
                  || model.ClubId == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null || MinTeamOfParticipant > 0 or Not Null" +
                                                    "|| StartTimeRegister Null ||EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint > 0 or Not Null  || ClubId Null");

                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);
                if (Check == false) throw new ArgumentException("Update Failed !");

                int memId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                Member mem = await _memberRepo.Get(memId);

                Competition comp = await _competitionRepo.Get(model.Id);

                //-------------------------------------------------------State Draft
                //Update Everything
                if (comp.Status == CompetitionStatus.Draft)
                {
                    bool dateInsertCases = CheckDateInsertCases(comp, localTime, model);
                    if (dateInsertCases == false) throw new ArgumentException("Date not suitable");

                    bool numMinMaxCases = CheckNumMinMaxCases(comp, model);
                    if (numMinMaxCases == false) throw new ArgumentException("0 < min < max Or Number of participant > 0");

                    comp = await UpdateFieldCompetition(comp, model, token);

                    await _competitionRepo.Update();
                    return true;
                }

                //-------------------------------------------------------State Approve       

                if (comp.Status == CompetitionStatus.Approve)
                {
                    //Update Everything Except: Content, Scope
                    bool checkScope = (model.Scope != comp.Scope) ? true : false;
                    bool checkContent = string.Equals(model.Content, comp.Content);

                    //Update Content Scope
                    //Change to Pending Review
                    if (checkScope || checkContent == false)
                    {

                        bool dateInsertCases = CheckDateInsertCases(comp, localTime, model);
                        if (dateInsertCases == false) throw new ArgumentException("Date not suitable");

                        bool numMinMaxCases = CheckNumMinMaxCases(comp, model);
                        if (numMinMaxCases == false) throw new ArgumentException("0 < min < max Or Number of participant > 0");

                        comp = await UpdateFieldCompetition(comp, model, token);

                        comp.Status = CompetitionStatus.PendingReview;

                        await _competitionRepo.Update();

                        //----------- InsertCompetition History
                        CompetitionHistory chim = new CompetitionHistory()
                        {
                            CompetitionId = comp.Id,
                            ChangerId = mem.Id,
                            ChangeDate = new LocalTime().GetLocalTime().DateTime,
                            Description = "Thay đổi cập nhật thể lệ cuộc thi",
                            Status = CompetitionStatus.PendingReview,
                        };
                        int result = await _competitionHistoryRepo.Insert(chim);

                        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                        return true;
                    }
                    //Not Change to Pending Review
                    else
                    {
                        bool dateInsertCases = CheckDateInsertCases(comp, localTime, model);//CheckDateInsertCasesStateApprove(comp, localTime, model);
                        if (dateInsertCases == false) throw new ArgumentException("Date not suitable must be Present < STR < ETR < ST < ET");

                        bool numMinMaxCases = CheckNumMinMaxCases(comp, model);
                        if (numMinMaxCases == false) throw new ArgumentException("0 < min < max Or Number of participant > 0");

                        comp = await UpdateFieldCompetition(comp, model, token);

                        await _competitionRepo.Update();
                        return true;
                    }

                }
                //-------------------------------------------------------State Publish
                //Update Date, location, Number Of Participant, max ,min
                if (comp.Status == CompetitionStatus.Publish || comp.Status == CompetitionStatus.Register || comp.Status == CompetitionStatus.UpComing)
                {
                    bool dateInsertCases = CheckDateInsertCases(comp, localTime, model);
                    if (dateInsertCases == false) throw new ArgumentException("Date not suitable");

                    bool numMinMaxCases = CheckNumMinMaxCases(comp, model);
                    if (numMinMaxCases == false) throw new ArgumentException("0 < min < max Or Number of participant > 0");

                    //update những field cho phép
                    comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                    comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                    comp.CeremonyTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime.Value.AddMinutes(-30) : comp.StartTime);
                    comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                    comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                    comp.MaxNumber = (model.MaxNumber.HasValue) ? model.MaxNumber : comp.MaxNumber;
                    comp.MinNumber = (model.MinNumber.HasValue) ? model.MinNumber : comp.MinNumber;
                    comp.NumberOfParticipation = (int)((model.NumberOfParticipant.HasValue) ? model.NumberOfParticipant : comp.NumberOfParticipation);
                    comp.RequiredMin = (int)(model.MinTeamOrParticipant.HasValue ? model.MinTeamOrParticipant : comp.RequiredMin);
                    comp.AddressName = (!string.IsNullOrEmpty(model.AddressName)) ? model.AddressName : comp.AddressName;
                    comp.Address = (!string.IsNullOrEmpty(model.Address)) ? model.Address : comp.Address;

                    await _competitionRepo.Update();
                    return true;
                }

                //-------------------------------------------------------State Start - OnGoing
                //State Start - OnGoing
                //Update Date
                if (comp.Status == CompetitionStatus.Start || comp.Status == CompetitionStatus.OnGoing)
                {             
                    //Xem đã đúng form chưa 
                    bool formDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, true);

                    if (formDate == false) throw new ArgumentException("Ngày tháng không hợp lệ phải theo quy tắc STR < ETR < ST < ET, và các mốc thời gian phải cách nhau đúng 1 giờ");

                    //check với round cuối cùng trong cuộc thi

                    //update những field cho phép
                    comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                    await _competitionRepo.Update();
                    return true;
                  
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //------Club Collaborate
        public async Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token)
        {
            try
            {
                if (model.ClubIdCollaborate == 0
                   || model.CompetitionId == 0
                   || model.ClubId == 0)
                    throw new ArgumentNullException("Club Id Collaborate Null || Competition Id Null || Club Id Null");

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);
                if (Check)
                {
                    //Chỉ Cho những Trạng Thái này update những trạng thái trước khi publish
                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    if (comp.Status == CompetitionStatus.Draft || comp.Status == CompetitionStatus.Approve)
                    {
                        //add 2 parameter to check
                        int UniversityId = _decodeToken.Decode(token, "UniversityId");
                        Competition competition = await _competitionRepo.Get(model.CompetitionId);

                        //------------ CHECK 2 club are the same 
                        if (model.ClubIdCollaborate == model.ClubId) throw new ArgumentException("Club is the same");

                        //---------------CHECK Club-Id-Collaborate
                        //CHECK club Id Collaborate has in system
                        Club clubCollaborate = await _clubRepo.Get(model.ClubIdCollaborate);
                        if (clubCollaborate == null) throw new ArgumentException("Club collaborate not found in system");

                        //------------- CHECK club Id Collaborate has in Competition
                        ViewCompetitionInClub vcic = await _competitionInClubRepo.GetCompetitionInClub(clubCollaborate.Id, model.CompetitionId);
                        if (vcic != null) throw new ArgumentException("Club has join in Competition");


                        //Scope != inter => Check ClubCollaborate University
                        bool checkClubIn_Out = false;
                        if (competition.Scope != CompetitionScopeStatus.InterUniversity)
                        {
                            if (clubCollaborate.UniversityId == UniversityId)

                            {
                                checkClubIn_Out = true;
                            }
                            else
                            {
                                checkClubIn_Out = false;
                            }
                        }
                        //Scope  == Inter => all join
                        else
                        {
                            checkClubIn_Out = true;
                        }
                        if (checkClubIn_Out == false) throw new ArgumentException("Club collaborate not in University");

                        CompetitionInClub competitionInClub = new CompetitionInClub();
                        competitionInClub.ClubId = model.ClubIdCollaborate;
                        competitionInClub.CompetitionId = model.CompetitionId;
                        competitionInClub.IsOwner = false;
                        int result = await _competitionInClubRepo.Insert(competitionInClub);

                        //------------- CHECK add 
                        if (result <= 0) throw new ArgumentException("Add Competition Or Event Failed");

                        CompetitionInClub cic = await _competitionInClubRepo.Get(result);
                        //Add Club Manager Of Club Collaborate in Competition Manager
                        Member clubLeaderCollaborate = await _memberRepo.GetLeaderByClub(model.ClubIdCollaborate);

                        //------------ Insert Member In Competition
                        MemberInCompetition mic = new MemberInCompetition()
                        {
                            CompetitionId = model.CompetitionId,
                            MemberId = clubLeaderCollaborate.Id,
                            CompetitionRoleId = 1,
                            Status = true,
                        };
                        await _memberInCompetitionRepo.Insert(mic);

                        return TransferViewCompetitionInClub(cic);
                    }
                    else
                    {
                        throw new ArgumentException("Competition State is not suitable to do this action");
                    }

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

        public async Task<bool> DeleteClubCollaborate(int CompetitionInClubId, int ClubId, string token)
        {
            try
            {

                //
                CompetitionInClub competitionInClub = await _competitionInClubRepo.Get(CompetitionInClubId);
                if (competitionInClub == null) throw new ArgumentException("Competition In Club Id Not Found");

                //không cho xóa club is owner == true
                if (competitionInClub.IsOwner == true) throw new ArgumentException("Can't delete Club Owner Competition");

                //
                Competition comp = await _competitionRepo.Get(competitionInClub.CompetitionId);
                if (comp.Status == CompetitionStatus.Draft || comp.Status == CompetitionStatus.Approve)
                {
                    //
                    bool Check = await CheckMemberInCompetition(token, comp.Id, ClubId, true);
                    if (Check == false) throw new ArgumentException("Delete Failed");
                    await _competitionInClubRepo.DeleteCompetitionInClub(CompetitionInClubId);
                    return true;
                }
                else
                {
                    throw new ArgumentException("Competition State is not suitable to do this action");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-----Major in Competition
        public async Task<List<ViewCompetitionInMajor>> AddCompetitionInMajor(CompetitionInMajorInsertModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0 || model.ClubId == 0 || model.ListMajorId.Count < 0)
                    throw new ArgumentNullException("Competition Id Null || List Major Id Null || ClubId Null");

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);
                if (Check)
                {
                    ////------------- CHECK Status Competition

                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    //if (comp.Status != CompetitionStatus.Draft || comp.Status != CompetitionStatus.Approve)
                    //    throw new ArgumentException("Competition State is not suitable to do this action");


                    //------------ CHECK Scope
                    //TH 1
                    if (comp.Scope == CompetitionScopeStatus.InterUniversity)
                    {
                        bool approve = await _majorRepo.CheckMajor(model.ListMajorId);
                        if (approve == false) throw new ArgumentException("Major Id not have in System");
                    }
                    // TH 2
                    else
                    {
                        //add extra parameter
                        int UniversityId = _decodeToken.Decode(token, "UniversityId");
                        //------------- CHECK List Major Id belong to University
                        bool majorBelongToUni = await _majorRepo.CheckMajorBelongToUni(model.ListMajorId, UniversityId);
                        if (majorBelongToUni == false) throw new ArgumentException("Major Id not have in University");
                    }

                    //------------- CHECK Add Major in Competition is existed
                    bool majorIsExsited = true;
                    foreach (int majorId in model.ListMajorId)
                    {
                        //
                        CompetitionInMajor cim = await _competitionInMajorRepo.GetMajorInCompetition(majorId, model.CompetitionId);
                        if (cim == null) majorIsExsited = false;
                    }

                    //------------- CHECK Major In Competition Is Exsited
                    if (majorIsExsited) throw new ArgumentException("Major already in Competition");

                    List<int> listCompetitionInMajorId = new List<int>();

                    List<ViewCompetitionInMajor> listResult = new List<ViewCompetitionInMajor>();

                    foreach (int majorId in model.ListMajorId)
                    {
                        CompetitionInMajor comInMaj = new CompetitionInMajor()
                        {
                            MajorId = majorId,
                            CompetitionId = model.CompetitionId
                        };
                        int id = await _competitionInMajorRepo.Insert(comInMaj);
                        listCompetitionInMajorId.Add(id);
                    }

                    //------------- CHECK Add List Major in Competition is Success
                    if (listCompetitionInMajorId.Count < 0) throw new ArgumentException("Add in DB Department Failed");

                    foreach (int id in listCompetitionInMajorId)
                    {
                        CompetitionInMajor cim = await _competitionInMajorRepo.Get(id);

                        ViewCompetitionInMajor vcim = new ViewCompetitionInMajor()
                        {
                            Id = cim.Id,
                            CompetitionId = cim.CompetitionId,
                            MajorId = cim.MajorId
                        };
                        listResult.Add(vcim);
                    }
                    return listResult;
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

        public async Task<bool> DeleteMajorInCompetition(CompetitionInMajorDeleteModel model, string token)
        {
            try
            {
                //
                if (model.CompetitionInMajorId == 0 || model.ClubId == 0)
                    throw new ArgumentNullException("Competition In Major Id NULL || Club Id NULL");
                //
                CompetitionInMajor competitionInMajor = await _competitionInMajorRepo.Get(model.CompetitionInMajorId);
                if (competitionInMajor == null) throw new ArgumentException("Competition In Major Id Not Found");

                //
                Competition comp = await _competitionRepo.Get(competitionInMajor.CompetitionId);
                //if (comp.Status != CompetitionStatus.Draft || comp.Status != CompetitionStatus.Approve)
                //    throw new ArgumentException("Competition State is not suitable to do this action");

                //
                bool Check = await CheckMemberInCompetition(token, comp.Id, model.ClubId, true);
                if (Check == false) throw new ArgumentException("Delete Failed");

                await _competitionInMajorRepo.DeleteCompetitionInMajor(model.CompetitionInMajorId);
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        //---------------------------------------------------------------------------------- STATE chỉnh bằng tay
        //State Pending Review - done
        //State Approve - done
        //State Publish - done
        //State Pending - done
        //State Finish  - done
        //State Evaluate - done 
        //State Complete - done
        //State Canceling - done

        //Admin Change to Approve or Draft
        public async Task<bool> ChangeStateByAdminUni(AdminUniUpdateCompetitionStatusModel model, string token)
        {
            try
            {
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                //user
                User adminUni = await _userRepo.Get(_decodeToken.Decode(token, "Id"));

                if (model.Id == 0 || !model.Status.HasValue) throw new ArgumentNullException("Competition Id Null || Status Null");

                Competition comp = await _competitionRepo.Get(model.Id);
                if (comp == null) throw new ArgumentException("Competition not found");

                int UniversityId = _decodeToken.Decode(token, "UniversityId");
                if (UniversityId != comp.UniversityId) throw new ArgumentException("Competition is not belong to your University");

                if (comp.Status != CompetitionStatus.PendingReview) throw new ArgumentException("Update Status of Competition Failed");

                if (model.Status.Value == CompetitionStatus.Approve)
                {
                    comp.Status = CompetitionStatus.Approve;
                    await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = adminUni.Id,
                        ChangeDate = localTime,
                        Description = "Admin University cập nhật Trạng Thái Duyệt",
                        Status = CompetitionStatus.Approve,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");

                    // send notification
                    Member member = await _memberRepo.GetLeaderClubOwnerByCompetition(model.Id);
                    string deviceToken = await _userRepo.GetDeviceTokenByUser(member.UserId);
                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        string body = $"Cuộc thi {comp.Name} của bạn vừa được duyệt bởi Admin";
                        Notification notification = new Notification()
                        {
                            Title = "Thông báo",
                            Body = body,
                            RedirectUrl = "/notification",
                            UserId = member.UserId,
                        };
                        await _notificationService.SendNotification(notification, deviceToken);
                    }

                    return true;

                }

                if (model.Status.Value == CompetitionStatus.Draft)
                {
                    comp.Status = CompetitionStatus.Draft;
                    await _competitionRepo.Update();
                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = adminUni.Id,
                        ChangeDate = localTime,
                        Description = "Admin University cập nhật Trạng Thái Bản Thảo",
                        Status = CompetitionStatus.Draft,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");

                    // send notification
                    Member member = await _memberRepo.GetLeaderClubOwnerByCompetition(model.Id);
                    string deviceToken = await _userRepo.GetDeviceTokenByUser(member.UserId);
                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        string body = $"Cuộc thi {comp.Name} của bạn vừa bị từ chối bởi Admin";
                        Notification notification = new Notification()
                        {
                            Title = "Thông báo",
                            Body = body,
                            RedirectUrl = "/notification",
                            UserId = member.UserId,
                        };
                        await _notificationService.SendNotification(notification, deviceToken);
                    }

                    return true;
                }

                throw new ArgumentException("State condition : Draft - Approve can update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CompetitionStatusUpdate(CompetitionStatusUpdateModel model, string token)
        {
            try
            {
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                //
                if (model.Id == 0 || model.ClubId == 0 || !model.Status.HasValue) throw new ArgumentNullException("Competition Id Null  || ClubId Null || Status Null");

                //
                bool check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);
                if (check == false) throw new ArgumentException("Update Status Competition Failed");

                int memId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                Member member = await _memberRepo.Get(memId);

                Competition comp = await _competitionRepo.Get(model.Id);

                if (comp == null) throw new ArgumentException("Competition not found");

                //Check Competition Status hiện tại 
                //if (comp.Status != CompetitionStatus.Pending)
                //{
                //-----------------------------------------------------------------Draft
                //State Conditition : Pending Review
                if (model.Status == CompetitionStatus.Draft)
                {
                    if (comp.Status == CompetitionStatus.PendingReview || comp.Status == CompetitionStatus.Cancel)
                    {
                        comp.Status = CompetitionStatus.Draft;
                        await _competitionRepo.Update();

                        // Update status rounds in competition
                        await _competitionRoundRepo.UpdateStatusRoundByCompe(comp.Id, CompetitionRoundStatus.Active);

                        // Update status matches in rounds
                        await _matchRepo.UpdateStatusMatchesByComp(comp.Id, MatchStatus.Ready);

                        //----------- InsertCompetition History
                        CompetitionHistory chim = new CompetitionHistory()
                        {
                            CompetitionId = comp.Id,
                            ChangerId = member.Id,
                            ChangeDate = localTime,
                            Description = //member.User.Fullname + 
                            "Trạng thái Bản Thảo",
                            Status = CompetitionStatus.Draft,
                        };
                        int result = await _competitionHistoryRepo.Insert(chim);
                        if (result == 0) throw new ArgumentException(" Add Competition History Failed");
                        return true;
                    }
                    else { throw new ArgumentException("Competition can't change to Draft Status"); }

                }

                //-----------------------------------------------------------------Pending Review
                //State Conditition : Draft
                if (model.Status == CompetitionStatus.PendingReview)
                {
                    if (comp.Status != CompetitionStatus.Draft) throw new ArgumentException("Competition can't change to Pending Review Status");
                    comp.Status = CompetitionStatus.PendingReview;
                    await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = localTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Chờ Duyệt",
                        Status = CompetitionStatus.PendingReview,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    return true;
                }

                //-----------------------------------------------------------------Publish
                //State Condition : Approve
                if (model.Status == CompetitionStatus.Publish)
                {
                    if (comp.Status != CompetitionStatus.Approve) throw new ArgumentException("Competition can't change to Publish Status");

                    bool checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, comp.EndTime, false);
                    if (checkDate == false) throw new ArgumentException("Date Now < Time Register < Time Start < Time End");

                    comp.Status = CompetitionStatus.Publish;
                    await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = localTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Công Bố",
                        Status = CompetitionStatus.Publish,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    return true;
                }


                //-----------------------------------------------------------------Pending
                //State Condition : Puplish, Register, UpComing
                if (model.Status == CompetitionStatus.Pending)
                {
                    if (comp.Status == CompetitionStatus.Publish || comp.Status == CompetitionStatus.Register || comp.Status == CompetitionStatus.UpComing)
                    {
                        comp.Status = CompetitionStatus.Pending;
                        await _competitionRepo.Update();

                        //----------- InsertCompetition History
                        CompetitionHistory chim = new CompetitionHistory()
                        {
                            CompetitionId = comp.Id,
                            ChangerId = member.Id,
                            ChangeDate = localTime,
                            Description = //member.User.Fullname + 
                            "Trạng thái Chờ",
                            Status = CompetitionStatus.Pending,
                        };
                        int result = await _competitionHistoryRepo.Insert(chim);
                        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("Competition can't change to Complete Status");
                    }
                }


                //-----------------------------------------------------------------Finish
                //State Condition : End
                if (model.Status == CompetitionStatus.Finish)
                {
                    if (comp.Status != CompetitionStatus.End) throw new ArgumentException("Competition can't change to End Status");

                    comp.Status = CompetitionStatus.Finish;
                    await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = new LocalTime().GetLocalTime().DateTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Hoàn Thành",
                        Status = CompetitionStatus.Finish,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");

                    //----------- Add Seeds Ponit to Participant
                    //Condition IsPresent = true, and In Team status = InTeam

                    //1. Get List Participant
                    List<Participant> participants = await _participantRepo.ListParticipantToAddPoint(comp.Id);
                    if (participants != null)
                    {
                        foreach (Participant participant in participants)
                        {
                            await _seedsWalletService.UpdateAmount(participant.StudentId, comp.SeedsPoint);
                        }
                    }
                    return true;
                }


                //-----------------------------------------------------------------Complete
                //State Condition : Finish
                if (model.Status == CompetitionStatus.Complete)
                {
                    if (comp.Status != CompetitionStatus.Finish) throw new ArgumentException("Competition can't change to Complete Status");
                    comp.Status = CompetitionStatus.Complete;
                    await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = new LocalTime().GetLocalTime().DateTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Đóng",
                        Status = CompetitionStatus.Complete,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    return true;

                }

                //-----------------------------------------------------------------Cancel
                //State Condition : Draft - Pending Review - Approve - Publish - Register - UpComing - Pending
                if (model.Status == CompetitionStatus.Cancel)
                {

                    //Ở trạng thái này thì có thể xóa
                    if (comp.Status == CompetitionStatus.Draft
                     || comp.Status == CompetitionStatus.PendingReview
                     || comp.Status == CompetitionStatus.Approve
                     || comp.Status == CompetitionStatus.Publish
                     || comp.Status == CompetitionStatus.Register
                     || comp.Status == CompetitionStatus.UpComing
                     || comp.Status == CompetitionStatus.Pending)
                    {
                        //nếu ở 2 trạng thái này mà hủy thì sẽ hoàn lại phí seedpoint đăng ký nếu có
                        if (comp.Status == CompetitionStatus.Register || comp.Status == CompetitionStatus.UpComing)
                        {
                            //lấy thông tin ng tham gia
                            List<Participant> participants = await _participantRepo.ListParticipant(comp.Id);
                            //return point
                            if (participants != null)
                            {
                                foreach (Participant participant in participants)
                                {
                                    await _seedsWalletService.UpdateAmount(participant.StudentId, comp.SeedsDeposited);
                                }
                            }
                        }


                        comp.Status = CompetitionStatus.Cancel;
                        await _competitionRepo.Update();

                        // Update status rounds in competition
                        await _competitionRoundRepo.UpdateStatusRoundByCompe(comp.Id, CompetitionRoundStatus.Cancel);

                        // Update status matches in rounds
                        await _matchRepo.UpdateStatusMatchesByComp(comp.Id, MatchStatus.Cancel);

                        //----------- InsertCompetition History
                        CompetitionHistory chim = new CompetitionHistory()
                        {
                            CompetitionId = comp.Id,
                            ChangerId = member.Id,
                            ChangeDate = new LocalTime().GetLocalTime().DateTime,
                            Description = //member.User.Fullname + 
                            "Trạng thái Hủy",
                            Status = CompetitionStatus.Cancel,
                        };

                        int result = await _competitionHistoryRepo.Insert(chim);
                        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException("Competition can't change to Complete Status");
                    }
                }
                //}
                ////Competition Status hiện tại == Pending
                ////---------------------------------------------------------------------State : Pending
                //else
                //{
                //    //lấy nearest State of Competition before Pending
                //    CompetitionHistory compeHis = await _competitionHistoryRepo.GetNearestStateAfterPending(comp.Id);

                //    //-----------------------------------------------------------------Puplish
                //    if (model.Status == CompetitionStatus.Publish && compeHis.Status == CompetitionStatus.Publish)
                //    {
                //        bool checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, comp.EndTime, false);
                //        if (checkDate == false) throw new ArgumentException("Date in Competition not suitable");


                //        comp.Status = CompetitionStatus.Publish;
                //        await _competitionRepo.Update();

                //        //----------- InsertCompetition History
                //        CompetitionHistory chim = new CompetitionHistory()
                //        {
                //            CompetitionId = comp.Id,
                //            ChangerId = member.Id,
                //            ChangeDate = localTime,
                //            Description = member.User.Fullname + " Update Status Publish",
                //            Status = CompetitionStatus.Publish,
                //        };
                //        int result = await _competitionHistoryRepo.Insert(chim);
                //        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                //        return true;
                //    }


                //    //-----------------------------------------------------------------Register
                //    if ((model.Status == CompetitionStatus.Register && compeHis.Status == CompetitionStatus.Register)
                //      || (model.Status == CompetitionStatus.Register && compeHis.Status == CompetitionStatus.UpComing))
                //    {
                //        bool checkStateRegister = CheckStateRegister(localTime, comp.StartTimeRegister, comp.EndTimeRegister);
                //        if (checkStateRegister == false) throw new ArgumentException("Date in Competition not suitable");


                //        comp.Status = CompetitionStatus.Register;
                //        await _competitionRepo.Update();

                //        //----------- InsertCompetition History
                //        CompetitionHistory chim = new CompetitionHistory()
                //        {
                //            CompetitionId = comp.Id,
                //            ChangerId = member.Id,
                //            ChangeDate = localTime,
                //            Description = member.User.Fullname + " Update Status Register",
                //            Status = CompetitionStatus.Register,
                //        };
                //        int result = await _competitionHistoryRepo.Insert(chim);
                //        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                //        return true;
                //    }


                //    throw new ArgumentException("The Nearest Status Of Competition is " + CompetitionStatusToString(compeHis.Status) + ", you must update from this Status");
                //    //-----------------------------------------------------------------Up - Comming
                //    if ((model.Status == CompetitionStatus.UpComing && compeHis.Status == CompetitionStatus.Register)
                //      || (model.Status == CompetitionStatus.UpComing && compeHis.Status == CompetitionStatus.UpComing))
                //    {
                //        bool checkStateUpComing = CheckStateUpComing(localTime, comp.EndTimeRegister, comp.CeremonyTime);
                //        if (checkStateUpComing == false) throw new ArgumentException("Date in Competition not suitable");

                //        comp.Status = CompetitionStatus.UpComing;
                //        await _competitionRepo.Update();

                //        //-----------InsertCompetition History
                //        CompetitionHistory chim = new CompetitionHistory()
                //        {
                //            CompetitionId = comp.Id,
                //            ChangerId = member.Id,
                //            ChangeDate = localTime,
                //            Description = member.User.Fullname + " Update Status UpComing",
                //            Status = CompetitionStatus.UpComing,
                //        };
                //        int result = await _competitionHistoryRepo.Insert(chim);
                //        if (result == 0) throw new ArgumentException("Add Competition History Failed");
                //        return true;
                //    }
                //}
                throw new ArgumentException("Can not update this Status at present !");

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> CompetitionUpdateStatusAfterPending(UpdateCompetitionWithStatePendingModel model, string token)
        {
            try
            {
                if (model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                  || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                 )
                    throw new ArgumentNullException("StartTimeRegister Null || EndTimeRegister Null  || StartTime Null || EndTime Null");

                DateTime localTime = new LocalTime().GetLocalTime().DateTime;
                //
                await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);

                int memId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                Member member = await _memberRepo.Get(memId);

                Competition comp = await _competitionRepo.Get(model.CompetitionId);
                if (comp == null) throw new ArgumentException("Competition not found");

                //Check Competition Status hiện tại 
                if (comp.Status != CompetitionStatus.Pending) throw new ArgumentException("API only Support Competition Or Event has State Pending");

                //lấy nearest State of Competition before Pending
                //CompetitionHistory compeHis = await _competitionHistoryRepo.GetNearestStateAfterPending(comp.Id);
                //if (compeHis == null) throw new ArgumentException("Competition History Lost Data");

                //if (status == CompetitionStatus.Publish || status == CompetitionStatus.Register || status == CompetitionStatus.UpComing)
                //{
                //if (compeHis.Status == CompetitionStatus.Approve)
                //{
                //    comp.Status = CompetitionStatus.Approve;
                //    await _competitionRepo.Update();

                //    //----------- InsertCompetition History
                //    CompetitionHistory chim = new CompetitionHistory()
                //    {
                //        CompetitionId = comp.Id,
                //        ChangerId = member.Id,
                //        ChangeDate = localTime,
                //        Description = //member.User.Fullname + 
                //        "Trạng thái Chờ sang Duyệt",
                //        Status = CompetitionStatus.Approve,
                //    };
                //    int result = await _competitionHistoryRepo.Insert(chim);
                //    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                //    return true;
                //}

                //check xem các mốc thời gian có đúng form hay không STR < ETR < ST < ET
                bool checkRuleDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, true);
                if (checkRuleDate == false) throw new ArgumentException("Ngày tháng không hợp lệ phải theo quy tắc STR < ETR < ST < ET");

                //chỉ có 3 trạng thái được quay về
                //Puplish
                bool statePuplish = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);

                //Register
                bool stateRegister = CheckStateRegister(localTime, model.StartTimeRegister.Value, model.StartTimeRegister.Value);

                //Up-Coming
                bool stateUpComing = CheckStateUpComing(localTime, model.StartTimeRegister.Value, model.StartTime.Value.AddMinutes(-30));

                //-----------------------------------------------------------------Puplish
                if (statePuplish)
                {
                    bool checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);
                    if (checkDate == false) throw new ArgumentException("Thời gian hiện tại không phù hợp, Now < STR < ETR < ET < ETR, các mốc thời gian cách nhau 1 giờ");

                    comp.Status = CompetitionStatus.Publish;
                    //await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = localTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Chờ sang Công Bố",
                        Status = CompetitionStatus.Publish,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    //return true;
                }

                //-----------------------------------------------------------------Register
                if (stateRegister)
                {
                    bool checkStateRegister = CheckStateRegister(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value);
                    if (checkStateRegister == false) throw new ArgumentException("Thời gian hiện tại không phù hợp, StartTimeRegister < Now < EndTimeRegister, các mốc thời gian cách nhay 1 giờ");


                    comp.Status = CompetitionStatus.Register;
                    //await _competitionRepo.Update();

                    //----------- InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = localTime,
                        Description = //member.User.Fullname +
                        "Trạng thái Chờ sang Mở Đăng Ký",
                        Status = CompetitionStatus.Register,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    //return true;
                }


                //-----------------------------------------------------------------Up - Comming
                if (stateUpComing)
                {
                    bool checkStateUpComing = CheckStateUpComing(localTime, model.EndTimeRegister.Value, model.StartTime.Value.AddMinutes(-30));
                    if (checkStateUpComing == false) throw new ArgumentException("Thời gian hiện tại không phù hợp, EndTimeRegister < Now < StartTime, các mốc thời gian cách nhau 1 giờ");

                    comp.Status = CompetitionStatus.UpComing;
                    //await _competitionRepo.Update();

                    //-----------InsertCompetition History
                    CompetitionHistory chim = new CompetitionHistory()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = member.Id,
                        ChangeDate = localTime,
                        Description = //member.User.Fullname + 
                        "Trạng thái Chờ sang Sắp Diễn Ra",
                        Status = CompetitionStatus.UpComing,
                    };
                    int result = await _competitionHistoryRepo.Insert(chim);
                    if (result == 0) throw new ArgumentException("Add Competition History Failed");
                    //return true;
                }
                //}

                //update competition
                if (statePuplish || stateRegister || stateUpComing)
                {
                    //update những field cho phép
                    comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                    comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                    comp.CeremonyTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime.Value.AddMinutes(-30) : comp.StartTime);
                    comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                    comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                    await _competitionRepo.Update();
                    return true;
                }
                else
                {
                    throw new ArgumentException("Chỉ cập nhật được thời gian nằm trong trạng thái là Puplish, Register, Up-coming thôi");
                }

            }
            catch (Exception)
            {
                throw;
            }

        }



        //HÀM NÀY CHỈ TEST UPDATE THÔI
        public async Task<bool> UpdateBE(LeaderUpdateCompOrEventModel model, string token)
        {
            try
            {
                if (model.Id == 0
                   || model.ClubId == 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null");
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);
                if (Check)
                {

                    //Chỉ Cho những Trạng Thái này update
                    Competition comp = await _competitionRepo.Get(model.Id);
                    if (comp.Status != CompetitionStatus.Draft) throw new ArgumentException("Competition State is not suitable to do this action");

                    bool checkDate = false;
                    //------------- CHECK Date Update
                    //------------- BE TEST
                    //TH1 STR
                    if (model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && !model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, comp.StartTime, comp.EndTime, true);
                    }
                    //TH2 ETR
                    if (!model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && !model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, comp.StartTime, comp.EndTime, true);
                    }
                    //TH3 ST
                    if (!model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, model.StartTime.Value, comp.EndTime, true);
                    }
                    //TH4 ET
                    if (!model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && !model.StartTime.HasValue && model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, true);
                    }
                    //TH5 not thing happen with date
                    if (!model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && !model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = true;
                    }
                    //TH 6 All
                    if (model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && model.StartTime.HasValue && model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, true);
                    }
                    if (checkDate)
                    {
                        //------------- CHECK Max,Min,NumberOfParticipant Update                          
                        bool checkMaxMin = false;

                        //------------- BE TEST
                        //Max
                        if (model.MaxNumber.HasValue && !model.MinNumber.HasValue && !model.NumberOfParticipant.HasValue)
                        {
                            checkMaxMin = CheckMaxMin(model.MaxNumber.Value, (int)comp.MinNumber, comp.NumberOfParticipation);

                        }
                        //Min
                        if (!model.MaxNumber.HasValue && model.MinNumber.HasValue && !model.NumberOfParticipant.HasValue)
                        {
                            checkMaxMin = CheckMaxMin((int)comp.MaxNumber, model.MinNumber.Value, comp.NumberOfParticipation);

                        }
                        //NumberOfParticipant
                        if (!model.MaxNumber.HasValue && !model.MinNumber.HasValue && model.NumberOfParticipant.HasValue)
                        {
                            checkMaxMin = CheckMaxMin((int)comp.MaxNumber, (int)comp.MinNumber, model.NumberOfParticipant.Value);
                        }
                        //Max,Min,NumberOfParticipant no Update
                        if (!model.MaxNumber.HasValue && !model.MinNumber.HasValue && !model.NumberOfParticipant.HasValue)
                        {
                            checkMaxMin = true;
                        }
                        //Max Min Num Update
                        if (model.MaxNumber.HasValue && model.MinNumber.HasValue && model.NumberOfParticipant.HasValue)
                        {
                            checkMaxMin = CheckMaxMin(model.MaxNumber.Value, model.MinNumber.Value, model.NumberOfParticipant.Value);

                        }
                        if (checkMaxMin)
                        {
                            comp.SeedsPoint = (double)((model.SeedsPoint.HasValue) ? model.SeedsPoint : comp.SeedsPoint);
                            //comp.SeedsDeposited = (double)((model.SeedsDeposited.HasValue) ? model.SeedsDeposited : comp.SeedsDeposited);
                            comp.AddressName = (!string.IsNullOrEmpty(model.AddressName)) ? model.AddressName : comp.AddressName;
                            comp.Address = (!string.IsNullOrEmpty(model.Address)) ? model.Address : comp.Address;
                            comp.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : comp.Name;
                            comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                            comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                            comp.CeremonyTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime.Value.AddMinutes(30) : comp.StartTime);
                            comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                            comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                            comp.Content = (!string.IsNullOrEmpty(model.Content)) ? model.Content : comp.Content;
                            comp.Fee = (double)((model.Fee.HasValue) ? model.Fee : comp.Fee);
                            comp.MaxNumber = (model.MaxNumber.HasValue) ? model.MaxNumber : comp.MaxNumber;
                            comp.MinNumber = (model.MinNumber.HasValue) ? model.MinNumber : comp.MinNumber;
                            comp.NumberOfParticipation = (int)((model.NumberOfParticipant.HasValue) ? model.NumberOfParticipant : comp.NumberOfParticipation);
                            await _competitionRepo.Update();
                            return true;
                        }
                        else
                        {
                            throw new ArgumentException("Max Number or Min Number or Number Of Participant is not suitable");
                        }
                    }//end check date
                    else
                    {
                        throw new ArgumentException("Date not suitable");
                    }

                }//end if check
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


        //-----------------------------------------------------------Manager In Competition
        public async Task<PagingResult<ViewMemberInCompetition>> GetAllManagerCompOrEve(MemberInCompetitionRequestModel request, string token)
        {
            try
            {
                if (request.CompetitionId == 0 || request.ClubId == 0) throw new ArgumentNullException("Competition Id Null || ClubId Null");

                bool check = await CheckMemberInCompetition(token, request.CompetitionId, request.ClubId, true);
                if (check)
                {
                    PagingResult<ViewMemberInCompetition> result = await _memberInCompetitionRepo.GetAllManagerCompOrEve(request);
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

        public async Task<ViewMemberInCompetition> AddMemberInCompetition(MemberInCompetitionInsertModel model, string token)
        {
            try
            {
                int UserId = _decodeToken.Decode(token, "Id");
                if (model.CompetitionId == 0
                        || model.ClubId == 0
                        || model.MemberId == 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null ||Member Id Null");

                bool check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);
                if (check)
                {
                    //------------- CHECK Id Member is added
                    Member mem = await _memberRepo.Get(model.MemberId);
                    if (mem == null) throw new ArgumentException("Member not found");

                    //------------- CHECK Id Member in club 
                    if (mem.ClubId != model.ClubId) throw new ArgumentException("Member is not in club");

                    //------------- CHECK Member has joined
                    MemberInCompetition isMemberInCompetition = await _memberInCompetitionRepo.GetMemberInCompetition(model.CompetitionId, mem.Id);
                    if (isMemberInCompetition != null) throw new ArgumentException("Member has already joined");

                    MemberInCompetition memberInCompetition = new MemberInCompetition();
                    memberInCompetition.CompetitionRoleId = 3;
                    memberInCompetition.MemberId = mem.Id;
                    memberInCompetition.CompetitionId = model.CompetitionId;
                    memberInCompetition.Status = true;

                    int result = await _memberInCompetitionRepo.Insert(memberInCompetition);
                    if (result <= 0) throw new ArgumentException("add failed");
                    return TransferViewMemberInCompetition(memberInCompetition);

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

        public async Task<bool> UpdateMemberInCompetition(MemberInCompetitionUpdateModel model, string token)
        {
            try
            {
                int UserId = _decodeToken.Decode(token, "Id");

                if (model.MemberInCompetitionId == 0 || model.ClubId == 0) throw new ArgumentNullException("Member In Competition Id Null ||ClubId Null");

                //------------- CHECK in system
                MemberInCompetition mic = await _memberInCompetitionRepo.Get(model.MemberInCompetitionId);
                if (mic == null) throw new ArgumentException("Record not found");

                //------------- CHECK 
                bool check = await CheckMemberInCompetition(token, mic.CompetitionId, model.ClubId, true);
                if (check)
                {
                    //------------- CHECK update by your self
                    int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId);
                    Member member = await _memberRepo.Get(memberId);
                    if (member.Id == mic.MemberId) throw new ArgumentException("Member id is the same you");

                    //------------- Update
                    if (model.RoleCompetitionId.HasValue)
                    {
                        //------------- CHECK Competition Role
                        CompetitionRole competitionRole = await _competitionRoleRepo.Get(model.RoleCompetitionId.Value);
                        if (competitionRole == null) throw new ArgumentException("Competition Role not have in system");

                        //------------- CHECK Can't update to role highest
                        if (competitionRole.Id == 1) throw new ArgumentException("You don't have permission to update this role");

                        //Manager update role for this member
                        mic.CompetitionRoleId = model.RoleCompetitionId.Value;
                    }
                    else
                    {
                        mic.CompetitionRoleId = mic.CompetitionRoleId;
                    }
                    mic.Status = (model.Status.HasValue) ? model.Status.Value : mic.Status;
                    await _memberInCompetitionRepo.Update();
                    return true;
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

        //transfer view

        private ViewMemberInCompetition TransferViewMemberInCompetition(MemberInCompetition memberInCompetition)
        {
            return new ViewMemberInCompetition()
            {
                Id = memberInCompetition.Id,
                CompetitionRoleId = memberInCompetition.CompetitionRoleId,
                CompetitionRoleName = memberInCompetition.CompetitionRole.RoleName,
                MemberId = memberInCompetition.MemberId,
                FullName = memberInCompetition.Member.User.Fullname,
                Status = memberInCompetition.Status
            };
        }

        private ViewCompetitionInClub TransferViewCompetitionInClub(CompetitionInClub competitionInClub)
        {
            return new ViewCompetitionInClub()
            {
                Id = competitionInClub.Id,
                ClubId = competitionInClub.ClubId,
                CompetitionId = competitionInClub.CompetitionId,
                IsOwner = competitionInClub.IsOwner,
            };
        }

        public async Task<ViewDetailCompetition> TransferViewDetailCompetition(Competition competition)
        {

            //List Clubs in Comeptition
            List<ViewClubInComp> viewClubsInCompetition = await _competitionInClubRepo.GetListClubInCompetition(competition.Id);

            //Add image club
            foreach (ViewClubInComp viewClub in viewClubsInCompetition)
            {
                viewClub.Image = await GetUrlImageClub(viewClub.Image, viewClub.ClubId);
            }

            //List Majors in Competition
            List<ViewMajorInComp> viewMajorsInCompetition = await _competitionInMajorRepo.GetListMajorInCompetition(competition.Id);

            //List Competition Entity
            //List<ViewCompetitionEntity> viewCompetitionEntities = new List<ViewCompetitionEntity>();

            List<ViewCompetitionEntity> competitionEntities = await _competitionEntityRepo.GetCompetitionEntities(competition.Id);

            if (competitionEntities != null)
            {
                foreach (ViewCompetitionEntity competitionEntity in competitionEntities)
                {
                    competitionEntity.ImageUrl = await GetUrlImageCompEntity(competitionEntity.ImageUrl, competitionEntity.Id);
                }
            }

            //Number Of Participant Join This Competition
            int NumberOfParticipantJoin = await _participantRepo.NumOfParticipant(competition.Id);

            //Competition type name
            CompetitionType competitionType = await _competitionTypeRepo.Get(competition.CompetitionTypeId);

            //University name
            University university = await _universityRepo.Get(competition.UniversityId);

            return new ViewDetailCompetition()
            {
                Id = competition.Id,
                UniversityId = competition.UniversityId,
                UniversityName = university.Name,
                UniversityImage = university.ImageUrl,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,
                CompetitionTypeName = competitionType.TypeName,
                Address = competition.Address,
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfTeam = (int)competition.NumberOfTeam,
                MinNumber = (int)competition.MinNumber,
                MaxNumber = (int)competition.MaxNumber,
                MinTeamOrParticipant = competition.RequiredMin,
                AddressName = competition.AddressName,
                CreateTime = competition.CreateTime,
                StartTime = competition.StartTime,
                EndTime = competition.EndTime,
                StartTimeRegister = competition.StartTimeRegister,
                EndTimeRegister = competition.EndTimeRegister,
                Content = competition.Content,
                Fee = competition.Fee,
                SeedsPoint = competition.SeedsPoint,
                SeedsDeposited = competition.SeedsDeposited,
                SeedsCode = competition.SeedsCode,
                IsSponsor = competition.IsSponsor,
                Scope = competition.Scope,
                Status = competition.Status,
                View = competition.View,
                //ClubInCompetition = (viewClubsInCompetition != null) ? viewClubsInCompetition : null,
                ClubInCompetition = viewClubsInCompetition,
                //CompetitionEntities = (competitionEntities != null) ? competitionEntities : null,
                CompetitionEntities = competitionEntities,
                //MajorsInCompetition = (viewMajorsInCompetition != null) ? viewMajorsInCompetition : null,
                MajorsInCompetition = viewMajorsInCompetition,
                NumberOfParticipantJoin = NumberOfParticipantJoin,
            };
        }

        private string GenerateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[10];
            string code = "";
            var random = new Random();

            for (int i = 0; i < chars.Length; i++)
            {
                code += string.Concat(codePool[random.Next(codePool.Length)]);
            }
            return code;
        }

        private async Task<string> CheckExistCode()
        {
            //auto generate seedCode
            bool check = true;
            string seedCode = "";
            while (check)
            {
                string generateCode = GenerateSeedCode();
                check = await _competitionRepo.CheckExistCode(generateCode);
                seedCode = generateCode;
            }
            return seedCode;
        }

        //Check Date Insert - Update
        private bool CheckDate(DateTime localTime, DateTime StartTimeRegister, DateTime EndTimeRegister, DateTime StartTime, DateTime EndTime, bool Update)
        {

            //condition
            bool round1 = false;
            bool round2 = false;
            bool round3 = false;
            bool round4 = false;
            bool round5 = false;
            bool result = false;


            //Use when API UPDATE
            //Can't update StartTime when Competition is happenning
            if (Update)
            {
                round1 = true;
            }
            else
            {
                //ROUND 1 
                //CHECK LOCALTIME < STR < ETR < ST < ET -> LocalTime
                //DateTime localTime = new LocalTime().GetLocalTime().DateTime;
                // resultLT1 < STR (sớm hơn)
                int resultLT1 = DateTime.Compare(localTime, StartTimeRegister);
                if (resultLT1 <= 0)
                {
                    //resultLT2 < ETR (sớm hơn)
                    int resultLT2 = DateTime.Compare(localTime, EndTimeRegister);
                    if (resultLT2 < 0)
                    {
                        //resultLT3 < ST (sớm hơn)
                        int resultLT3 = DateTime.Compare(localTime, StartTime);
                        if (resultLT3 < 0)
                        {
                            //resultLT4 < ET (sớm hơn)
                            int resultLT4 = DateTime.Compare(localTime, EndTime);
                            if (resultLT4 < 0)
                            {
                                round1 = true;
                            }
                        }
                    }
                }
            }

            //ROUND 2
            if (round1)
            {
                //STR == LocalTime 
                //STR < ETR < ST < ET -> STR true
                //kq 1 < 0 -> STR < ETR (sớm hơn)
                int kq1 = DateTime.Compare(StartTimeRegister, EndTimeRegister);
                if (kq1 < 0)
                {
                    //kq 2 < 0 -> STR < ST (sớm hơn)
                    int kq2 = DateTime.Compare(StartTimeRegister, StartTime);
                    if (kq2 < 0)
                    {
                        //kq 3 < 0 -> STR < ET (sớm hơn)
                        int kq3 = DateTime.Compare(StartTimeRegister, EndTime);
                        if (kq3 < 0)
                        {
                            round2 = true;
                        }
                    }//end kq2
                }//end kq1
            }

            //ROUND 3
            //ETR < ST < ET -> ETR true
            if (round1 && round2)
            {
                //kq 4 < 0 -> ETR < ST (sớm hơn)
                int kq4 = DateTime.Compare(EndTimeRegister, StartTime);
                if (kq4 < 0)
                {
                    //kq 5 < 0 -> ETR < ET (sớm hơn)
                    int kq5 = DateTime.Compare(EndTimeRegister, EndTime);
                    if (kq5 < 0)
                    {
                        round3 = true;
                    }
                }
            }

            //ROUND 4
            //ST  < ET - > ST true
            if (round1 && round2 && round3)
            {
                //kq 6 < 0 -> ST < ET (sớm hơn)
                int kq6 = DateTime.Compare(StartTime, EndTime);
                if (kq6 < 0)
                {
                    round4 = true;
                }
            }


            //ROUND 5 check Time must be sperate 1 hours
            if (round1 && round2 && round3 && round4)
            {
                //STR < ETR 1 hours or more
                TimeSpan cm1 = EndTimeRegister - StartTimeRegister;
                if ((TimeSpan.Compare(cm1, TimeSpan.FromHours(1)) >= 0))
                {
                    //ETR < ST 1 hours or more chỗ này chắc chắn là ceremony time hợp lệ vì nó sớm hơn ETR time 30p
                    TimeSpan cm2 = StartTime - EndTimeRegister;
                    if ((TimeSpan.Compare(cm2, TimeSpan.FromHours(1)) >= 0))
                    {
                        //ST < ET 1 hours or more
                        TimeSpan cm3 = EndTime - StartTime;
                        if ((TimeSpan.Compare(cm3, TimeSpan.FromHours(1)) >= 0))
                        {
                            round5 = true;
                        }
                    }
                }
            }

            //
            if (round1 && round2 && round3 && round4 && round5)
            {
                result = true;
            }

            return result;
        }

        //Check state Register
        private bool CheckStateRegister(DateTime localTime, DateTime StartTimeRegister, DateTime EndTimeRegister)
        {
            bool round1 = false;
            bool result = false;

            // Register < localTime < EndRegister
            int result1 = DateTime.Compare(localTime, StartTimeRegister); // > 0 later
            if (result1 > 0)
            {
                int result2 = DateTime.Compare(localTime, EndTimeRegister); // < 0 sooner
                if (result2 < 0)
                {
                   round1 = true;
                }
            }

            if (round1) {
                //1 hours or more
                TimeSpan cm1 = EndTimeRegister - StartTimeRegister;
                if ((TimeSpan.Compare(cm1, TimeSpan.FromHours(1)) >= 0))
                {
                    result = true;
                }
            }
                return result;
        }

        //Check state UpComing
        private bool CheckStateUpComing(DateTime localTime, DateTime EndTimeRegister, DateTime CeremonyTime)
        {
            bool round1 = false;
            bool result = false;

            // EndRegister < localTime < Ceremony time
            int result1 = DateTime.Compare(localTime, EndTimeRegister); // > 0 later
            if (result1 > 0)
            {
                int result2 = DateTime.Compare(localTime, CeremonyTime); // < 0 sooner
                if (result2 < 0)
                {
                    round1 = true;
                }
            }


            if (round1)
            {
                //30 minutes or more
                TimeSpan cm1 = CeremonyTime - EndTimeRegister;
                if ((TimeSpan.Compare(cm1, TimeSpan.FromMinutes(30)) >= 0))
                {
                    result = true;
                }
            }

            return result;
        }

        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            //Competition competition = await _competitionRepo.Get(CompetitionId);
            //if (competition == null) throw new ArgumentException("Competition or Event not found ");
            bool isExisted = await _competitionRepo.CheckExistedCompetition(CompetitionId);
            if (!isExisted) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            //Club club = await _clubRepo.Get(ClubId);
            //if (club == null) throw new ArgumentException("Club in not found");
            isExisted = await _clubRepo.CheckExistedClub(ClubId);
            if (!isExisted) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), ClubId);
            //Member member = await _memberRepo.Get(memberId);
            if (memberId.Equals(0)) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition competitionManager = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (competitionManager == null) throw new UnauthorizedAccessException("You do not in Competition Manager ");

            if (isOrganization && competitionManager.CompetitionRoleId >= 3) // accept competitionRoleId 1,2
                throw new UnauthorizedAccessException("Only role Manager can do this action");

            return true;
            //if (isOrganization)
            //{
            //    //1,2 accept
            //    if (competitionManager.CompetitionRoleId >= 3) throw new UnauthorizedAccessException("Only role Manager can do this action");
            //    return true;
            //}
            //else
            //{
            //    return true;
            //}
        }

        private bool CheckMaxMin(int max, int min, int NumberOfParticipant)
        {
            if (max < 0 || min < 0 || max < min)
            {
                throw new ArgumentException("0 < min < max ");
            }

            if (NumberOfParticipant <= 0)
            {
                throw new ArgumentException("Number Of Participant > 0");
            }
            return true;
        }

        //all have data
        private bool CheckDateInsertCases(Competition comp, DateTime localTime, LeaderUpdateCompOrEventModel model)
        {
            bool checkDate = false;
            if (model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && model.StartTime.HasValue && model.EndTime.HasValue)
            {
                bool STR = false;
                bool ETR = false;
                bool ST = false;
                bool ET = false;
                //STR Update = STR 
                if (DateTime.Compare(model.StartTimeRegister.Value, comp.StartTimeRegister) != 0) // data mới
                {
                    STR = true;
                }
                //ETR Update = ETR 
                if (DateTime.Compare(model.EndTimeRegister.Value, comp.EndTimeRegister) != 0) // data mới
                {
                    ETR = true;
                }
                //ST Update = ST
                if (DateTime.Compare(model.StartTime.Value, comp.StartTime) != 0) // data mới
                {
                    ST = true;
                }
                //ET Update = ET
                if (DateTime.Compare(model.EndTime.Value, comp.EndTime) != 0) // data mới
                {
                    ET = true;
                }

                // STR - ETR - ST - ET
                //  1  -  1  - 1  - 1   all true
                //  1  -  1  - 1  - 2
                //  1  -  1  - 2  - 1
                //  1  -  1  - 2  - 2
                //  1  -  2  - 1  - 1
                //  1  -  2  - 1  - 2
                //  1  -  2  - 2  - 1
                //  1  -  2  - 2  - 2
                //  2  -  1  - 1  - 1
                //  2  -  1  - 1  - 2
                //  2  -  1  - 2  - 1
                //  2  -  1  - 2  - 2  
                //  2  -  2  - 1  - 1
                //  2  -  2  - 1  - 2
                //  2  -  2  - 2  - 1
                //  2  -  2  - 2  - 2   all false

                //All true
                if (STR && ETR && ST && ET)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, true);
                }

                //  1  -  1  - 1  - 2
                if (STR && ETR && ST && ET == false)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, comp.EndTime, true);
                }

                //  1  -  1  - 2  - 1
                if (STR && ETR && ST == false && ET)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, comp.StartTime, model.EndTime.Value, true);
                }

                //  1  -  1  - 2  - 2
                if (STR && ETR && ST == false && ET == false)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, comp.StartTime, comp.EndTime, true);
                }

                //  1  -  2  - 1  - 1
                if (STR && ETR == false && ST && ET)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, model.StartTime.Value, model.EndTime.Value, true);
                }

                //  1  -  2  - 1  - 2
                if (STR && ETR == false && ST && ET == false)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, model.StartTime.Value, comp.EndTime, true);
                }

                //  1  -  2  - 2  - 1
                if (STR && ETR == false && ST && ET == false)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, true);
                }

                //  1  -  2  - 2  - 2
                if (STR && ETR == false && ST == false && ET == false)
                {
                    checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, comp.StartTime, comp.EndTime, true);
                }

                //  2  -  1  - 1  - 1
                if (STR == false && ETR && ST && ET)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, true);
                }

                //  2  -  1  - 1  - 2
                if (STR == false && ETR && ST && ET == false)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, model.StartTime.Value, comp.EndTime, true);
                }

                //  2  -  1  - 2  - 1
                if (STR == false && ETR && ST == false && ET)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, comp.StartTime, model.EndTime.Value, true);
                }

                //  2  -  1  - 2  - 2  
                if (STR == false && ETR && ST == false && ET == false)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, comp.StartTime, comp.EndTime, true);
                }

                //  2  -  2  - 1  - 1
                if (STR == false && ETR == false && ST && ET)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, model.StartTime.Value, model.EndTime.Value, true);
                }

                //  2  -  2  - 1  - 2
                if (STR == false && ETR == false && ST && ET == false)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, model.StartTime.Value, comp.EndTime, true);
                }

                //  2  -  2  - 2  - 1
                if (STR == false && ETR == false && ST == false && ET)
                {
                    checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, true);
                }

                //All false
                if (STR == false && ETR == false && ST == false && ET == false)
                {
                    checkDate = true;
                }

            }
            else
            {
                throw new ArgumentException("Missing Field Date");
            }

            return checkDate;
        }

        private bool CheckNumMinMaxCases(Competition comp, LeaderUpdateCompOrEventModel model)
        {
            //------------- CHECK Max,Min,NumberOfParticipant Update                          
            bool checkMaxMin = false;
            //------------- FE,MOBILE TEST
            //TH CHung
            if (model.MaxNumber.HasValue && model.MinNumber.HasValue && model.NumberOfParticipant.HasValue)
            {
                bool Max = false;
                bool Min = false;
                bool NumOfParticipant = false;

                //Max Update = Max
                if (model.MaxNumber.Value != comp.MaxNumber)                // data mới
                {
                    Max = true;
                }
                //Min Update = Min
                if (model.MinNumber.Value != comp.MinNumber)                // data mới
                {
                    Min = true;
                }
                //Number Of Participant Update = Number Of Participant
                if (model.NumberOfParticipant.Value != comp.NumberOfParticipation)  // data mới
                {
                    NumOfParticipant = true;
                }

                // Max  Min  Num
                // 1    1     1     //All True
                // 1    1     0
                // 1    0     1
                // 1    0     0     //Max Update                               
                // 0    1     1
                // 0    1     0     //Min Update
                // 0    0     1     //Number Of Participant Update 
                // 0    0     0     //All False


                // 1    1     1
                if (Max && Min && NumOfParticipant)
                {
                    checkMaxMin = CheckMaxMin(model.MaxNumber.Value, model.MinNumber.Value, model.NumberOfParticipant.Value);

                }

                // 1    1     0
                if (Max && Min && NumOfParticipant == false)
                {
                    checkMaxMin = CheckMaxMin(model.MaxNumber.Value, model.MinNumber.Value, comp.NumberOfParticipation);

                }

                // 1    0     1
                if (Max && Min == false && NumOfParticipant)
                {
                    checkMaxMin = CheckMaxMin(model.MaxNumber.Value, (int)comp.MinNumber, model.NumberOfParticipant.Value);

                }

                // 1    0     0
                if (Max && Min == false && NumOfParticipant == false)
                {
                    checkMaxMin = CheckMaxMin(model.MaxNumber.Value, comp.MinNumber.Value, comp.NumberOfParticipation);

                }

                // 0    1     1                              
                if (Max == false && Min && NumOfParticipant)
                {
                    checkMaxMin = CheckMaxMin((int)comp.MaxNumber, model.MinNumber.Value, model.NumberOfParticipant.Value);

                }

                // 0    1     0 
                if (Max == false && Min && NumOfParticipant == false)
                {
                    checkMaxMin = CheckMaxMin((int)comp.MaxNumber, model.MinNumber.Value, comp.NumberOfParticipation);

                }

                // 0    0     1
                if (Max == false && Min == false && NumOfParticipant)
                {
                    checkMaxMin = CheckMaxMin((int)comp.MaxNumber, (int)comp.MinNumber, model.NumberOfParticipant.Value);
                }

                // 0    0     0 
                if (Max == false && Min == false && NumOfParticipant == false)
                {
                    checkMaxMin = true;
                }
            }
            else
            {
                throw new ArgumentException("Missing Field Max or Min or Number Of Participant");
            }
            return checkMaxMin;
        }

        private async Task<Competition> UpdateFieldCompetition(Competition comp, LeaderUpdateCompOrEventModel model, string token)
        {

            //Check Competition Type 
            CompetitionType ct = await _competitionTypeRepo.Get(model.CompetitionTypeId.Value);
            if (ct == null) throw new ArgumentException("Competition Type Id not have in System");

            //------------ Check FK
            // MajorId
            bool insertMajor;
            if (model.ListMajorId.Count > 0)
            {
                //TH1: InterUniversity
                if (model.Scope == CompetitionScopeStatus.InterUniversity)
                {
                    bool Check = await _majorRepo.CheckMajor(model.ListMajorId);
                    if (Check)
                    {
                        insertMajor = true;
                    }
                    else
                    {
                        throw new ArgumentException("Major not have in System");
                    }
                }
                //TH2: University-Club
                else
                {
                    bool Check = await _majorRepo.CheckMajorBelongToUni(model.ListMajorId, _decodeToken.Decode(token, "UniversityId"));
                    if (Check)
                    {
                        insertMajor = true;
                    }
                    else
                    {
                        throw new ArgumentException("Major Id not have in University");
                    }
                }
            }
            else
            {
                await _competitionInMajorRepo.DeleteAllCompetitionInMajor(comp.Id);
                insertMajor = true;
            }

            //-----------------insert major
            if (insertMajor)
            {
                //run delete 

                await _competitionInMajorRepo.DeleteAllCompetitionInMajor(comp.Id);

                foreach (int majorId in model.ListMajorId)
                {
                    CompetitionInMajor comInMaj = new CompetitionInMajor()
                    {
                        MajorId = majorId,
                        CompetitionId = comp.Id
                    };
                    await _competitionInMajorRepo.Insert(comInMaj);
                }
            }
            comp.CompetitionTypeId = model.CompetitionTypeId.HasValue ? model.CompetitionTypeId.Value : comp.CompetitionTypeId;
            comp.SeedsPoint = (double)(model.SeedsPoint.HasValue ? model.SeedsPoint.Value : comp.SeedsPoint);
            comp.AddressName = (!string.IsNullOrEmpty(model.AddressName)) ? model.AddressName : comp.AddressName;
            comp.Address = (!string.IsNullOrEmpty(model.Address)) ? model.Address : comp.Address;
            comp.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : comp.Name;
            comp.StartTimeRegister = model.StartTimeRegister.HasValue ? model.StartTimeRegister.Value : comp.StartTimeRegister;
            comp.EndTimeRegister = model.EndTimeRegister.HasValue ? model.EndTimeRegister.Value : comp.EndTimeRegister;
            comp.CeremonyTime = model.StartTime.HasValue ? model.StartTime.Value.AddMinutes(-30) : comp.StartTime;
            comp.StartTime = model.StartTime.HasValue ? model.StartTime.Value : comp.StartTime;
            comp.EndTime = model.EndTime.HasValue ? model.EndTime.Value : comp.EndTime;
            comp.Content = (!string.IsNullOrEmpty(model.Content)) ? model.Content : comp.Content;
            comp.Fee = (double)(model.Fee.HasValue ? model.Fee.Value : comp.Fee);
            comp.MaxNumber = model.MaxNumber.HasValue ? model.MaxNumber.Value : comp.MaxNumber;
            comp.MinNumber = model.MinNumber.HasValue ? model.MinNumber.Value : comp.MinNumber;
            comp.NumberOfParticipation = model.NumberOfParticipant.HasValue ? model.NumberOfParticipant.Value : comp.NumberOfParticipation;
            comp.RequiredMin = model.MinTeamOrParticipant.HasValue ? model.MinTeamOrParticipant.Value : comp.RequiredMin;
            comp.Scope = model.Scope.HasValue ? model.Scope.Value : comp.Scope;
            return comp;
        }

        //private string CompetitionStatusToString(CompetitionStatus competitionStatus)
        //{
        //    if (competitionStatus == CompetitionStatus.Register)
        //    {
        //        return "Register";
        //    }
        //    if (competitionStatus == CompetitionStatus.Publish)
        //    {
        //        return "Publish";
        //    }
        //    return null;
        //}


        //Special for update State Approve
        //private bool CheckDateInsertCasesStateApprove(Competition comp, DateTime localTime, LeaderUpdateCompOrEventModel model)
        //{
        //    bool checkDate = false;
        //    if (model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && model.StartTime.HasValue && model.EndTime.HasValue)
        //    {
        //        bool STR = false;
        //        bool ETR = false;
        //        bool ST = false;
        //        bool ET = false;
        //        //STR Update = STR 
        //        if (DateTime.Compare(model.StartTimeRegister.Value, comp.StartTimeRegister) != 0) // data mới
        //        {
        //            STR = true;
        //        }
        //        //ETR Update = ETR 
        //        if (DateTime.Compare(model.EndTimeRegister.Value, comp.EndTimeRegister) != 0) // data mới
        //        {
        //            ETR = true;
        //        }
        //        //ST Update = ST
        //        if (DateTime.Compare(model.StartTime.Value, comp.StartTime) != 0) // data mới
        //        {
        //            ST = true;
        //        }
        //        //ET Update = ET
        //        if (DateTime.Compare(model.EndTime.Value, comp.EndTime) != 0) // data mới
        //        {
        //            ST = true;
        //        }

        //        // STR - ETR - ST - ET
        //        //  1  -  1  - 1  - 1   all true
        //        //  1  -  1  - 1  - 2
        //        //  1  -  1  - 2  - 1
        //        //  1  -  1  - 2  - 2
        //        //  1  -  2  - 1  - 1
        //        //  1  -  2  - 1  - 2
        //        //  1  -  2  - 2  - 1
        //        //  1  -  2  - 2  - 2
        //        //  2  -  1  - 1  - 1
        //        //  2  -  1  - 1  - 2
        //        //  2  -  1  - 2  - 1
        //        //  2  -  1  - 2  - 2  
        //        //  2  -  2  - 1  - 1
        //        //  2  -  2  - 1  - 2
        //        //  2  -  2  - 2  - 1
        //        //  2  -  2  - 2  - 2   all false

        //        //All true
        //        if (STR && ETR && ST && ET)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);
        //        }

        //        //  1  -  1  - 1  - 2
        //        if (STR && ETR && ST && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, comp.EndTime, false);
        //        }

        //        //  1  -  1  - 2  - 1
        //        if (STR && ETR && ST == false && ET)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, comp.StartTime, model.EndTime.Value, false);
        //        }

        //        //  1  -  1  - 2  - 2
        //        if (STR && ETR && ST == false && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, comp.StartTime, comp.EndTime, false);
        //        }

        //        //  1  -  2  - 1  - 1
        //        if (STR && ETR == false && ST && ET)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, model.StartTime.Value, model.EndTime.Value, false);
        //        }

        //        //  1  -  2  - 1  - 2
        //        if (STR && ETR == false && ST && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, model.StartTime.Value, comp.EndTime, false);
        //        }

        //        //  1  -  2  - 2  - 1
        //        if (STR && ETR == false && ST && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, false);
        //        }

        //        //  1  -  2  - 2  - 2
        //        if (STR && ETR == false && ST == false && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, comp.EndTimeRegister, comp.StartTime, comp.EndTime, false);
        //        }

        //        //  2  -  1  - 1  - 1
        //        if (STR == false && ETR && ST && ET)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);
        //        }

        //        //  2  -  1  - 1  - 2
        //        if (STR == false && ETR && ST && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, model.StartTime.Value, comp.EndTime, false);
        //        }

        //        //  2  -  1  - 2  - 1
        //        if (STR == false && ETR && ST == false && ET)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, comp.StartTime, model.EndTime.Value, false);
        //        }

        //        //  2  -  1  - 2  - 2  
        //        if (STR == false && ETR && ST == false && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, model.EndTimeRegister.Value, comp.StartTime, comp.EndTime, false);
        //        }

        //        //  2  -  2  - 1  - 1
        //        if (STR == false && ETR == false && ST && ET)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, model.StartTime.Value, model.EndTime.Value, false);
        //        }

        //        //  2  -  2  - 1  - 2
        //        if (STR == false && ETR == false && ST && ET == false)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, model.StartTime.Value, comp.EndTime, false);
        //        }

        //        //  2  -  2  - 2  - 1
        //        if (STR == false && ETR == false && ST == false && ET)
        //        {
        //            checkDate = CheckDate(localTime, comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, model.EndTime.Value, false);
        //        }

        //        //All false
        //        if (STR == false && ETR == false && ST == false && ET == false)
        //        {
        //            checkDate = true;
        //        }

        //    }
        //    else
        //    {
        //        throw new ArgumentException("Missing Field Date");
        //    }

        //    return checkDate;
        //}


    }
}
