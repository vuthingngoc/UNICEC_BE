using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionEntityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionTypeRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;
using UniCEC.Data.ViewModels.Entities.CompetitionInDepartment;

namespace UniCEC.Business.Services.CompetitionSvc
{
    public class CompetitionService : ICompetitionService
    {
        private ICompetitionRepo _competitionRepo;
        //check Infomation Member -> is Leader
        private IClubHistoryRepo _clubHistoryRepo;
        // check Club Has Competition - Insert   
        private ICompetitionInClubRepo _competitionInClubRepo;
        // check Sponsor create Competition- Insert
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;
        // Insert Department in Competition
        private ICompetitionInDepartmentRepo _competitionInDepartmentRepo;
        //
        private IDepartmentInUniversityRepo _departmentInUniversityRepo;
        //
        private IDepartmentRepo _departmentRepo;
        //
        private IClubRepo _clubRepo;
        //
        private ISponsorRepo _sponsorRepo;
        //
        private IParticipantRepo _participantRepo;
        //
        private ICompetitionTypeRepo _competitionTypeRepo;
        //
        private IFileService _fileService;
        //
        private ICompetitionEntityRepo _competitionEntityRepo;
        //
        private ICompetitionManagerRepo _competitionManagerRepo;
        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IClubHistoryRepo clubHistoryRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  ISponsorInCompetitionRepo sponsorInCompetitionRepo,
                                  ICompetitionInDepartmentRepo competitionInDepartmentRepo,
                                  IDepartmentInUniversityRepo departmentInUniversityRepo,
                                  IClubRepo clubRepo,
                                  ISponsorRepo sponsorRepo,
                                  IParticipantRepo participantRepo,
                                  ICompetitionTypeRepo competitionTypeRepo,
                                  IDepartmentRepo departmentRepo,
                                  ICompetitionEntityRepo competitionEntityRepo,
                                  ICompetitionManagerRepo competitionManagerRepo,
                                  IFileService fileService)
        {
            _competitionRepo = competitionRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _competitionInDepartmentRepo = competitionInDepartmentRepo;
            _departmentInUniversityRepo = departmentInUniversityRepo;
            _clubRepo = clubRepo;
            _sponsorRepo = sponsorRepo;
            _participantRepo = participantRepo;
            _competitionTypeRepo = competitionTypeRepo;
            _departmentRepo = departmentRepo;
            _fileService = fileService;
            _competitionEntityRepo = competitionEntityRepo;
            _competitionManagerRepo = competitionManagerRepo;
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

        //Leader Insert
        public async Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel model, string token, IFormFile file)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var UniversityIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));

                int UserId = Int32.Parse(UserIdClaim.Value);
                int UniversityId = Int32.Parse(UniversityIdClaim.Value);

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
                    || model.ClubId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null || NumberOfTeam Null || StartTimeRegister Null " +
                                                    " EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint Null || SeedsDeposited Null || ClubId Null || TermId Null ");

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
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

                                //------------ Insert Competition
                                //ở trong trường hợp này phân biệt EVENT - COMPETITION
                                //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                                Competition competition = new Competition();
                                competition.CompetitionTypeId = model.CompetitionTypeId;
                                competition.AddressName = model.AddressName;
                                competition.Address = model.Address;
                                competition.Name = model.Name;
                                // Nếu NumberOfTeam có giá trị là = 0 => đó là đang create EVENT
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
                                //nếu là leader -> IsSponsor == false
                                competition.IsSponsor = false;
                                competition.Status = CompetitionStatus.Launching;
                                competition.Public = model.Public;
                                //auto = 0
                                competition.View = 0;
                                int competition_Id = await _competitionRepo.Insert(competition);
                                if (competition_Id > 0)
                                {
                                    Competition comp = await _competitionRepo.Get(competition_Id);

                                    //------------ Insert Competition-In-Club
                                    CompetitionInClub competitionInClub = new CompetitionInClub();
                                    competitionInClub.ClubId = model.ClubId;
                                    competitionInClub.CompetitionId = competition_Id;
                                    int compInClub_Id = await _competitionInClubRepo.Insert(competitionInClub);


                                    //------------ Insert Competition-Manager
                                    CompetitionManager cm = new CompetitionManager()
                                    {
                                        CompetitionInClubId = compInClub_Id,
                                        //auto role 1 Manager
                                        CompetitionRoleId = 1,
                                        MemberId = infoClubMem.MemberId,
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
            catch (Exception)
            {
                throw;
            }
        }


        //add Competition Entity
        public async Task<ViewCompetitionEntity> AddCompetitionEntity(CompetitionEntityInsertModel model, string token, IFormFile file)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);

                if (model.CompetitionId == 0
                  || model.ClubId == 0
                  || model.TermId == 0
                  || string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException("|| Competition Id Null  || Name Null" +
                                                     " ClubId Null || TermId Null ");

                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------- CHECK Club in system
                    Club club = await _clubRepo.Get(model.ClubId);
                    if (club != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = model.ClubId,
                            TermId = model.TermId
                        };
                        ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                        //------------- CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK is in CompetitionManger table
                            CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(model.CompetitionId, model.ClubId, infoClubMem.MemberId);
                            if (isAllow != null)
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
                            //end check allow
                            else
                            {
                                throw new UnauthorizedAccessException("You do not have permission to do this action");
                            }
                        }
                        //end check member
                        else
                        {
                            throw new UnauthorizedAccessException("You are not member in Club");
                        }
                    }
                    //end check club in system
                    else
                    {
                        throw new ArgumentException("Club in not found");
                    }
                }
                //end check competition 
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


        //add Competition In Department

        public async Task<List<ViewCompetitionInDepartment>> AddCompetitionInDepartment(CompetitionInDepartmentInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var UniversityIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));

                int UserId = Int32.Parse(UserIdClaim.Value);
                int UniversityId = Int32.Parse(UniversityIdClaim.Value);

                if (model.CompetitionId == 0
                  || model.ClubId == 0
                  || model.TermId == 0
                  || model.ListDepartmentId.Count < 0)
                    throw new ArgumentNullException("|| Competition Id Null  || Department Null" +
                                                     " ClubId Null || TermId Null ");
                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------- CHECK Club in system
                    Club club = await _clubRepo.Get(model.ClubId);
                    if (club != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = model.ClubId,
                            TermId = model.TermId
                        };
                        ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                        //------------- CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK is in CompetitionManger table
                            CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(model.CompetitionId, model.ClubId, infoClubMem.MemberId);
                            if (isAllow != null)
                            {
                                bool departmentBelongToUni = await CheckDepartmentId(model.ListDepartmentId, UniversityId);

                                List<int> list_dic_Id = new List<int>();

                                List<ViewCompetitionInDepartment> list_result = new List<ViewCompetitionInDepartment>();
                                //------------- CHECK Department belong to University
                                if (departmentBelongToUni)
                                {
                                    //------------- CHECK Add Department is existed
                                    bool DepartmentIsExsited = true;
                                    foreach (int dep_id in model.ListDepartmentId)
                                    {
                                        CompetitionInDepartment cid = await _competitionInDepartmentRepo.Get(dep_id);
                                        if (cid != null)
                                        {
                                            DepartmentIsExsited = false;
                                        }
                                    }
                                    if (DepartmentIsExsited)
                                    {
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
                            //end check allow
                            else
                            {
                                throw new UnauthorizedAccessException("You do not have permission to do this action");
                            }
                        }
                        //end check member
                        else
                        {
                            throw new UnauthorizedAccessException("You are not member in Club");
                        }
                    }
                    //end check club in system
                    else
                    {
                        throw new ArgumentException("Club in not found");
                    }
                }
                //end check competition 
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


        public async Task<bool> LeaderUpdate(LeaderUpdateCompOrEventModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);

                if (model.CompetitionId == 0
                   || model.ClubId == 0
                   || model.TermId == 0)
                    throw new ArgumentNullException("|| Competition Id Null  " +
                                                     " ClubId Null || TermId Null ");

                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------- CHECK Club in system
                    Club club = await _clubRepo.Get(model.ClubId);
                    if (club != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = model.ClubId,
                            TermId = model.TermId
                        };
                        ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                        //------------- CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK is in CompetitionManger table
                            CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(model.CompetitionId, model.ClubId, infoClubMem.MemberId);
                            if (isAllow != null)
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
                            }
                            //end check allow
                            else
                            {
                                throw new UnauthorizedAccessException("You do not have permission to do this action");
                            }
                        }//end check member
                        else
                        {
                            throw new UnauthorizedAccessException("You aren't member in Club");
                        }
                    }
                    //end check club in system
                    else
                    {
                        throw new ArgumentException("Club in not found");
                    }
                }
                else
                {
                    throw new ArgumentException("Competition or Event not found to update");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<bool> LeaderDelete(LeaderDeleteCompOrEventModel model, string token)
        {
            try
            {

                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);

                if (model.CompetitionId == 0
                    || model.ClubId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException(" Competition Id Null || ClubId Null || TermId Null ");

                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------- CHECK Club in system
                    Club club = await _clubRepo.Get(model.ClubId);
                    if (club != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = model.ClubId,
                            TermId = model.TermId
                        };
                        ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                        //------------ CHECK Mem in that club
                        if (infoClubMem != null)
                        {
                            //------------- CHECK is in CompetitionManger table
                            CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(model.CompetitionId, model.ClubId, infoClubMem.MemberId);
                            if (isAllow != null)
                            {
                                //------------- CHECK Role Is Manger
                                if (isAllow.CompetitionRoleId == 1)
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
                        //end not member in club
                        else
                        {
                            throw new UnauthorizedAccessException("You aren't member in Club");
                        }
                    } //end check club
                    else
                    {
                        throw new ArgumentException("Club not in the system");
                    }
                }// end check Competition not in system
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




        //ROLE MANAGER
        //----------------------------------------------------------------------------------------Competition-In-Club
        public async Task<ViewCompetitionInClub> AddClubCollaborate(CompetitionInClubInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var UniversityIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));

                int UserId = Int32.Parse(UserIdClaim.Value);

                int UniversityId = Int32.Parse(UniversityIdClaim.Value);

                if (model.ClubIdCollaborate == 0
                   || model.CompetitionId == 0
                   || model.ClubId == 0
                   || model.TermId == 0)
                    throw new ArgumentNullException("Club Id Collaborate Null || Competition Id Null || Club Id Null || Term Id Null ");

                //------------- CHECK Competition is have in system or not
                Competition competition = await _competitionRepo.Get(model.CompetitionId);
                if (competition != null)
                {
                    //------------- CHECK Club in system
                    Club club = await _clubRepo.Get(model.ClubId);
                    if (club != null)
                    {
                        GetMemberInClubModel conditions = new GetMemberInClubModel()
                        {
                            UserId = UserId,
                            ClubId = model.ClubId,
                            TermId = model.TermId
                        };
                        ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                        //------------ CHECK 2 club are the same 
                        if (model.ClubIdCollaborate != model.ClubId)
                        {
                            //------------ CHECK Mem in that club
                            if (infoClubMem != null)
                            {
                                //------------- CHECK is in CompetitionManger table
                                CompetitionManager isAllow = await _competitionManagerRepo.GetCompetitionManager(model.CompetitionId, model.ClubId, infoClubMem.MemberId);
                                if (isAllow != null)
                                {
                                    //------------- CHECK Role Is Manger
                                    if (isAllow.CompetitionRoleId == 1)
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
                                    }//end check role Manager
                                    else
                                    {
                                        throw new UnauthorizedAccessException("Only role Manager can do this action");
                                    }
                                }//end check allow
                                else
                                {
                                    throw new UnauthorizedAccessException("You do not have permission to do this action");
                                }
                            }//end not member in club
                            else
                            {
                                throw new UnauthorizedAccessException("You aren't member in Club");
                            }
                        }//end check 2 club are the same 
                        else
                        {
                            throw new ArgumentException("Club is the same ");
                        }
                    }
                    //end check club
                    else
                    {
                        throw new ArgumentException("Club not in the system");
                    }
                }// end check Competition not in system
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





        //ROLE SPONSOR
        //----------------------------------------------------------------------------------------Sponsor-In-Competition
        public async Task<ViewSponsorInCompetition> AddSponsorCollaborate(SponsorInCompetitionInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var SponsorIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));

                int UserId = Int32.Parse(UserIdClaim.Value);
                int SponsorId = Int32.Parse(SponsorIdClaim.Value);

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
                        sponsorInCompetition.Status = SponsorInCompetitionStatus.Waiting;

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

        //Transfer View
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
                Status = competition.Status,
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


        //generate Seed code length 10
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
        //----------------------------------------------------------------------------------------Check
        //check exist code
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

        //Check exist Department Id Belong To University - Leader
        private async Task<bool> CheckDepartmentId(List<int> listDepartmentId, int universityId)
        {
            //
            bool result = await _departmentInUniversityRepo.checkDepartmentBelongToUni(listDepartmentId, universityId);
            return result;
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

        //Check number of team - number of member in Team
        private bool CheckNumber_Team(int numberOfTeam, int numberOfMember)
        {
            if (numberOfMember % numberOfTeam == 0)
            {
                return true;
            }
            return false;
        }

    }
}
