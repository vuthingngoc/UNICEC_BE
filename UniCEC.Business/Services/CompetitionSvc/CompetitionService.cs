using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.Competition;

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


        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IClubHistoryRepo clubHistoryRepo,
                                  ICompetitionInClubRepo competitionInClubRepo,
                                  ISponsorInCompetitionRepo sponsorInCompetitionRepo)
        {
            _competitionRepo = competitionRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionInClubRepo = competitionInClubRepo;
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
        }



        public Task<PagingResult<ViewCompetition>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewCompetition> GetById(int id)
        {
            //
            Competition comp = await _competitionRepo.Get(id);

            //
            if (comp != null)
            {
                comp.View += 1;
                await _competitionRepo.Update();

                return TransformViewModel(comp);
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
            return result;
        }


        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEve(request);
            return result;
        }



        //Leader Insert
        public async Task<ViewCompetition> LeaderInsert(LeaderInsertCompOrEventModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);
                bool roleLeader = false;


                if (string.IsNullOrEmpty(model.Name)
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.NumberOfTeam == 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint == 0
                    || model.SeedsDeposited == 0
                    || model.ClubId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException("Name Null || CompetitionTypeId Null || NumberOfParticipations Null || NumberOfTeam Null || StartTimeRegister Null " +
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
                }
                if (roleLeader)
                {
                    //------------ Check Date
                    bool checkDate = CheckDate(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime, false);
                    if (checkDate)
                    {
                        //------------ Insert Competition
                        //ở trong trường hợp này phân biệt EVENT - COMPETITION
                        //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                        Competition competition = new Competition();
                        competition.CompetitionTypeId = model.CompetitionTypeId;
                        competition.Address = model.Address;
                        competition.Name = model.Name;
                        // Nếu NumberOfTeam có giá trị là = 0 => đó là đang create EVENT
                        competition.NumberOfTeam = model.NumberOfTeam;
                        competition.NumberOfParticipation = model.NumberOfParticipations;
                        competition.StartTime = model.StartTime;
                        competition.EndTime = model.EndTime;
                        competition.StartTimeRegister = model.StartTimeRegister;
                        competition.EndTimeRegister = model.EndTimeRegister;
                        competition.SeedsPoint = model.SeedsPoint;
                        competition.SeedsDeposited = model.SeedsDeposited;
                        competition.SeedsCode = await checkExistCode();
                        //nếu là leader -> IsSponsor == false
                        competition.IsSponsor = false;
                        //auto status = NotAssigned 
                        //-> tạo thành công nhưng chưa đc assigned cho 1 or các clb
                        competition.Status = CompetitionStatus.NotAssigned;
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
                            if (compInClub_Id > 0)
                            {
                                //---------Update Status Competition
                                Competition comp_Update = await _competitionRepo.Get(competition_Id);
                                comp_Update.Status = CompetitionStatus.Launching;
                                await _competitionRepo.Update();
                                ViewCompetition viewCompetition = TransformViewModel(comp);
                                return viewCompetition;
                            }
                            else
                            {
                                return null;
                            }//end compInClub_Id > 0
                        }//end if competition_Id > 0
                        else
                        {
                            return null;
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Sponsor Insert
        //Check Authorize Sponsor in controller
        public async Task<ViewCompetition> SponsorInsert(SponsorInsertCompOrEventModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var spIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("SponsorId"));
                int SponsorId = Int32.Parse(spIdClaim.Value);

                if (string.IsNullOrEmpty(model.Name)
                    || model.CompetitionTypeId == 0
                    || model.NumberOfParticipations == 0
                    || model.NumberOfTeam == 0
                    || model.StartTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTimeRegister == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.EndTime == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.SeedsPoint == 0
                    || model.SeedsDeposited == 0)
                    throw new ArgumentNullException("Name Null || CompetitionTypeId Null || NumberOfParticipations Null || NumberOfTeam Null || StartTimeRegister Null " +
                                                     " EndTimeRegister Null  || StartTime Null || EndTime Null ||  SeedsPoint Null || SeedsDeposited Null ");



                //------------ Check Date
                bool checkDate = CheckDate(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime, false);
                if (checkDate)
                {
                    //ở trong trường hợp này phân biệt EVENT - COMPETITION
                    //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                    Competition competition = new Competition();
                    competition.CompetitionTypeId = model.CompetitionTypeId;
                    competition.Address = model.Address;
                    competition.Name = model.Name;
                    // Nếu NumberOfTeam có giá trị là = 0 => đó là đang create EVENT
                    competition.NumberOfTeam = model.NumberOfTeam;
                    competition.NumberOfParticipation = model.NumberOfParticipations;
                    competition.StartTime = model.StartTime;
                    competition.EndTime = model.EndTime;
                    competition.StartTimeRegister = model.StartTimeRegister;
                    competition.EndTimeRegister = model.EndTimeRegister;
                    competition.SeedsPoint = model.SeedsPoint;
                    competition.SeedsDeposited = model.SeedsDeposited;
                    competition.SeedsCode = await checkExistCode();
                    competition.IsSponsor = true;
                    //auto status = NotAssigned 
                    //-> tạo thành công nhưng chưa đc assigned cho 1 Sponsor or các Sponsor
                    competition.Status = CompetitionStatus.NotAssigned;
                    competition.Public = model.Public;
                    //auto = 0
                    competition.View = 0;
                    int competition_Id = await _competitionRepo.Insert(competition);
                    if (competition_Id > 0)
                    {
                        Competition comp = await _competitionRepo.Get(competition_Id);
                        //------------ Sponsor-In-Competition
                        SponsorInCompetition sponsorInCompetition = new SponsorInCompetition();
                        sponsorInCompetition.SponsorId = SponsorId;
                        sponsorInCompetition.CompetitionId = competition_Id;

                        int spoInCom_Id = await _sponsorInCompetitionRepo.Insert(sponsorInCompetition);
                        if (spoInCom_Id > 0)
                        {
                            //---------Update Status Competition
                            Competition comp_Update = await _competitionRepo.Get(competition_Id);
                            comp_Update.Status = CompetitionStatus.Launching;
                            await _competitionRepo.Update();
                            ViewCompetition viewCompetition = TransformViewModel(comp);
                            return viewCompetition;
                        }
                        else
                        {
                            return null;
                        }//end if spoInCom_Id > 0
                    }//end if competition_Id > 0
                    else
                    {
                        return null;
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
                            comp.Address = (model.Address.Length > 0) ? model.Address : comp.Address;
                            comp.Name = (model.Name.Length > 0) ? model.Name : comp.Name;
                            comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                            comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                            comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                            comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
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
                        comp.Address = (model.Address.Length > 0) ? model.Address : comp.Address;
                        comp.Name = (model.Name.Length > 0) ? model.Name : comp.Name;
                        comp.StartTimeRegister = (DateTime)((model.StartTimeRegister.HasValue) ? model.StartTimeRegister : comp.StartTimeRegister);
                        comp.EndTimeRegister = (DateTime)((model.EndTimeRegister.HasValue) ? model.EndTimeRegister : comp.EndTimeRegister);
                        comp.StartTime = (DateTime)((model.StartTime.HasValue) ? model.StartTime : comp.StartTime);
                        comp.EndTime = (DateTime)((model.EndTime.HasValue) ? model.EndTime : comp.EndTime);
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
                    throw new ArgumentNullException("|| Competition Id Null" +
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

        public ViewCompetition TransformViewModel(Competition competition)
        {
            return new ViewCompetition()
            {
                CompetitionId = competition.Id,
                Name = competition.Name,
                CompetitionTypeId = competition.CompetitionTypeId,
                //address
                Address = competition.Address,
                //Group member
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfTeam = competition.NumberOfTeam,
                //Time
                StartTime = competition.StartTime,
                EndTime = competition.EndTime,
                StartTimeRegister = competition.StartTimeRegister,
                EndTimeRegister = competition.EndTimeRegister,
                //Seed code - point
                SeedsPoint = competition.SeedsPoint,
                SeedsDeposited = competition.SeedsDeposited,
                SeedsCode = competition.SeedsCode,
                //Scope
                Public = competition.Public,
                //Status
                Status = competition.Status,
                //View
                View = competition.View,
            };
        }

        //generate Seed code length 10
        private string generateSeedCode()
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

        //check exist code
        private async Task<string> checkExistCode()
        {
            //auto generate seedCode
            bool check = true;
            string seedCode = "";
            while (check)
            {
                string generateCode = generateSeedCode();
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


    }
}
