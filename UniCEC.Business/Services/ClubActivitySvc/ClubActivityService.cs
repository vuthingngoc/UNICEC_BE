using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
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

        public ClubActivityService(IClubActivityRepo clubActivityRepo, IMemberTakesActivityRepo memberTakesActivityRepo, IClubHistoryRepo clubHistoryRepo)
        {
            _clubActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubHistoryRepo = clubHistoryRepo;
        }

        //Delete-Club-Activity-By-Id
        public async Task<bool> Delete(int id)
        {
            try
            {
                ClubActivity clubActivity = await _clubActivityRepo.Get(id);
                if (clubActivity != null)
                {
                    //
                    clubActivity.Status = ClubActivityStatus.Canceling;
                    await _clubActivityRepo.Update();
                    return true;
                }
                return false;
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
                return null;
            }
        }

        //Insert
        public async Task<ViewClubActivity> Insert(ClubActivityInsertModel model)
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
                    //------------ Check Role Member Is Leader 
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }
                }
                if (roleLeader)
                {
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
                    clubActivity.Status = GetClubActivityStatus(model.Beginning);
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
        public async Task<bool> Update(ClubActivityUpdateModel model)
        {
            try
            {
                bool check = false;
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
                    //------------ Check Role Member Is Leader 
                    if (infoClubMem.ClubRoleName.Equals("Leader"))
                    {
                        roleLeader = true;
                    }
                }
                if (roleLeader)
                {
                    //get club Activity
                    ClubActivity clubActivity = await _clubActivityRepo.Get(model.Id);

                    if (clubActivity != null)
                    {
                        //update name-des-seedpoint-beginning-ending
                        clubActivity.Name = (model.Name.Length > 0) ? model.Name : clubActivity.Name;
                        clubActivity.Description = (model.Description.Length > 0) ? model.Description : clubActivity.Description;
                        clubActivity.SeedsPoint = (model.SeedsPoint != 0) ? model.SeedsPoint : clubActivity.SeedsPoint;
                        clubActivity.Beginning = (DateTime)((model.Beginning.HasValue) ? model.Beginning : clubActivity.Beginning);
                        clubActivity.Ending = (DateTime)((model.Ending.HasValue) ? model.Ending : clubActivity.Ending);
                        //clubActivity.NumOfMember = (model.NumOfMember != 0) ? model.NumOfMember : clubActivity.NumOfMember;
                        //clubActivity.Status = (ClubActivityStatus)((model.Status.HasValue) ? model.Status : clubActivity.Status);

                        await _clubActivityRepo.Update();
                        return true;
                    }
                    return check;
                }
                else
                {
                    return check;
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

        //check status by date when insert
        private ClubActivityStatus GetClubActivityStatus(DateTime BeginingTime)
        {
            DateTime now = DateTime.Now;
            int result = DateTime.Compare(now, BeginingTime);
            //Earlier
            if (result < 0)
            {
                return ClubActivityStatus.HappenningSoon;
            }
            if (result > 0)
            {
                return ClubActivityStatus.Happenning;
            }
            if (result == 0)
            {
                return ClubActivityStatus.Happenning;
            }
            return ClubActivityStatus.Error;
        }



        //Get-List-Club-Activities-By-Conditions
        //lấy tất cả các task của 1 trường - 1 câu lạc bộ - seed point - Number of member
        public async Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions)
        {
            //
            PagingResult<ViewClubActivity> result = await _clubActivityRepo.GetListClubActivitiesByConditions(conditions);
            //
            return result;
        }

        ////Get Top 4 Club Activities depend on create time
        //public async Task<List<ViewClubActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId)
        //{
        //    //
        //    List<ViewClubActivity> result = await _clubActivityRepo.GetClubActivitiesByCreateTime(universityId, clubId);
        //    //
        //    return result;
        //}

        //Get Process Club Activity
        //public async Task<ViewProcessClubActivity> GetProcessClubActivity(int clubActivityId, MemberTakesActivityStatus status)
        //{
        //    ClubActivity ca = await _clubActivityRepo.Get(clubActivityId);
        //    if (ca != null)
        //    {
        //        //get total num of member join
        //        int NumberOfMemberJoin = await _memberTakesActivityRepo.GetNumOfMemInTask(clubActivityId);
        //        //get number of member done task
        //        int NumberOfeMemberJoin_Status = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(clubActivityId, status);
        //        //
        //        ViewClubActivity viewClubActivity = TransformView(ca);

        //        return new ViewProcessClubActivity()
        //        {
        //            ClubId = viewClubActivity.ClubId,
        //            Beginning = viewClubActivity.Beginning,
        //            Ending = viewClubActivity.Ending,
        //            CreateTime = viewClubActivity.CreateTime,
        //            Description = viewClubActivity.Description,
        //            Id = viewClubActivity.Id,
        //            Name = viewClubActivity.Name,
        //            NumOfMember = viewClubActivity.NumOfMember,
        //            SeedsCode = viewClubActivity.SeedsCode,
        //            SeedsPoint = viewClubActivity.SeedsPoint,
        //            Status = viewClubActivity.Status,
        //            NumOfMemberJoin = NumberOfMemberJoin,
        //            NumMemberDoingTask = NumberOfeMemberJoin_Status,
        //        };
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //Get Process + Top 4
        public async Task<List<ViewProcessClubActivity>> GetTop4_Process(int universityId, int clubId)
        {
            //
            List<ViewProcessClubActivity> viewProcessClubActivities = new List<ViewProcessClubActivity>();
            //top 4 activity by ClubId
            List<ViewClubActivity> ListViewClubActivity = await _clubActivityRepo.GetClubActivitiesByCreateTime(universityId, clubId);

            foreach (ViewClubActivity viewClubActivity in ListViewClubActivity)
            {
                //Get Process
                //get total num of member join
                int NumberOfMemberJoin = await _memberTakesActivityRepo.GetNumOfMemInTask(viewClubActivity.Id);
                //get number of member doing task
                int NumMemberDoingTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Doing);
                //get number of member done task
                int NumMemberDoneTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.DoneOnTime);
                //get number of member late task
                int NumMemberLateTask = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(viewClubActivity.Id, MemberTakesActivityStatus.Late);

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
                    NumMemberLateTask = NumMemberLateTask,
                };
                viewProcessClubActivities.Add(vpca);
            }
            return (viewProcessClubActivities.Count > 0) ? viewProcessClubActivities : null;    

        }
    }
}
