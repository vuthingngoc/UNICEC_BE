using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionInClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
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
        // check Club Has Competition   
        private ICompetitionInClubRepo _competitionInClubRepo;

        public CompetitionService(ICompetitionRepo competitionRepo,
                                  IClubHistoryRepo clubHistoryRepo,
                                  ICompetitionInClubRepo competitionInClubRepo)
        {
            _competitionRepo = competitionRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _competitionInClubRepo = competitionInClubRepo;

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

        //Sponsor Insert
        public async Task<ViewCompetition> SponsorInsert(CompetitionInsertModel model)
        {
            //Check Authorize Sponsor in controller
            try
            {
                //------------ Check Date
                bool checkDate = CheckDateInsert(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime);
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
                    int result = await _competitionRepo.Insert(competition);
                    if (result > 0)
                    {
                        Competition comp = await _competitionRepo.Get(result);
                        ViewCompetition viewCompetition = TransformViewModel(comp);
                        return viewCompetition;
                    }//end if result != 0
                    else
                    {
                        return null;
                    }
                }//end if check date
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

        //Leader Insert
        public async Task<ViewCompetition> LeaderInsert(CompetitionInsertModel model)
        {
            try
            {
                bool roleLeader = false;
                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = (int)model.UserId,
                    ClubId = (int)model.ClubId,
                    TermId = (int)model.TermId
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
                    bool checkDate = CheckDateInsert(model.StartTimeRegister, model.EndTimeRegister, model.StartTime, model.EndTime);
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
                        //nếu là leader -> IsSponsor == false
                        competition.IsSponsor = false;
                        //auto status = NotAssigned 
                        //-> tạo thành công nhưng chưa đc assigned cho 1 or các clb
                        competition.Status = CompetitionStatus.NotAssigned;
                        competition.Public = model.Public;
                        //auto = 0
                        competition.View = 0;
                        int result = await _competitionRepo.Insert(competition);
                        if (result > 0)
                        {
                            Competition comp = await _competitionRepo.Get(result);
                            ViewCompetition viewCompetition = TransformViewModel(comp);
                            return viewCompetition;
                        }//end if result != 0
                        else
                        {
                            return null;
                        }
                    }//end if check date
                    else
                    {
                        return null;
                    }
                }//end if role leader
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

        public async Task<bool> Update(CompetitionUpdateModel competition)
        {
            try
            {
                bool roleLeader = false;
                bool clubHasCreateCompetition = false;

                //Use method check
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                clubHasCreateCompetition = await _competitionInClubRepo.CheckDuplicateCreateCompetitionOrEvent(competition.ClubId, competition.Id);
                //------------ Check Club Has Create Competition
                if (clubHasCreateCompetition == false)
                {
                    GetMemberInClubModel conditions = new GetMemberInClubModel()
                    {
                        UserId = competition.UserId,
                        ClubId = competition.ClubId,
                        TermId = competition.TermId
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
                        Competition comp = await _competitionRepo.Get(competition.Id);
                        comp.SeedsPoint = (competition.SeedsPoint != 0) ? competition.SeedsPoint : comp.SeedsPoint;
                        comp.SeedsDeposited = (competition.SeedsDeposited != 0) ? competition.SeedsDeposited : comp.SeedsDeposited;
                        comp.Address = (competition.Address.Length > 0) ? competition.Address : comp.Address;
                        comp.Name = (competition.Name.Length > 0) ? competition.Name : comp.Name;
                        //
                        await _competitionRepo.Update();
                        return true;
                    }
                    else
                    {
                        return false;
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


        public async Task<bool> Delete(CompetitionDeleteModel competition)
        {
            try
            {
                bool roleLeader = false;
                bool clubHasCreateCompetition = false;

                //Use method check
                //-> if FALSE mean it's created -> Can Update
                //-> if TRUE mean it isn't created -> Can't Update
                clubHasCreateCompetition = await _competitionInClubRepo.CheckDuplicateCreateCompetitionOrEvent(competition.ClubId, competition.Id);
                //------------ Check Club Has Create Competition
                if (clubHasCreateCompetition == false)
                {
                    GetMemberInClubModel conditions = new GetMemberInClubModel()
                    {
                        UserId = competition.UserId,
                        ClubId = competition.ClubId,
                        TermId = competition.TermId
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
                        Competition comp = await _competitionRepo.Get(competition.Id);
                        comp.Status = CompetitionStatus.Canceling;
                        //
                        await _competitionRepo.Update();
                        return true;
                    }
                    else
                    {
                        return false;
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

        public ViewCompetition TransformViewModel(Competition competition)
        {
            return new ViewCompetition()
            {
                Id = competition.Id,
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

        //Check Date Insert
        private bool CheckDateInsert(DateTime StartTimeRegister, DateTime EndTimeRegister, DateTime StartTime, DateTime EndTime)
        {

            //condition
            bool round1 = false;
            bool round2 = false;
            bool round3 = false;
            bool result = false;

            //ROUND 1
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
                        round1 = true;
                    }
                }//end kq2
            }//end kq1

            //ROUND2
            //ETR < ST < ET -> ETR true
            if (round1)
            {
                //kq 4 < 0 -> ETR < ST (sớm hơn)
                int kq4 = DateTime.Compare(EndTimeRegister, StartTime);
                if (kq4 < 0)
                {
                    //kq 5 < 0 -> ETR < ET (sớm hơn)
                    int kq5 = DateTime.Compare(EndTimeRegister, EndTime);
                    if (kq5 < 0)
                    {
                        round2 = true;
                    }
                }
            }

            //ROUND 3
            //ST  < ET - > ST true
            if (round1 && round2)
            {
                //kq 6 < 0 -> ST < ET (sớm hơn)
                int kq6 = DateTime.Compare(StartTime, EndTime);
                if (kq6 < 0)
                {
                    round3 = true;
                }
            }

            if (round1 && round2 && round3)
            {
                result = true;
            }

            return result;
        }


    }
}
