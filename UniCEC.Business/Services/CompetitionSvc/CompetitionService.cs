using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
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
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionHistory;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
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
        private IDepartmentRepo _departmentRepo;
        private ITeamRepo _teamRepo;
        private IUserRepo _userRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private IMajorRepo _majorRepo;
        private ICompetitionInMajorRepo _competitionInMajorRepo;
        private ICompetitionHistoryStatusRepo _competitionHistoryRepo;
        private DecodeToken _decodeToken;
        private readonly IConfiguration _configuration;

        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IMemberRepo memberRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  IClubRepo clubRepo,
                                  IParticipantRepo participantRepo,
                                  ICompetitionTypeRepo competitionTypeRepo,
                                  ICompetitionEntityRepo competitionEntityRepo,
                                  IMemberInCompetitionRepo memberInCompetitionRepo,
                                  IConfiguration configuration,
                                  IDepartmentRepo departmentRepo,
                                  ITeamRepo teamRepo,
                                  IUserRepo userRepo,
                                  IMajorRepo majorRepo,
                                  ICompetitionInMajorRepo competitionInMajorRepo,
                                  ICompetitionHistoryStatusRepo competitionHistoryRepo,
                                  ICompetitionRoleRepo competitionRoleRepo,
                                  IFileService fileService)
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
            _departmentRepo = departmentRepo;
            _userRepo = userRepo;
            _teamRepo = teamRepo;
            _majorRepo = majorRepo;
            _decodeToken = new DecodeToken();
            _competitionRoleRepo = competitionRoleRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionInMajorRepo = competitionInMajorRepo;
            _competitionHistoryRepo = competitionHistoryRepo;
        }


        //Get
        public async Task<ViewDetailCompetition> GetById(int id)
        {
            //
            Competition comp = await _competitionRepo.Get(id);
            //
            if (comp != null)
            {
                comp.View += 1;
                await _competitionRepo.Update();

                return await TransferViewDetailCompetition(comp);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //Get top 3 EVENT or COMPETITION by Status
        public async Task<List<ViewCompetition>> GetTop3CompOrEve(int? ClubId, bool? Event, CompetitionStatus? Status, CompetitionScopeStatus? Scope)
        {
            List<ViewCompetition> result = await _competitionRepo.GetTop3CompOrEve(ClubId, Event, Status, Scope);


            foreach (ViewCompetition item in result)
            {

                //List Competition Entity
                List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

                List<CompetitionEntity> CompetitionEntities = await _competitionEntityRepo.GetListCompetitionEntity(item.CompetitionId);

                if (CompetitionEntities != null)
                {
                    foreach (CompetitionEntity competitionEntity in CompetitionEntities)
                    {
                        //get IMG from Firebase                        
                        string imgUrl_CompetitionEntity;
                        try
                        {
                            imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
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
                            ImageUrl = imgUrl_CompetitionEntity,
                        };
                        //
                        ListView_CompetitionEntities.Add(viewCompetitionEntity);
                    }
                }

                item.CompetitionEntities = ListView_CompetitionEntities;
            }
            if (result == null) throw new NullReferenceException();
            return result;
        }

        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEve(request);

            List<ViewCompetition> resultList = result.Items.ToList();

            foreach (ViewCompetition item in resultList)
            {

                //List Competition Entity
                List<ViewCompetitionEntity> ListView_CompetitionEntities = new List<ViewCompetitionEntity>();

                List<CompetitionEntity> CompetitionEntities = await _competitionEntityRepo.GetListCompetitionEntity(item.CompetitionId);

                if (CompetitionEntities != null)
                {
                    foreach (CompetitionEntity competitionEntity in CompetitionEntities)
                    {
                        //get IMG from Firebase                        
                        string imgUrl_CompetitionEntity;
                        try
                        {
                            imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
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
                            ImageUrl = imgUrl_CompetitionEntity,
                        };
                        //
                        ListView_CompetitionEntities.Add(viewCompetitionEntity);
                    }
                }

                item.CompetitionEntities = ListView_CompetitionEntities;
            }

            if (result == null) throw new NullReferenceException();
            return result;

        }


        //----ROLE LEADER OF CLUB
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
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint < 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null" +
                                                    " ||StartTimeRegister Null ||EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint Null  || ClubId Null");

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
                           || string.IsNullOrEmpty(sponsor.Email)
                           || string.IsNullOrEmpty(sponsor.Description)
                           || string.IsNullOrEmpty(sponsor.Website))
                            throw new ArgumentNullException("Image of Sponsor is NULL || Name is NULL || Email is NULL || Description is NULL|| Website is NULL");
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
                competition.CreateTime = localTime;
                competition.StartTimeRegister = localTime;
                competition.EndTimeRegister = model.EndTimeRegister;
                competition.CeremonyTime = model.StartTime.AddMinutes(30); // auto add 30 minutes để state là Start
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
                CompetitionHistoryStatusInsertModel chim = new CompetitionHistoryStatusInsertModel()
                {
                    CompetitionId = comp.Id,
                    ChangerId = member.Id,
                    ChangeDate = localTime,
                    Description = "Create Competition", // auto des when create Competition
                    Status = CompetitionStatus.Draft,   // when create status draft
                };
                InsertCompetitionHistoryStatus(chim);


                ViewDetailCompetition viewDetailCompetition = await TransferViewDetailCompetition(comp);
                return viewDetailCompetition;

            }
            catch (Exception)
            {
                throw;
            }
        }

        //----ROLE IN COMPETITION MANAGER
        public async Task<bool> DeleteCompetitionOrEvent(LeaderDeleteCompOrEventModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException(" Competition Id Null || ClubId Null");

                bool Check = await CheckMemberInCompetition(token, model.CompetitionId, model.ClubId, true);
                if (Check)
                {

                    Competition comp = await _competitionRepo.Get(model.CompetitionId);

                    //Ở trạng thái này thì có thể xóa
                    if (comp.Status != CompetitionStatus.Draft
                     || comp.Status != CompetitionStatus.Approve
                     || comp.Status != CompetitionStatus.Publish
                     || comp.Status != CompetitionStatus.Register
                     || comp.Status != CompetitionStatus.UpComing)
                        throw new ArgumentException("Competition State is not suitable to do this action");

                    comp.Status = CompetitionStatus.Cancel;
                    await _competitionRepo.Update();


                    //----------- InsertCompetition History
                    CompetitionHistoryStatusInsertModel chim = new CompetitionHistoryStatusInsertModel()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId),
                        ChangeDate = new LocalTime().GetLocalTime().DateTime,
                        Description = "Delete Competition", // auto des when Delete Competition
                        Status = CompetitionStatus.Cancel,   // when create status draft
                    };
                    InsertCompetitionHistoryStatus(chim);

                    return true;

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


        //---------------------------------------------------------------------------------- STATE DRAFT - APPROVE
        public async Task<bool> UpdateCompetitionOrEvent(LeaderUpdateCompOrEventModel model, string token)
        {
            try
            {
                if (model.Id == 0
                   || model.ClubId == 0)
                    throw new ArgumentNullException("Competition Id Null  || ClubId Null");
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;



                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);
                if (Check)
                {

                    //Chỉ Cho những Trạng Thái này update những trạng thái trước khi publish
                    Competition comp = await _competitionRepo.Get(model.Id);
                    if (comp.Status != CompetitionStatus.Draft) throw new ArgumentException("Competition State is not suitable to do this action");


                    bool checkDate = false;
                    //------------- CHECK Date Update  
                    //------------- FE,MOBILE TEST                       
                    //TH CHung
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
                            ST = true;
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
                            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);
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
                    if (checkDate) throw new ArgumentException("Date not suitable");

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
                    if (checkMaxMin == false) throw new ArgumentException("Max Number or Min Number or Number Of Participant is not suitable");

                    comp.SeedsPoint = (double)((model.SeedsPoint.HasValue) ? model.SeedsPoint : comp.SeedsPoint);
                    comp.SeedsDeposited = (double)((model.SeedsDeposited.HasValue) ? model.SeedsDeposited : comp.SeedsDeposited);
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
                    comp.Scope = model.Scope; //auto

                    await _competitionRepo.Update();
                    return true;
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
                    if (comp.Status != CompetitionStatus.Draft || comp.Status != CompetitionStatus.Approve)
                        throw new ArgumentException("Competition State is not suitable to do this action");

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


        //---------------------------------------------------------------------------------- STATE PENDING REVIEW


        //---------------------------------------------------------------------------------- STATE APPROVE
        public async Task<bool> UpdateConstraintBeforePublish(UpdateConstraintBeforePublishModel model, string token)
        {
            try
            {
                if (model.ClubId == 0 || model.Id == 0 || string.IsNullOrEmpty(model.Content) || !model.Scope.HasValue || string.IsNullOrEmpty(model.Comment))
                    throw new ArgumentException("Club Id NULL || Competition Id NULL || Content NULL || Scope NULL || Comment NULL");

                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);

                if (Check == false) return false;

                Competition comp = await _competitionRepo.Get(model.Id);
                if (comp.Status != CompetitionStatus.Approve) throw new ArgumentException("Competition State is not suitable to do this action");


                comp.Content = (!string.IsNullOrEmpty(model.Content)) ? model.Content : comp.Content;
                comp.Scope = (model.Scope.HasValue) ? model.Scope.Value : comp.Scope;
                await _competitionRepo.Update();

                //Insert Competition History Status
                //có 2 trạng thái để chuyển State khi update
                //nếu là Draft thì vẫn như cũ
                //nếu đã là Approve thì chuyển về pendding review
                if (comp.Status == CompetitionStatus.Approve)
                {
                    comp.Status = CompetitionStatus.PendingReview;

                    //----------- InsertCompetition History
                    CompetitionHistoryStatusInsertModel chim = new CompetitionHistoryStatusInsertModel()
                    {
                        CompetitionId = comp.Id,
                        ChangerId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId),
                        ChangeDate = new LocalTime().GetLocalTime().DateTime,
                        Description = model.Comment, // auto des when Delete Competition
                        Status = CompetitionStatus.PendingReview,   // when create status draft
                    };
                    InsertCompetitionHistoryStatus(chim);
                }

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBeforePublish(UpdateBeforePublishModel model, string token)
        {
            try
            {
                if (model.Id == 0 || model.ClubId == 0) throw new ArgumentNullException("Competition Id Null  || ClubId Null");
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);
                if (Check)
                {

                    //Chỉ Cho những Trạng Thái này update những trạng thái trước khi publish
                    Competition comp = await _competitionRepo.Get(model.Id);
                    if (comp.Status != CompetitionStatus.Approve) throw new ArgumentException("Competition State is not suitable to do this action");


                    bool checkDate = false;
                    //------------- CHECK Date Update  
                    //------------- FE,MOBILE TEST                       
                    //TH CHung
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
                            ST = true;
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
                            checkDate = CheckDate(localTime, model.StartTimeRegister.Value, model.EndTimeRegister.Value, model.StartTime.Value, model.EndTime.Value, false);
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
                    if (checkDate) throw new ArgumentException("Date not suitable");

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
                    if (checkMaxMin == false) throw new ArgumentException("Max Number or Min Number or Number Of Participant is not suitable");

                    comp.SeedsPoint = (double)((model.SeedsPoint.HasValue) ? model.SeedsPoint : comp.SeedsPoint);
                    comp.SeedsDeposited = (double)((model.SeedsDeposited.HasValue) ? model.SeedsDeposited : comp.SeedsDeposited);
                    comp.AddressName = (!string.IsNullOrEmpty(model.AddressName)) ? model.AddressName : comp.AddressName;
                    comp.Address = (!string.IsNullOrEmpty(model.Address)) ? model.Address : comp.Address;
                    comp.Name = (!string.IsNullOrEmpty(model.Name)) ? model.Name : comp.Name;
                    comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                    comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                    comp.CeremonyTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime.Value.AddMinutes(30) : comp.StartTime);
                    comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                    comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                    comp.Fee = (double)((model.Fee.HasValue) ? model.Fee : comp.Fee);
                    comp.MaxNumber = (model.MaxNumber.HasValue) ? model.MaxNumber : comp.MaxNumber;
                    comp.MinNumber = (model.MinNumber.HasValue) ? model.MinNumber : comp.MinNumber;
                    comp.NumberOfParticipation = (int)((model.NumberOfParticipant.HasValue) ? model.NumberOfParticipant : comp.NumberOfParticipation);
                    await _competitionRepo.Update();
                    return true;
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

        //---------------------------------------------------------------------------------- STATE PUBLISH - STATE REGISTER - UPCOMMING
        public async Task<bool> UpdateBeforeCeremonyTime(UpdateBeforeCeremonyModel model, string token)
        {
            try
            {
                if (model.Id == 0 || model.ClubId == 0) throw new ArgumentNullException("Competition Id Null  || ClubId Null");
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;

                bool Check = await CheckMemberInCompetition(token, model.Id, model.ClubId, true);

                return Check;


            }
            catch (Exception)
            {
                throw;
            }
        }

        //---------------------------------------------------------------------------------- STATE AFTER END
        //---------------------------------------------------------------------------------- STATE AFTER END
        //---------------------------------------------------------------------------------- STATE AFTER END
        //---------------------------------------------------------------------------------- STATE AFTER END
        //---------------------------------------------------------------------------------- STATE AFTER END
        //---------------------------------------------------------------------------------- STATE AFTER END

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
                            comp.SeedsDeposited = (double)((model.SeedsDeposited.HasValue) ? model.SeedsDeposited : comp.SeedsDeposited);
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


                            //có 2 trạng thái để chuyển State khi update
                            //nếu là Draft thì vẫn như cũ
                            //nếu đã là Approve thì chuyển về pendding review
                            //if (comp.Status == CompetitionStatus.Approve)
                            //{
                            //    comp.Status = CompetitionStatus.PendingReview;

                            //    //----------- InsertCompetition History
                            //    CompetitionHistoryStatusInsertModel chim = new CompetitionHistoryStatusInsertModel()
                            //    {
                            //        CompetitionId = comp.Id,
                            //        ChangerId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token, "Id"), model.ClubId),
                            //        ChangeDate = new LocalTime().GetLocalTime().DateTime,
                            //        Description = "Delete Competition", // auto des when Delete Competition
                            //        Status = CompetitionStatus.PendingReview,   // when create status draft
                            //    };
                            //    InsertCompetitionHistoryStatus(chim);
                            //}

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

            //List Majors in Competition
            List<ViewMajorInComp> viewMajorsInCompetition = await _competitionInMajorRepo.GetListMajorInCompetition(competition.Id);

            //List Competition Entity
            List<ViewCompetitionEntity> viewCompetitionEntities = new List<ViewCompetitionEntity>();

            List<CompetitionEntity> CompetitionEntities = await _competitionEntityRepo.GetListCompetitionEntity(competition.Id);

            if (CompetitionEntities != null)
            {
                foreach (CompetitionEntity competitionEntity in CompetitionEntities)
                {
                    //get IMG from Firebase                        
                    string imgUrl_CompetitionEntity;
                    try
                    {
                        imgUrl_CompetitionEntity = await _fileService.GetUrlFromFilenameAsync(competitionEntity.ImageUrl);
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
                        ImageUrl = imgUrl_CompetitionEntity,
                        EntityTypeId = competitionEntity.EntityTypeId,
                        EntityTypeName = competitionEntity.EntityType.Name,
                        Email = (competitionEntity.Email != null) ? competitionEntity.Email : null,
                        Website = (competitionEntity.Website != null) ? competitionEntity.Website : null,
                        Description = (competitionEntity.Description != null) ? competitionEntity.Description : null,
                    };
                    //
                    viewCompetitionEntities.Add(viewCompetitionEntity);
                }
            }

            //Number Of Participant Join This Competition
            int NumberOfParticipantJoin = await _participantRepo.NumOfParticipant(competition.Id);

            //Competition type name
            CompetitionType competitionType = await _competitionTypeRepo.Get(competition.CompetitionTypeId);
            string competitionTypeName = competitionType.TypeName;

            return new ViewDetailCompetition()
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,
                CompetitionTypeName = competitionTypeName,
                Address = competition.Address,
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfTeam = (int)competition.NumberOfTeam,
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
                ClubInCompetition = (viewClubsInCompetition != null) ? viewClubsInCompetition : null,
                CompetitionEntities = (viewCompetitionEntities != null) ? viewCompetitionEntities : null,
                MajorsInCompetition = (viewMajorsInCompetition != null) ? viewMajorsInCompetition : null,
                NumberOfParticipantJoin = NumberOfParticipantJoin,
            };
        }


        private async void InsertCompetitionHistoryStatus(CompetitionHistoryStatusInsertModel model)
        {
            CompetitionHistoryStatus competitionHistory = new CompetitionHistoryStatus()
            {
                CompetitionId = model.CompetitionId,
                ChangerId = (model.ChangerId.HasValue) ? model.ChangerId.Value : null,
                ChangeDate = model.ChangeDate,
                Description = (string.IsNullOrEmpty(model.Description)) ? null : model.Description,
                Status = model.Status,
            };

            int result = await _competitionHistoryRepo.Insert(competitionHistory);
            if (result == 0) throw new ArgumentException("Add Competition History Failed");
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
            //
            if (round1 && round2 && round3 && round4)
            {
                result = true;
            }

            return result;
        }

        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition == null) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            Club club = await _clubRepo.Get(ClubId);
            if (club == null) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), club.Id);
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition isAllow = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (isAllow == null) throw new UnauthorizedAccessException("You do not in Competition Manager ");

            if (isOrganization)
            {
                //------------- CHECK Role Is highest role
                if (isAllow.CompetitionRoleId != 1 || isAllow.CompetitionRoleId != 2) throw new UnauthorizedAccessException("Only role Manager can do this action");
                return true;
            }
            else
            {
                return true;
            }
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


    }
}
