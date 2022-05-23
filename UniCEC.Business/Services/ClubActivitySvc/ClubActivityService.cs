using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public class ClubActivityService : IClubActivityService
    {
        private IClubActivityRepo _clubActivityRepo;
        //
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        //check Infomation Member -> is Leader
        private IClubHistoryRepo _clubHistoryRepo;
        private IClubRepo _clubRepo;

        public ClubActivityService(IClubActivityRepo clubActivityRepo, IMemberTakesActivityRepo memberTakesActivityRepo, IClubHistoryRepo clubHistoryRepo, IClubRepo clubRepo)
        {
            _clubActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubHistoryRepo = clubHistoryRepo;
            _clubRepo = clubRepo;
        }

        //Delete-Club-Activity-By-Id
        public async Task<bool> Delete(ClubActivityDeleteModel model, string token)
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
                        ClubActivity clubActivity = await _clubActivityRepo.Get(model.ClubActivityId);
                        if (clubActivity != null)
                        {
                            //
                            clubActivity.Status = ClubActivityStatus.Canceling;
                            await _clubActivityRepo.Update();
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
        public async Task<ViewClubActivity> GetByClubActivityId(int id)
        {
            ClubActivity clubActivity = await _clubActivityRepo.Get(id);
            //
            if (clubActivity != null)
            {
                return TransformView(clubActivity);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //Insert
        public async Task<ViewClubActivity> Insert(ClubActivityInsertModel model, string token)
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
                        if (checkDate)
                        {
                            //
                            ClubActivity clubActivity = new ClubActivity();
                            //
                            clubActivity.ClubId = model.ClubId;
                            //When Member Take Activity will +1
                            clubActivity.NumOfMember = 0;
                            clubActivity.Description = model.Description;
                            clubActivity.Name = model.Name;
                            clubActivity.SeedsPoint = model.SeedsPoint;
                            //LocalTime
                            clubActivity.CreateTime = new LocalTime().GetLocalTime().DateTime;
                            //
                            clubActivity.Beginning = model.Beginning;
                            clubActivity.Ending = model.Ending;
                            //Check Status
                            clubActivity.Status = ClubActivityStatus.Open;
                            //Check Code
                            clubActivity.SeedsCode = await checkExistCode();

                            int result = await _clubActivityRepo.Insert(clubActivity);
                            if (result > 0)
                            {
                                ClubActivity ca = await _clubActivityRepo.Get(result);
                                ViewClubActivity viewClubActivity = TransformView(ca);
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
        public ViewClubActivity TransformView(ClubActivity clubActivity)
        {
            return new ViewClubActivity()
            {
                ClubId = clubActivity.ClubId,
                Beginning = clubActivity.Beginning,
                Ending = clubActivity.Ending,
                CreateTime = clubActivity.CreateTime,
                Description = clubActivity.Description,
                Id = clubActivity.Id,
                Name = clubActivity.Name,
                NumOfMember = clubActivity.NumOfMember,
                SeedsCode = clubActivity.SeedsCode,
                SeedsPoint = clubActivity.SeedsPoint,
                Status = clubActivity.Status
            };
        }





        //Update
        public async Task<bool> Update(ClubActivityUpdateModel model, string token)
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
                        //get club Activity
                        ClubActivity clubActivity = await _clubActivityRepo.Get(model.Id);
                        if (clubActivity != null)
                        {
                            //------------ Check date update
                            //th1
                            if (model.Beginning.HasValue && !model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, clubActivity.Ending, true);
                            }
                            //th2
                            if (model.Ending.HasValue && !model.Beginning.HasValue)
                            {
                                checkDateUpdate = CheckDate(clubActivity.Beginning, (DateTime)model.Ending, true);
                            }
                            //th3
                            if (model.Beginning.HasValue && model.Ending.HasValue)
                            {
                                checkDateUpdate = CheckDate((DateTime)model.Beginning, (DateTime)model.Ending, true);
                            }
                            if (checkDateUpdate)
                            {
                                //update name-des-seedpoint-beginning-ending
                                clubActivity.Name = (model.Name.Length > 0) ? model.Name : clubActivity.Name;
                                clubActivity.Description = (model.Description.Length > 0) ? model.Description : clubActivity.Description;
                                clubActivity.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : clubActivity.SeedsPoint;
                                clubActivity.Beginning = (DateTime)((model.Beginning.HasValue) ? model.Beginning : clubActivity.Beginning);
                                clubActivity.Ending = (DateTime)((model.Ending.HasValue) ? model.Ending : clubActivity.Ending);

                                //Update DEADLINE day of member takes activity
                                await _memberTakesActivityRepo.UpdateDeadlineDate(clubActivity.Id, clubActivity.Ending);
                                await _clubActivityRepo.Update();
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
                check = await _clubActivityRepo.CheckExistCode(generateCode);
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
        public async Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions)
        {
            //
            PagingResult<ViewClubActivity> result = await _clubActivityRepo.GetListClubActivitiesByConditions(conditions);
            //
            return result;
        }


        //Get Process + Top 4
        public async Task<List<ViewProcessClubActivity>> GetTop4_Process(int clubId, string token)
        {
            //
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var UniIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));
            int UniId = Int32.Parse(UniIdClaim.Value);

            List<ViewProcessClubActivity> viewProcessClubActivities = new List<ViewProcessClubActivity>();

            //check clubId in the system
            Club club = await _clubRepo.Get(clubId);
            if (club != null)
            {
                //check club belong to university id
                if (club.UniversityId == UniId)
                {
                    //top 4 activity by ClubId
                    List<ViewClubActivity> ListViewClubActivity = await _clubActivityRepo.GetClubActivitiesByCreateTime(UniId, clubId);

                    foreach (ViewClubActivity viewClubActivity in ListViewClubActivity)
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

                        ViewProcessClubActivity vpca = new ViewProcessClubActivity()
                        {
                            ClubId = viewClubActivity.ClubId,
                            Beginning = viewClubActivity.Beginning,
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
