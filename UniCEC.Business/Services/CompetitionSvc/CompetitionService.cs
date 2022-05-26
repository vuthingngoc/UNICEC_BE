using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

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

        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IClubHistoryRepo clubHistoryRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  ISponsorInCompetitionRepo sponsorInCompetitionRepo,
                                  ICompetitionInDepartmentRepo competitionInDepartmentRepo,
                                  IDepartmentInUniversityRepo departmentInUniversityRepo,
                                  IClubRepo clubRepo,
                                  ISponsorRepo sponsorRepo,
                                  IDepartmentRepo departmentRepo)
        {
            _competitionRepo = competitionRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _competitionInDepartmentRepo = competitionInDepartmentRepo;
            _departmentInUniversityRepo = departmentInUniversityRepo;
            _clubRepo = clubRepo;
            _sponsorRepo = sponsorRepo;
            _departmentRepo = departmentRepo;
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
        public async Task<ViewDetailCompetition> LeaderInsert(LeaderInsertCompOrEventModel model, string token)
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
                    //------------ Check Role Member Is Leader 
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

                                bool insertDepartment = false;
                                //------------Check FK
                                if (model.ListDepartmentId.Count > 0)
                                {
                                    bool departmentBelongToUni = await CheckDepartmentId(model.ListDepartmentId, UniversityId);
                                    if (departmentBelongToUni == false)
                                    {
                                        throw new ArgumentException("Department Id not have in University");
                                    }// end if CheckDepartmentId
                                    else
                                    {
                                        insertDepartment = true;
                                    }
                                }

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

                                    //------------ Insert Competition-In-Department -----------
                                    if (insertDepartment)
                                    {
                                        foreach (int dep_id in model.ListDepartmentId)
                                        {
                                            CompetitionInDepartment comp_in_dep = new CompetitionInDepartment()
                                            {
                                                DepartmentId = dep_id,
                                                CompetitionId = competition_Id
                                            };
                                            await _competitionInDepartmentRepo.Insert(comp_in_dep);
                                        }
                                    }

                                    //------------ Insert Competition-Entities-----------
                                    //
                                    //... to be continuted

                                    //------------ Insert Competition-In-Club
                                    CompetitionInClub competitionInClub = new CompetitionInClub();
                                    competitionInClub.ClubId = model.ClubId;
                                    competitionInClub.CompetitionId = competition_Id;
                                    int compInClub_Id = await _competitionInClubRepo.Insert(competitionInClub);

                                    if (compInClub_Id > 0)
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

        //Sponsor Insert
        //Check Authorize Sponsor in controller
        public async Task<ViewDetailCompetition> SponsorInsert(SponsorInsertCompOrEventModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);

                if (string.IsNullOrEmpty(model.Name)
                    || string.IsNullOrEmpty(model.Content)
                    || string.IsNullOrEmpty(model.AddressName)
                    || string.IsNullOrEmpty(model.Address)
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.NumberOfTeam < 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint == 0
                    || model.SeedsDeposited == 0)
                    throw new ArgumentNullException("Name Null || Content Null || Address || AddressName || CompetitionTypeId Null || NumberOfParticipations Null || NumberOfTeam Null || StartTimeRegister Null " +
                                                     " EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint Null || SeedsDeposited Null ");

                //------------ Check Date
                bool checkDate = CheckDate(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime, false);
                if (checkDate)
                {
                    if (CheckNumber_Team(model.NumberOfTeam, model.NumberOfParticipations))
                    {
                        //------------Check FK 
                        bool insertDepartment = false;
                        if (model.ListDepartmentId.Count > 0)
                        {
                            bool departmentInSystem = await CheckDepartmentId(model.ListDepartmentId);
                            if (departmentInSystem == false)
                            {
                                throw new ArgumentException("Department Id not have in System");
                            }// end if CheckDepartmentId
                            else
                            {
                                insertDepartment = true;
                            }
                        }

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
                        competition.IsSponsor = true;
                        competition.Status = CompetitionStatus.Launching;
                        //auto true
                        competition.Public = true;
                        //auto = 0
                        competition.View = 0;
                        int competition_Id = await _competitionRepo.Insert(competition);
                        if (competition_Id > 0)
                        {
                            Competition comp = await _competitionRepo.Get(competition_Id);


                            //------------ Insert Competition-In-Department -----------
                            if (insertDepartment)
                            {
                                foreach (int dep_id in model.ListDepartmentId)
                                {
                                    CompetitionInDepartment comp_in_dep = new CompetitionInDepartment()
                                    {
                                        DepartmentId = dep_id,
                                        CompetitionId = comp.Id
                                    };
                                    await _competitionInDepartmentRepo.Insert(comp_in_dep);
                                }
                            }

                            //------------ Insert Competition-Entities-----------(OPTIONAL 2)
                            //
                            //... to be continuted


                            //------------ Sponsor-In-Competition -----------
                            SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                            sponsorInCompetition.SponsorId = SponsorId;
                            sponsorInCompetition.CompetitionId = competition_Id;
                            int spoInCom_Id = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);

                            if (spoInCom_Id > 0)
                            {
                                ViewDetailCompetition viewDetailCompetition = await TransformViewDetailCompetition(comp);
                                return viewDetailCompetition;
                            }//end if spoInCom_Id > 0
                            else
                            {
                                throw new ArgumentException("Add Competition Or Event Failed");
                            }
                        }//end if competition_Id > 0
                        else
                        {
                            throw new ArgumentException("Add Competition Or Event Failed");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Number of Team and Number of Member not suitable");
                    }
                }//end if check date
                else
                {
                    throw new ArgumentException("Date not suitable");
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

                bool roleLeader = false;

                //Use method check
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                bool CompOrEventNotCreated = await _competitionInClubRepo.CheckDuplicateCreateCompetitionOrEvent(model.ClubId, model.CompetitionId);

                if (model.CompetitionId == 0
                    || model.ClubId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException("|| Competition Id Null  " +
                                                     " ClubId Null || TermId Null ");


                //------------ Check Club Has Create Competition
                if (CompOrEventNotCreated == false)
                {
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
                        //------------ Check Role Member Is Leader 
                        if (infoClubMem.ClubRoleName.Equals("Leader"))
                        {
                            roleLeader = true;
                        }

                        if (roleLeader)
                        {
                            //check date
                            bool checkDate = false;
                            Competition comp = await _competitionRepo.Get(model.CompetitionId);
                            //------------ Check Date Update
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
                        }//end check leader 
                        else
                        {
                            throw new UnauthorizedAccessException("You do not a role Leader to update this Competition");
                        }
                    }//end check member
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't member in Club");
                    }
                }// competition is not created
                else
                {
                    throw new ArgumentException("Competition not found to update");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> SponsorUpdate(SponsorUpdateCompOrEvent model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);

                //check date
                bool checkDate = false;
                //Use method check
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                bool CompOrEventNotCreated = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(SponsorId, model.CompetitionId);

                if (model.CompetitionId == 0)
                    throw new ArgumentNullException("|| Competition Id Null");

                //------------ Check Sponsor Has Create Competition
                if (CompOrEventNotCreated == false)
                {
                    Competition comp = await _competitionRepo.Get(model.CompetitionId);
                    //------------ Check Date Update
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
                }//end check CompOrEventNotCreated
                else
                {
                    throw new ArgumentException("Competition not found to update");
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

                bool roleLeader = false;
                //Use method check  
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                bool CompOrEventNotCreated = await _competitionInClubRepo.CheckDuplicateCreateCompetitionOrEvent(model.ClubId, model.CompetitionId);

                if (model.CompetitionId == 0
                    || model.ClubId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException(" Competition Id Null || ClubId Null || TermId Null ");

                //------------ Check Club Has Create Competition
                if (CompOrEventNotCreated == false)
                {
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
                        //------------ Check Role Member Is Leader 
                        if (infoClubMem.ClubRoleName.Equals("Leader"))
                        {
                            roleLeader = true;
                        }

                        if (roleLeader)
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
                        }//end check role leader
                        else
                        {
                            throw new UnauthorizedAccessException("You do not a role Leader to Delete this Competition");
                        }
                    }// check member
                    else
                    {
                        throw new UnauthorizedAccessException("You aren't member in Club");
                    }
                }//end clubHasCreateCompetition
                else
                {
                    throw new ArgumentException("Competition not found to Delete");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SponsorDelete(SponsorDeleteCompOrEventModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);
                //Use method check  
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                bool CompOrEventNotCreated = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(SponsorId, model.CompetitionId);

                if (model.CompetitionId == 0)
                    throw new ArgumentNullException("|| Competition Id Null");

                if (CompOrEventNotCreated == false)
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
                    throw new ArgumentException("Competition not found to Delete");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewDetailCompetition> TransformViewDetailCompetition(Competition competition)
        {

            //List Sponsors in Competition
            List<int> SponsorsInCompetition_Id = await _sponsorInCompetitionRepo.GetListSponsorId_In_Competition(competition.Id);

            //List Clubs in Comeptition
            List<int> ClubsInCompetition_Id = await _competitionInClubRepo.GetListClubId_In_Competition(competition.Id);

            //List Department in Competition
            List<int> DepartmentsInCompetition_Id = await _competitionInDepartmentRepo.GetListDepartmentId_In_Competition(competition.Id);


            return new ViewDetailCompetition()
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,                
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
                ClubInCompetition_Id = ClubsInCompetition_Id,
                //
                SponsorInCompetition_Id = SponsorsInCompetition_Id,
                //
                DepartmentInCompetition_Id = DepartmentsInCompetition_Id
            };
        }



        public ViewCompetition TransformViewCompetition(Competition competition)
        {
          
            return new ViewCompetition()
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,
                AddressName = competition.AddressName,             
                Address = competition.Address,               
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfTeam = competition.NumberOfTeam,
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
                View = competition.View              
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

        //Check exist Department id in System
        private async Task<bool> CheckDepartmentId(List<int> listDepartmentId)
        {
            bool result = await _departmentRepo.checkDepartment(listDepartmentId);
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

        private ViewCompetitionInClub TransferViewCompetitionInClub(CompetitionInClub competitionInClub)
        {
            return new ViewCompetitionInClub()
            {
                Id = competitionInClub.Id,
                ClubId = competitionInClub.ClubId,
                CompetitionId = competitionInClub.CompetitionId,
            };
        }


        //----------------------------------------------------------------------------------------Sponsor-In-Competition
        public async Task<ViewSponsorInCompetition> AddSponsorCollaborate(SponsorInCompetitionInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);

                if (model.SponsorIdCollaborate == 0
                   || model.CompetitionId == 0)
                    throw new ArgumentNullException("Sponsor Id Collaborate Null || Competition Id Null");
                //------------------------------------ 2 sponsor are the same 
                if (model.SponsorIdCollaborate != SponsorId)
                {
                    //------------------------------------check-sponsor-id-create-competition-or-event-duplicate
                    bool checkCreateSponsorInCompetition = await _sponsorInCompetitionRepo.CheckDuplicateCreateCompetitionOrEvent(SponsorId, model.CompetitionId);
                    //true  -> có nghĩa là nó chưa được tạo -> kh thể add được 
                    //false -> có nghĩa là nó chưa được tạo -> add được (do đây là add thêm Sponsor collaborate)
                    if (checkCreateSponsorInCompetition == false)
                    {
                        //------------------------------------Check Sponsor Collaborate in System 
                        Sponsor sponsor = await _sponsorRepo.Get(model.SponsorIdCollaborate);
                        if (sponsor != null)
                        {
                            SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                            sponsorInCompetition.SponsorId = model.SponsorIdCollaborate;
                            sponsorInCompetition.CompetitionId = model.CompetitionId;

                            int result = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);
                            if (result > 0)
                            {
                                SponsorInCompetition sic = await _sponsorInCompetitionRepo.Get(result);
                                return TransferViewSponsorInCompetition(sic);
                            }//end result
                            else
                            {
                                throw new ArgumentException("Add Competition Or Event Failed");
                            }
                        }//end check sponsor Collaborate
                        else
                        {
                            throw new ArgumentException("Sponsor collaborate not in system");
                        }
                    }//end check exsit Competition Or Event
                    else
                    {
                        throw new ArgumentException("Competition or Event not found");
                    }
                }// end 2 sponsor is the same 
                else
                {
                    throw new ArgumentException("Sponsor already join");
                }
            }
            catch (Exception)
            {
                throw;
            }
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

    }
}
