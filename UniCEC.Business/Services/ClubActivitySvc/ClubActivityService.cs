using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public class ClubActivityService : IClubActivityService
    {
        private IClubActivityRepo _clubActivityRepo;
        //
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        public ClubActivityService(IClubActivityRepo clubActivityRepo, IMemberTakesActivityRepo memberTakesActivityRepo)
        {
            _clubActivityRepo = clubActivityRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
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
                    clubActivity.Status = ClubActivityStatus.Canceled;
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
                ClubActivity clubActivity = new ClubActivity();
                //
                clubActivity.ClubId = model.ClubId;
                clubActivity.NumOfMember = model.NumOfMember;
                clubActivity.Description = model.Description;
                clubActivity.Name = model.Name;
                clubActivity.SeedsPoint = model.SeedsPoint;
                clubActivity.CreateTime = DateTime.Now;
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
                Id = clubActivity.ClubId,
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
                //get club Activity
                ClubActivity clubActivity = await _clubActivityRepo.Get(model.Id);
                bool check = false;
                if (clubActivity != null)
                {
                    //update name-des-seedpoint-beginning-ending-numOfmem-status
                    clubActivity.Name = (!model.Name.Equals("")) ? model.Name : clubActivity.Name;
                    clubActivity.Description = (!model.Description.Equals("")) ? model.Description : clubActivity.Description;
                    clubActivity.SeedsPoint = (!model.SeedsPoint.Equals("")) ? model.SeedsPoint : clubActivity.SeedsPoint;
                    clubActivity.Beginning = model.Beginning;
                    clubActivity.Ending = model.Ending;
                    clubActivity.NumOfMember = (!model.NumOfMember.ToString().Equals("")) ? model.NumOfMember : clubActivity.NumOfMember;
                    clubActivity.Status = (!model.Status.ToString().Equals("")) ? model.Status : clubActivity.Status;
                    await _clubActivityRepo.Update();
                    return true;
                }
                return check;
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

        //Get Top 4 Club Activities depend on create time
        public async Task<List<ViewClubActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId)
        {
            //
            List<ViewClubActivity> result = await _clubActivityRepo.GetClubActivitiesByCreateTime(universityId, clubId);
            //
            return result;
        }

        //Get Process Club Activity
        public async Task<ViewProcessClubActivity> GetProcessClubActivity(int clubActivityId, MemberTakesActivityStatus status)
        {
            ClubActivity ca = await _clubActivityRepo.Get(clubActivityId);
            if (ca != null)
            {
                //get total num of member join
                int NumberOfMemberJoin = await _memberTakesActivityRepo.GetNumOfMemInTask(clubActivityId);
                //get number of member done task
                int NumberOfeMemberJoin_Status = await _memberTakesActivityRepo.GetNumOfMemInTask_Status(clubActivityId, status);
                //
                ViewClubActivity viewClubActivity = TransformView(ca);


                return new ViewProcessClubActivity()
                {
                    ClubId = viewClubActivity.ClubId,
                    Beginning = viewClubActivity.Beginning,
                    Ending = viewClubActivity.Ending,
                    CreateTime = viewClubActivity.CreateTime,
                    Description = viewClubActivity.Description,
                    Id = viewClubActivity.ClubId,
                    Name = viewClubActivity.Name,
                    NumOfMember = viewClubActivity.NumOfMember,
                    SeedsCode = viewClubActivity.SeedsCode,
                    SeedsPoint = viewClubActivity.SeedsPoint,
                    Status = viewClubActivity.Status,
                    NumOfMemberJoin = NumberOfMemberJoin,
                    NumOfMemberTakesTaskStatus = NumberOfeMemberJoin_Status,
                };
            }
            else
            {
                return null;
            }
        }
    }
}
