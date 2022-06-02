﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.InfluencerInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.InfluencerRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionInDepartment;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;
using UniCEC.Data.ViewModels.Entities.Influencer;
using UniCEC.Data.ViewModels.Entities.InfluencerInComeptition;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public class CompetitionService : ICompetitionService
    {
        private ICompetitionRepo _competitionRepo;
        private IMemberRepo _memberRepo;
        private ICompetitionInClubRepo _competitionInClubRepo;
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;
        private ICompetitionInDepartmentRepo _competitionInDepartmentRepo;
        private IDepartmentInUniversityRepo _departmentInUniversityRepo;
        private IClubRepo _clubRepo;
        private IParticipantRepo _participantRepo;
        private ICompetitionTypeRepo _competitionTypeRepo;
        private IFileService _fileService;
        private ICompetitionEntityRepo _competitionEntityRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;
        private IInfluencerRepo _influencerRepo;
        private IInfluencerInCompetitionRepo _influencerInCompetitionRepo;
        private ITermRepo _termRepo;
        private JwtSecurityTokenHandler _tokenHandler;
        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IMemberRepo memberRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  ISponsorInCompetitionRepo sponsorInCompetitionRepo,
                                  ICompetitionInDepartmentRepo competitionInDepartmentRepo,
                                  IDepartmentInUniversityRepo departmentInUniversityRepo,
                                  IClubRepo clubRepo,
                                  IParticipantRepo participantRepo,
                                  ICompetitionTypeRepo competitionTypeRepo,
                                  ICompetitionEntityRepo competitionEntityRepo,
                                  ICompetitionManagerRepo competitionManagerRepo,
                                  IInfluencerRepo influencerRepo,
                                  IInfluencerInCompetitionRepo influencerInCompetitionRepo,
                                  ITermRepo termRepo,
                                  IFileService fileService)
        {
            _competitionRepo = competitionRepo;
            _memberRepo = memberRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _competitionInDepartmentRepo = competitionInDepartmentRepo;
            _departmentInUniversityRepo = departmentInUniversityRepo;
            _clubRepo = clubRepo;
            _participantRepo = participantRepo;
            _competitionTypeRepo = competitionTypeRepo;
            _fileService = fileService;
            _competitionEntityRepo = competitionEntityRepo;
            _competitionManagerRepo = competitionManagerRepo;
            _influencerInCompetitionRepo = influencerInCompetitionRepo;
            _termRepo = termRepo;
            _influencerRepo = influencerRepo;
        }

        public CompetitionService()
        {
        }

        public Task<PagingResult<ViewCompetition>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewDetailCompetition> GetById(int id)
        {
            //
            Competition comp = await _competitionRepo.Get(id);

            //
            if (comp != null)
            {
                comp.View += 1;
                await _competitionRepo.Update();

                return await TransformViewDetailCompetition(comp);
            }
            else
            {
                return null;
            }
        }

        //Get top 3 EVENT or COMPETITION by Status
        public async Task<List<ViewCompetition>> GetTop3CompOrEve(int? ClubId, bool? Event, CompetitionStatus? Status, bool? Public)
        {
            List<ViewCompetition> result = await _competitionRepo.GetTop3CompOrEve(ClubId, Event, Status, Public);
            if (result == null) throw new NullReferenceException();
            return result;
        }

        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEve(request);
            if (result == null) throw new NullReferenceException();
            return result;
        }

        public async Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel model, string token, IFormFile file)
        {
            try
            {
                int UserId = DecodeToken(token, "Id");
                int UniversityId = DecodeToken(token, "UniversityId");

                bool roleLeader = false;

                if (string.IsNullOrEmpty(model.Name)
                    || string.IsNullOrEmpty(model.Content)
                    || string.IsNullOrEmpty(model.Address)
                    || string.IsNullOrEmpty(model.AddressName)
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.NumberOfTeam < 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint == 0
                    || model.SeedsDeposited == 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null || NumberOfTeam Null || StartTimeRegister Null " +
                                                    " EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint Null || SeedsDeposited Null || ClubId Null");
                //------------- CHECK Club in system
                Club club = await _clubRepo.Get(model.ClubId);
                if (club != null)
                {
                    ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(model.ClubId);

                    GetMemberInClubModel conditions = new GetMemberInClubModel()
                    {
                        UserId = UserId,
                        ClubId = model.ClubId,
                        TermId = CurrentTermOfCLub.Id
                    };
                    ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions); // GetMemberInClub => GetBasicInfoMember - I have implemented this func again
                    //------------ Check Mem in that club
                    if (infoClubMem != null)
                    {
                        //------------ Check Role Member Is Leader Of Club
                        if (infoClubMem.ClubRoleName.Equals("Leader"))
                        {
                            roleLeader = true;
                        }
                        if (roleLeader)
                        {
                            //------------ Check Date
                            bool checkDate = CheckDate(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime, false);
                            if (checkDate)
                            {
                                if (CheckNumber_Team(model.NumberOfTeam, model.NumberOfParticipations))
                                {
                                    //------------ Check FK
                                    // Department ID
                                    bool insertDepartment;
                                    if (model.ListDepartmentId.Count > 0)
                                    {

                                        bool check = await _departmentInUniversityRepo.CheckDepartmentBelongToUni(model.ListDepartmentId, UniversityId);
                                        if (check)
                                        {
                                            insertDepartment = true;
                                        }
                                        else
                                        {
                                            throw new ArgumentException("Department Id not have in University");
                                        }
                                    }
                                    else
                                    {
                                        insertDepartment = false;
                                    }

                                    //List Influencer ID
                                    bool insertInfluencer;
                                    if (model.ListInfluencerId.Count > 0)
                                    {

                                        bool check = await _influencerRepo.CheckInfluencerInSystem(model.ListInfluencerId);
                                        if (check)
                                        {
                                            insertInfluencer = true;
                                        }
                                        else
                                        {
                                            throw new ArgumentException("Influencer Id not have in University");
                                        }
                                    }
                                    else
                                    {
                                        insertInfluencer = false;
                                    }

                                    //------------ Insert Competition
                                    //ở trong trường hợp này phân biệt EVENT - COMPETITION
                                    //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                                    Competition competition = new Competition();
                                    competition.CompetitionTypeId = model.CompetitionTypeId;
                                    competition.AddressName = model.AddressName;
                                    competition.Address = model.Address;
                                    competition.Name = model.Name;
                                    competition.NumberOfTeam = model.NumberOfTeam;
                                    competition.NumberOfParticipation = model.NumberOfParticipations;
                                    competition.CreateTime = new LocalTime().GetLocalTime().DateTime;
                                    competition.StartTime = model.StartTime;
                                    competition.EndTime = model.EndTime;
                                    competition.StartTimeRegister = model.StartTimeRegister;
                                    competition.EndTimeRegister = model.EndTimeRegister;
                                    competition.Content = model.Content;
                                    competition.Fee = model.Fee;
                                    competition.SeedsPoint = model.SeedsPoint;
                                    competition.SeedsDeposited = model.SeedsDeposited;
                                    competition.SeedsCode = await CheckExistCode();
                                    competition.IsSponsor = false;
                                    competition.Status = CompetitionStatus.Launching;
                                    competition.Public = model.Public;
                                    competition.View = 0; // auto
                                    int competition_Id = await _competitionRepo.Insert(competition);
                                    if (competition_Id > 0)
                                    {
                                        Competition comp = await _competitionRepo.Get(competition_Id);

                                        //------------ Insert Competition-Entity                                 
                                        if (file.Length > 0)
                                        {
                                            bool insertEntity = false;
                                            string imgUrl = await _fileService.UploadFile(file);
                                            if (imgUrl != null)
                                            {
                                                insertEntity = true;
                                            }
                                            if (insertEntity)
                                            {
                                                CompetitionEntity competitionEntity = new CompetitionEntity()
                                                {
                                                    CompetitionId = comp.Id,
                                                    Name = model.Name,
                                                    ImageUrl = imgUrl
                                                };

                                                await _competitionEntityRepo.Insert(competitionEntity);
                                            }
                                        }

                                        //------------ Insert Competition-In-Department  
                                        if (insertDepartment)
                                        {
                                            foreach (int dep_id in model.ListDepartmentId)
                                            {
                                                CompetitionInDepartment comp_in_dep = new CompetitionInDepartment()
                                                {
                                                    DepartmentId = dep_id,
                                                    CompetitionId = comp.Id
                                                };
                                                int id = await _competitionInDepartmentRepo.Insert(comp_in_dep);
                                            }
                                        }

                                        //------------ Insert Influencer-In-Competition
                                        if (insertInfluencer)
                                        {
                                            foreach (int influ_id in model.ListInfluencerId)
                                            {
                                                InfluencerInCompetition influ_in_comp = new InfluencerInCompetition()
                                                {
                                                    CompetitionId = comp.Id,
                                                    InfluencerId = influ_id,
                                                };
                                                int id = await _influencerInCompetitionRepo.Insert(influ_in_comp);

                                            }
                                        }
                                        //------------ Insert Competition-In-Club
                                        CompetitionInClub competitionInClub = new CompetitionInClub();
                                        competitionInClub.ClubId = model.ClubId;
                                        competitionInClub.CompetitionId = comp.Id;
                                        int compInClub_Id = await _competitionInClubRepo.Insert(competitionInClub);


                                        //------------ Insert Competition-Manager
                                        CompetitionManager cm = new CompetitionManager()
                                        {
                                            CompetitionInClubId = compInClub_Id,
                                            //auto role 1 Manager
                                            CompetitionRoleId = 1,
                                            MemberId = infoClubMem.Id,
                                            Fullname = infoClubMem.Name
                                        };

                                        int cmId = await _competitionManagerRepo.Insert(cm);

                                        if (cmId > 0)
                                        {
                                            ViewDetailCompetition viewDetailCompetition = await TransformViewDetailCompetition(comp);
                                            return viewDetailCompetition;

                                        }//end compInClub_Id > 0
                                        else
                                        {
                                            throw new ArgumentException("Add Competition Or Event Failed");
                                        }
                                    }//end if competition_Id > 0
                                    else
                                    {
                                        throw new ArgumentException("Add Competition Or Event Failed");
                                    }
                                }//end if check Number Team
                                else
                                {
                                    throw new ArgumentException("Number of Team and Number of Member not suitable");
                                }
                            }//end if check date
                            else
                            {
                                throw new ArgumentException("Date not suitable");
                            }
                        }//end if role leader
                        else
                        {
                            throw new UnauthorizedAccessException("You do not a role Leader to insert this Competititon");
                        }
                    }//end check member
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't member in Club");
                    }
                }
                else
                {
                    throw new ArgumentException("Club in not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token, IFormFile file)
        {
            try
            {
                if (model.CompetitionId == 0
                  || model.ClubId == 0
                  || string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException("|| Competition Id Null  || Name Null" +
                                                     " ClubId Null");

                bool Check = await CheckConditions(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //------------ Insert Competition-Entities-----------
                    string Url = await _fileService.UploadFile(file);
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
                        return new ViewCompetitionEntity()
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            CompetitionId = entity.CompetitionId,
                            ImageUrl = entity.ImageUrl,
                        };
                    }
                    return null;
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

        public async Task<List<ViewCompetitionInDepartment>> AddCompetitionInDepartment(CompetitionInDepartmentInsertModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                  || model.ClubId == 0
                  || model.ListDepartmentId.Count < 0)
                    throw new ArgumentNullException("|| Competition Id Null  || Department Null" +
                                                     " ClubId Null");

                bool Check = await CheckConditions(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //add extra parameter
                    int UniversityId = DecodeToken(token, "UniversityId");
                    //------------- CHECK Department belong to University
                    bool departmentBelongToUni = await _departmentInUniversityRepo.CheckDepartmentBelongToUni(model.ListDepartmentId, UniversityId);

                    if (departmentBelongToUni)
                    {
                        //------------- CHECK Add Department is existed
                        bool DepartmentIsExsited = true;
                        foreach (int dep_id in model.ListDepartmentId)
                        {
                            //
                            CompetitionInDepartment cid = await _competitionInDepartmentRepo.GetDepartment_In_Competition(dep_id, model.CompetitionId);
                            if (cid != null)
                            {
                                DepartmentIsExsited = false;
                            }
                        }
                        if (DepartmentIsExsited)
                        {
                            List<int> list_dic_Id = new List<int>();
                            List<ViewCompetitionInDepartment> list_result = new List<ViewCompetitionInDepartment>();

                            foreach (int dep_id in model.ListDepartmentId)
                            {
                                CompetitionInDepartment comp_in_dep = new CompetitionInDepartment()
                                {
                                    DepartmentId = dep_id,
                                    CompetitionId = model.CompetitionId
                                };
                                int id = await _competitionInDepartmentRepo.Insert(comp_in_dep);
                                list_dic_Id.Add(id);
                            }
                            if (list_dic_Id.Count > 0)
                            {
                                foreach (int id in list_dic_Id)
                                {
                                    CompetitionInDepartment cid = await _competitionInDepartmentRepo.Get(id);

                                    ViewCompetitionInDepartment vcid = new ViewCompetitionInDepartment()
                                    {
                                        Id = cid.Id,
                                        CompetitionId = cid.CompetitionId,
                                        DepartmentId = cid.DepartmentId
                                    };
                                    list_result.Add(vcid);
                                }
                                return list_result;
                            }//
                            else
                            {
                                throw new ArgumentException("Add Department Failed");
                            }
                        }//
                        else
                        {
                            throw new ArgumentException("Department already in Competition");
                        }
                    }// end if CheckDepartmentId
                    else
                    {
                        throw new ArgumentException("Department Id not have in University");
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

        //----ROLE LEADER OF CLUB
        public async Task<bool> LeaderUpdate(LeaderUpdateCompOrEventModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                   || model.ClubId == 0)
                    throw new ArgumentNullException("|| Competition Id Null  " +
                                                     " ClubId Null");

                bool Check = await CheckConditions(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //check date
                    bool checkDate = false;
                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    //------------- CHECK Date Update
                    //TH1 STR
                    if (model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && !model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate((DateTime)model.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, comp.EndTime, true);
                    }
                    //TH2 ETR
                    if (!model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && !model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(comp.StartTimeRegister, (DateTime)model.EndTimeRegister, comp.StartTime, comp.EndTime, true);
                    }
                    //TH3 ST
                    if (!model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && model.StartTime.HasValue && !model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(comp.StartTimeRegister, comp.EndTimeRegister, (DateTime)model.StartTime, comp.EndTime, true);
                    }
                    //TH4 ET
                    if (!model.StartTimeRegister.HasValue && !model.EndTimeRegister.HasValue && !model.StartTime.HasValue && model.EndTime.HasValue)
                    {
                        checkDate = CheckDate(comp.StartTimeRegister, comp.EndTimeRegister, comp.StartTime, (DateTime)model.EndTime, true);
                    }
                    //TH5 new STR ETR ST ET
                    if (model.StartTimeRegister.HasValue && model.EndTimeRegister.HasValue && model.StartTime.HasValue && model.EndTime.HasValue)
                    {
                        checkDate = CheckDate((DateTime)model.StartTimeRegister, (DateTime)model.EndTimeRegister, (DateTime)model.StartTime, (DateTime)model.EndTime, true);
                    }
                    if (checkDate)
                    {
                        comp.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : comp.SeedsPoint;
                        comp.SeedsDeposited = (model.SeedsDeposited != 0) ? model.SeedsDeposited : comp.SeedsDeposited;
                        comp.AddressName = (model.AddressName.Length > 0) ? model.AddressName : comp.AddressName;
                        comp.Address = (model.Address.Length > 0) ? model.Address : comp.Address;
                        comp.Name = (model.Name.Length > 0) ? model.Name : comp.Name;
                        comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                        comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                        comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                        comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
                        comp.Content = (!string.IsNullOrEmpty(model.Content)) ? model.Content : comp.Content;
                        comp.Fee = (double)((model.Fee.HasValue) ? model.Fee : comp.Fee);
                        //
                        await _competitionRepo.Update();
                        return true;

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

        //----ROLE MANAGER      
        public async Task<bool> LeaderDelete(LeaderDeleteCompOrEventModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                    || model.ClubId == 0)
                    throw new ArgumentNullException(" Competition Id Null || ClubId Null");

                bool Check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //
                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    if (comp != null)
                    {
                        comp.Status = CompetitionStatus.Canceling;
                        //
                        await _competitionRepo.Update();
                        return true;
                    }//end if comp != null
                    else
                    {
                        return false;
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

        public async Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token)
        {
            try
            {
                if (model.ClubIdCollaborate == 0
                   || model.CompetitionId == 0
                   || model.ClubId == 0)
                    throw new ArgumentNullException("Club Id Collaborate Null || Competition Id Null || Club Id Null");

                bool Check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (Check)
                {
                    //add 2 parameter to check
                    int UniversityId = DecodeToken(token, "UniversityId");
                    Competition competition = await _competitionRepo.Get(model.CompetitionId);
                    //------------ CHECK 2 club are the same 
                    if (model.ClubIdCollaborate != model.ClubId)
                    {
                        //---------------CHECK Club-Id-Collaborate----------
                        //check club Id Collaborate has in system
                        Club clubCollaborate = await _clubRepo.Get(model.ClubIdCollaborate);
                        if (clubCollaborate != null)
                        {
                            //
                            bool checkClubIn_Out = false;
                            //public == false just for club inside University 
                            if (competition.Public == false)
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

                                    //Add Club Manager Of Club Collaborate in Competition Manager
                                    Member ClubLeaderCollaborate = await _memberRepo.GetLeaderByClub(model.ClubIdCollaborate);
                                    CompetitionManager competitionManager = new CompetitionManager()
                                    {
                                        MemberId = ClubLeaderCollaborate.Id,
                                        CompetitionInClubId = result,
                                        CompetitionRoleId = 1,
                                        Fullname = ClubLeaderCollaborate.User.Fullname
                                    };
                                    await _competitionManagerRepo.Insert(competitionManager);

                                    return TransferViewCompetitionInClub(cic);
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
                    }//end check 2 club are the same 
                    else
                    {
                        throw new ArgumentException("Club is the same ");
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

        public async Task<List<ViewInfluencerInCompetition>> AddInfluencerInCompetition(InfluencerInComeptitionInsertModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                     || model.ClubId == 0
                     || model.ListInfluencerId.Count < 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null || List Influencer Id Null");

                bool check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (check)
                {
                    //------------- CHECK Influencer belong to system
                    bool influencerBelongToSystem = await _influencerRepo.CheckInfluencerInSystem(model.ListInfluencerId);
                    if (influencerBelongToSystem)
                    {
                        //------------- CHECK Add Influencer Id has already existed in competition
                        bool InfluencerIsExsited = true;
                        foreach (int influ_id in model.ListInfluencerId)
                        {
                            InfluencerInCompetition iic = await _influencerInCompetitionRepo.GetInfluencerInCompetition(influ_id, model.CompetitionId);
                            if (iic != null)
                            {
                                InfluencerIsExsited = false;
                            }
                        }
                        if (InfluencerIsExsited)
                        {
                            List<int> list_iic_Id = new List<int>();
                            List<ViewInfluencerInCompetition> list_result = new List<ViewInfluencerInCompetition>();

                            foreach (int influ_id in model.ListInfluencerId)
                            {
                                InfluencerInCompetition influ_in_comp = new InfluencerInCompetition()
                                {
                                    CompetitionId = model.CompetitionId,
                                    InfluencerId = influ_id,
                                };
                                int id = await _influencerInCompetitionRepo.Insert(influ_in_comp);
                                list_iic_Id.Add(id);
                            }

                            if (list_iic_Id.Count > 0)
                            {
                                foreach (int id in list_iic_Id)
                                {
                                    InfluencerInCompetition iic = await _influencerInCompetitionRepo.Get(id);

                                    ViewInfluencerInCompetition viic = new ViewInfluencerInCompetition()
                                    {
                                        CompetitionId = iic.CompetitionId,
                                        InfluencerInCompetitionId = iic.Id,
                                        Id = iic.Influencer.Id,
                                        ImageUrl = iic.Influencer.ImageUrl,
                                        Name = iic.Influencer.Name
                                    };
                                    list_result.Add(viic);
                                }
                                return list_result;
                            }
                            else
                            {
                                throw new ArgumentException("Add Influencer Failed");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Influencer already in Competition");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Influencer Id not have in System");
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

        public async Task<bool> DeleteInluencerInCompetition(InfluencerInCompetitionDeleteModel model, string token)
        {
            try
            {
                if (model.CompetitionId == 0
                     || model.ClubId == 0
                     || model.InfluencerId == 0)
                    throw new ArgumentNullException("Competition Id Null || ClubId Null || List Influencer Id Null");

                bool check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
                if (check)
                {
                    Influencer influencer = await _influencerRepo.Get(model.InfluencerId);

                    if (influencer != null)
                    {
                        InfluencerInCompetition iic = await _influencerInCompetitionRepo.GetInfluencerInCompetition(model.InfluencerId, model.CompetitionId);
                        if (iic != null)
                        {
                            await _influencerInCompetitionRepo.DeleteInfluencerInCompetition(iic.Id);
                            return true;
                        }
                        else
                        {
                            throw new ArgumentException("Influencer Id not in Competition");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Influencer Id not have in System");
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
                        if (mem.ClubId != model.ClubId)
                        {
                            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(model.ClubId);
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
                                    CompetitionInClubId = cic.Id,
                                    CompetitionRoleId = 4, //auto role lowest in competition manager
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

        //public async Task<bool> UpdateMemberInCompetitionManager(CompetitionManagerUpdateModel model, string token)
        //{
        //    try
        //    {
        //        int UserId = DecodeToken(token, "Id");
        //        if (model.CompetitionId == 0
        //                || model.ClubId == 0
        //                || model.MemberId == 0)
        //            throw new ArgumentNullException("Competition Id Null || ClubId Null ||Member Id Null");

        //        bool check = await CheckCompetitionManager(token, model.CompetitionId, model.ClubId);
        //        if (check)
        //        {
        //            //------------- CHECK Id Member
        //            Member mem = await _memberRepo.Get(model.MemberId);
        //            if (mem != null)
        //            {
        //                //------------- CHECK Id Member in club 
        //                if (mem.ClubId != model.ClubId)
        //                {
        //                    ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(model.ClubId);
        //                    //------------- CHECK Id Member the same as manager                   
        //                    GetMemberInClubModel conditions = new GetMemberInClubModel()
        //                    {
        //                        UserId = UserId,
        //                        ClubId = model.ClubId,
        //                        TermId = CurrentTermOfCLub.Id
        //                    };
        //                    ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
        //                    if (infoClubMem.Id != model.MemberId)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        throw new ArgumentException("Member id is the same");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new ArgumentException("Member is not in club");
        //                }
        //            }
        //            //
        //            else
        //            {
        //                throw new ArgumentException("Member not found");
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //----ROLE SPONSOR
        public async Task<ViewSponsorInCompetition> AddSponsorCollaborate(SponsorInCompetitionInsertModel model, string token)
        {
            try
            {

                int UserId = DecodeToken(token, "Id");
                int SponsorId = DecodeToken(token, "SponsorId");

                if (model.CompetitionId == 0) throw new ArgumentNullException("Competition Id Null");

                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------------------------------check-sponsor-id-create-competition-or-event-duplicate
                    //true  -> có nghĩa là chưa tạo -> continute
                    //false -> là sponsor này đã có trong cuộc thi r -> lỗi thêm sponsor
                    bool checkCreateSponsorInCompetition = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(SponsorId, model.CompetitionId);
                    if (checkCreateSponsorInCompetition)
                    {
                        SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                        sponsorInCompetition.SponsorId = SponsorId;
                        sponsorInCompetition.CompetitionId = model.CompetitionId;
                        //sponsorInCompetition.Status = SponsorInCompetitionStatus.Waiting;

                        int result = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);
                        if (result > 0)
                        {
                            SponsorInCompetition sic = await _sponsorInCompetitionRepo.Get(result);
                            //UPDATE IsSponsor của competition -> IsSponsor true                                               
                            competition.IsSponsor = true;
                            await _competitionRepo.Update();
                            return TransferViewSponsorInCompetition(sic);
                        }
                        else
                        {
                            throw new ArgumentException("Add Sponsor failed");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Sponsor has already submit");
                    }
                }
                else
                {
                    throw new ArgumentException("Competition or Event not found ");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        //transfer view

        private ViewCompetitionInClub TransferViewCompetitionInClub(CompetitionInClub competitionInClub)
        {
            return new ViewCompetitionInClub()
            {
                Id = competitionInClub.Id,
                ClubId = competitionInClub.ClubId,
                CompetitionId = competitionInClub.CompetitionId,
            };
        }

        public async Task<ViewDetailCompetition> TransformViewDetailCompetition(Competition competition)
        {

            //List Sponsors in Competition
            List<ViewSponsorInComp> SponsorsInCompetition = await _sponsorInCompetitionRepo.GetListSponsor_In_Competition(competition.Id);

            //List Clubs in Comeptition
            List<ViewClubInComp> ClubsInCompetition = await _competitionInClubRepo.GetListClub_In_Competition(competition.Id);

            //List Department in Competition
            List<ViewDeparmentInComp> DepartmentsInCompetition_Id = await _competitionInDepartmentRepo.GetListDepartment_In_Competition(competition.Id);

            //Number Of Participant Join This Competition
            int NumberOfParticipantJoin = await _participantRepo.NumOfParticipant(competition.Id);

            //competition type name
            CompetitionType competitionType = await _competitionTypeRepo.Get(competition.Id);
            string competitionTypeName = competitionType.TypeName;

            //Img Url
            CompetitionEntity compeEntity = await _competitionEntityRepo.Get(competition.Id);
            string imgUrl = compeEntity.ImageUrl;
            return new ViewDetailCompetition()
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,
                CompetitionTypeName = competitionTypeName,
                Address = competition.Address,
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfTeam = competition.NumberOfTeam,
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
                Public = competition.Public,
                //Status = competition.Status,
                View = competition.View,
                //
                ClubInCompetition = ClubsInCompetition,
                //
                SponsorInCompetition = SponsorsInCompetition,
                //
                DepartmentInCompetition = DepartmentsInCompetition_Id,
                //
                NumberOfParticipantJoin = NumberOfParticipantJoin,
                //
                ImgUrl = imgUrl
            };
        }

        private ViewSponsorInCompetition TransferViewSponsorInCompetition(SponsorInCompetition sponsorInCompetition)
        {
            return new ViewSponsorInCompetition()
            {
                Id = sponsorInCompetition.Id,
                SponsorId = sponsorInCompetition.SponsorId,
                CompetitionId = sponsorInCompetition.CompetitionId,

            };
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
        private bool CheckDate(DateTime StartTimeRegister, DateTime EndTimeRegister, DateTime StartTime, DateTime EndTime, bool Update)
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
                DateTime localTime = new LocalTime().GetLocalTime().DateTime;
                // resultLT1 < STR (sớm hơn)
                int resultLT1 = DateTime.Compare(localTime, StartTimeRegister);
                if (resultLT1 < 0)
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

        private bool CheckNumber_Team(int numberOfTeam, int numberOfMember)
        {
            if (numberOfMember % numberOfTeam == 0)
            {
                return true;
            }
            return false;
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
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
                        CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(CompetitionId, ClubId, infoClubMem.Id);
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
                            throw new UnauthorizedAccessException("You do not have permission to do this action");
                        }
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("You are not member in Club");
                    }
                }
                else
                {
                    throw new ArgumentException("Club in not found");
                }
            }
            else
            {
                throw new ArgumentException("Competition or Event not found ");
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
                        CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(CompetitionId, ClubId, infoClubMem.Id);
                        if (isAllow != null)
                        {
                            return true;
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("You do not have permission to do this action");
                        }
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("You are not member in Club");
                    }
                }
                else
                {
                    throw new ArgumentException("Club in not found");
                }
            }
            else
            {
                throw new ArgumentException("Competition or Event not found ");
            }
        }


    }
}
