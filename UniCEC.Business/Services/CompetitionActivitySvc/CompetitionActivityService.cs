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
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

namespace UniCEC.Business.Services.CompetitionActivitySvc
{
    public class CompetitionActivityService : ICompetitionActivityService
    {
        private ICompetitionActivityRepo _competitionActivityRepo;
        //
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        //check Infomation Member -> is Leader
        private IClubHistoryRepo _clubHistoryRepo;
        private IClubRepo _clubRepo;

        public CompetitionActivityService(ICompetitionActivityRepo clubActivityRepo, IMemberTakesActivityRepo memberTakesActivityRepo, IClubHistoryRepo clubHistoryRepo, IClubRepo clubRepo)
        {
            _competitionActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _clubRepo = clubRepo;
        }

        //Delete-Club-Activity-By-Id
        public async Task<bool> Delete(CompetitionActivityDeleteModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);


                bool roleLeader = false;

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };

                ViewClubMember infoClubMem = new ViewClubMember();//await _clubHistoryRepo.GetMemberInCLub(conditions);
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
                        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.ClubActivityId);
                        if (competitionActivity != null)
                        {
                            //
                            //clubActivity.Status = ClubActivityStatus.Canceling;
                            await _competitionActivityRepo.Update();
                            return true;
                        }
                        else
                        {
                            throw new ArgumentException("Club Activity not found to update");
                        }
                    }//end check role Leader
                    else
                    {
                        throw new UnauthorizedAccessException("You do not a role Leader to delete this Club Activity");
                    }
                }//end member
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


        //Get-ClubActivity-By-Id
        public async Task<ViewCompetitionActivity> GetByClubActivityId(int id)
        {
            CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(id);
            //
            if (competitionActivity != null)
            {
                return TransformView(competitionActivity);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //Insert
        public async Task<ViewCompetitionActivity> Insert(CompetitionActivityInsertModel model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);

                if (string.IsNullOrEmpty(model.Name)
                    || model.TermId == 0
                    || model.ClubId == 0
                    || model.SeedsPoint < 0
                    || string.IsNullOrEmpty(model.Description)
                    || model.Beginning == DateTime.Parse("1/1/0001 12:00:00 AM")
                    || model.Ending == DateTime.Parse("1/1/0001 12:00:00 AM"))
                    throw new ArgumentNullException("Name Null || ClubId Null || SeedsPoint Null || Description Null || Beginning Null " +
                                                     " Ending Null || TermId Null");

                bool roleLeader = false;
                bool checkDate = CheckDate(model.Beginning, model.Ending, false);
                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = new ViewClubMember();//await _clubHistoryRepo.GetMemberInCLub(conditions);
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
                        if (checkDate)
                        {
                            //
                            CompetitionActivity competitionActivity = new CompetitionActivity();
                            //When Member Take Activity will +1
                            competitionActivity.NumOfMember = 0;
                            competitionActivity.Description = model.Description;
                            competitionActivity.Name = model.Name;
                            competitionActivity.SeedsPoint = model.SeedsPoint;
                            //LocalTime
                            competitionActivity.CreateTime = new LocalTime().GetLocalTime().DateTime;
                            competitionActivity.Ending = model.Ending;
                            //Check Status
                            //clubActivity.Status = ClubActivityStatus.Open;
                            //Check Code
                            competitionActivity.SeedsCode = await checkExistCode();

                            int result = await _competitionActivityRepo.Insert(competitionActivity);
                            if (result > 0)
                            {
                                CompetitionActivity ca = await _competitionActivityRepo.Get(result);
                                ViewCompetitionActivity viewClubActivity = TransformView(ca);
                                return viewClubActivity;
                            }
                            else
                            {
                                return null;
                            }
                        }//end check date
                        else
                        {
                            throw new ArgumentException("Date not suitable");
                        }
                    }//end role leader
                    else
                    {
                        throw new UnauthorizedAccessException("You do not a role Leader to insert this Club Activity");
                    }
                }
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


        //transform View Model
        public ViewCompetitionActivity TransformView(CompetitionActivity competitionActivity)
        {
            return new ViewCompetitionActivity()
            {
                Ending = competitionActivity.Ending,
                CreateTime = competitionActivity.CreateTime,
                Description = competitionActivity.Description,
                Id = competitionActivity.Id,
                Name = competitionActivity.Name,
                NumOfMember = competitionActivity.NumOfMember,
                SeedsCode = competitionActivity.SeedsCode,
                SeedsPoint = competitionActivity.SeedsPoint,
                //Status = clubActivity.Status
            };
        }





        //Update
        public async Task<bool> Update(CompetitionActivityUpdateModel model, string token)
        {
            try
            {

                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                int UserId = Int32.Parse(UserIdClaim.Value);


                bool roleLeader = false;
                bool checkDateUpdate = false;

                GetMemberInClubModel conditions = new GetMemberInClubModel()
                {
                    UserId = UserId,
                    ClubId = model.ClubId,
                    TermId = model.TermId
                };
                ViewClubMember infoClubMem = new ViewClubMember();// await _clubHistoryRepo.GetMemberInCLub(conditions);
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
                        //get club Activity
                        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.Id);
                        if (competitionActivity != null)
                        {
                            //------------ Check date update
                            //th1
                            if (model.Beginning.HasValue && !model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, competitionActivity.Ending, true);
                            }
                            //th2
                            if (model.Beginning.HasValue && model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, (DateTime)model.Ending, true);
                            }
                            if (checkDateUpdate)
                            {
                                //update name-des-seedpoint-beginning-ending
                                competitionActivity.Name = (model.Name.Length > 0) ? model.Name : competitionActivity.Name;
                                competitionActivity.Description = (model.Description.Length > 0) ? model.Description : competitionActivity.Description;
                                competitionActivity.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : competitionActivity.SeedsPoint;
                                competitionActivity.Ending = (DateTime)((model.Ending.HasValue) ? model.Ending : competitionActivity.Ending);

                                //Update DEADLINE day of member takes activity
                                await _memberTakesActivityRepo.UpdateDeadlineDate(competitionActivity.Id, competitionActivity.Ending);
                                await _competitionActivityRepo.Update();
                                return true;

                            }
                            else
                            {
                                throw new ArgumentException("Date not suitable");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Club Activity not found to update");
                        }

                    }
                    else
                    {
                        throw new UnauthorizedAccessException("You do not a role Leader to update this Club Activity");
                    }
                }
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

        //generate Seed code length 8
        private string generateSeedCode()
        {
            string codePool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[8];
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
                check = await _competitionActivityRepo.CheckExistCode(generateCode);
                seedCode = generateCode;
            }
            return seedCode;
        }



        //Check date Insert
        // LCT < ST < ET
        private bool CheckDate(DateTime Beginning, DateTime Ending, bool Update)
        {
            bool round1 = false;
            bool result = false;
            DateTime localTime = new LocalTime().GetLocalTime().DateTime;

            //Use when API UPDATE
            if (Update)
            {
                round1 = true;
            }
            else
            //Use for INSERT
            {
                //LcTime < StartTime , EndTime
                int rs1 = DateTime.Compare(localTime, Beginning);
                if (rs1 < 0)
                {
                    int rs2 = DateTime.Compare(localTime, Ending);
                    if (rs2 < 0)
                    {
                        round1 = true;
                    }
                }
            }

            //ST < ET
            if (round1)
            {
                int rs3 = DateTime.Compare(Beginning, Ending);
                if (rs3 < 0)
                {
                    result = true;
                }
            }

            return result;
        }


        ////Check Update 
        ////th1 Check Beginnin
        //private bool CheckDateBeginning()


        //Get-List-Club-Activities-By-Conditions
        //lấy tất cả các task của 1 trường - 1 câu lạc bộ - seed point - Number of member
        public async Task<PagingResult<ViewCompetitionActivity>> GetListClubActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {
            //
            PagingResult<ViewCompetitionActivity> result = await _competitionActivityRepo.GetListClubActivitiesByConditions(conditions);
            //
            return result;
        }


        //Get Process + Top 4
        public async Task<List<ViewProcessCompetitionActivity>> GetTop4_Process(int clubId, string token)
        {
            //
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var UniIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int UniId = Int32.Parse(UniIdClaim.Value);

            List<ViewProcessCompetitionActivity> viewProcessClubActivities = new List<ViewProcessCompetitionActivity>();

            //check clubId in the system
            Club club = await _clubRepo.Get(clubId);
            if (club != null)
            {
                //check club belong to university id
                if (club.UniversityId == UniId)
                {
                    //top 4 activity by ClubId
                    List<ViewCompetitionActivity> ListViewClubActivity = await _competitionActivityRepo.GetClubActivitiesByCreateTime(UniId, clubId);

                    foreach (ViewCompetitionActivity viewClubActivity in ListViewClubActivity)
                    {
                        //Get Process
                        //get total num of member join
                        int NumberOfMemberJoin = await _memberTakesActivityRepo.GetNumOfMemInTask(viewClubActivity.Id);
                        //get number of member doing task
                        int NumMemberDoingTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Doing);
                        //get number of member submit on time task
                        int NumMemberDoneTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Finished);
                        //get number of member submit on late task
                        int NumMemberDoneLateTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.FinishedLate);
                        //get number of member late task
                        int NumMemberLateTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.LateTime);
                        //get number of member out task
                        //int NumMemberOutTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Approved);

                        ViewProcessCompetitionActivity vpca = new ViewProcessCompetitionActivity()
                        {
                            Ending = viewClubActivity.Ending,
                            CreateTime = viewClubActivity.CreateTime,
                            Description = viewClubActivity.Description,
                            Id = viewClubActivity.Id,
                            Name = viewClubActivity.Name,
                            NumOfMember = viewClubActivity.NumOfMember,
                            SeedsCode = viewClubActivity.SeedsCode,
                            SeedsPoint = viewClubActivity.SeedsPoint,
                            Status = viewClubActivity.Status,
                            NumOfMemberJoin = NumberOfMemberJoin,
                            NumMemberDoingTask = NumMemberDoingTask,
                            NumMemberDoneTask = NumMemberDoneTask,
                            NumMemberDoneLateTask = NumMemberDoneLateTask,
                            NumMemberLateTask = NumMemberLateTask,
                            //NumMemberOutTask = NumMemberOutTask
                        };
                        viewProcessClubActivities.Add(vpca);
                    }
                    return (viewProcessClubActivities.Count > 0) ? viewProcessClubActivities : throw new NullReferenceException();
                }//end check club in Uni
                else
                {
                    throw new ArgumentException("Club not in University");
                }
            }//end check club
            else
            {
                throw new ArgumentException("Club not found ");
            }
        }
    }
}
