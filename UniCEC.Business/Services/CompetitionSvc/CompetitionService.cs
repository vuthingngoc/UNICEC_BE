using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
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

        public CompetitionService(ICompetitionRepo competitionRepo, 
                                  IClubHistoryRepo clubHistoryRepo)
        {
            _competitionRepo = competitionRepo;
            _clubHistoryRepo = clubHistoryRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
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
        public async Task<List<ViewCompetition>> GetTop3CompOrEve_Status(bool? Event, CompetitionStatus? Status, bool? Public)
        {
            List<ViewCompetition> result = await _competitionRepo.GetTop3CompOrEve_Status(Event, Status, Public);
            return result;
        }


        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            PagingResult<ViewCompetition> result = await _competitionRepo.GetCompOrEve(request);
            return result;  
        }

        public async Task<ViewCompetition> Insert(CompetitionInsertModel model)
        {
            try
            {

                bool roleLeader = false;
                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = model.UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                //------------ Check Mem in that club
                if (infoClubMem != null)
                {
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }
                }
                //------------ Check Role Member Is Leader 
                if (roleLeader)
                {
                    //ở trong trường hợp này phân biệt EVENT - COMPETITION
                    //thì ta sẽ phân biệt bằng ==> NumberOfGroup = 0
                    Competition competition = new Competition();
  
                    competition.CompetitionTypeId = model.CompetitionTypeId;
                    competition.Address = model.Address;
                    // Nếu NumberOfGroup có giá trị là = 0 => đó là đang create EVENT
                    competition.NumberOfGroup = model.NumberOfGroups;       
                    competition.NumberOfParticipation = model.NumberOfParticipations;
                    competition.StartTime = model.StartTime;
                    competition.EndTime = model.EndTime;
                    competition.StartTimeRegister = model.StartTimeRegister;
                    competition.EndTimeRegister = model.EndTimeRegister;

                    //LƯU Ý ĐÃ BỎ TRƯỜNG ApprovedTime  
                    competition.ApprovedTime = DateTime.Now;

                    competition.SeedsPoint = model.SeedsPoint;
                    competition.SeedsDeposited = model.SeedsDeposited;
                    competition.SeedsCode = await checkExistCode();
                    //auto status = NotAssigned 
                    //-> tạo thành công nhưng chưa đc assigned cho 1 or các clb
                    competition.Status = CompetitionStatus.NotAssigned;
                    //
                    competition.Public = model.Public;
                    //auto = 0
                    competition.View = 0;
                    //
                    int result = await _competitionRepo.Insert(competition);
                    if (result > 0)
                    {
                        Competition comp = await _competitionRepo.Get(result);
                        ViewCompetition viewCompetition = TransformViewModel(comp);
                        return viewCompetition;
                    }
                    else
                    {
                        return null;
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

        public Task<bool> Update(ViewCompetition competition)
        {
            throw new NotImplementedException();
        }

        public ViewCompetition TransformViewModel(Competition competition)
        {
            return new ViewCompetition()
            {
                Id = competition.Id,
                CompetitionTypeId = competition.CompetitionTypeId,

                //address
                Address = competition.Address,
                //Group member
                NumberOfParticipation = competition.NumberOfParticipation,
                NumberOfGroup = competition.NumberOfGroup,
                //Time
                StartTime = competition.StartTime,
                EndTime = competition.EndTime,
                StartTimeRegister = competition.StartTimeRegister,
                EndTimeRegister = competition.EndTimeRegister,
                //ApprovedTime = competition.ApprovedTime,
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

       
    }
}
