using System;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public class ClubActivityService : IClubActivityService
    {
        private IClubActivityRepo _clubActivityRepo;

        public ClubActivityService(IClubActivityRepo clubActivityRepo)
        {
            _clubActivityRepo = clubActivityRepo;
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
                    clubActivity.Status = ClubActivityStatus.Inactive;
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
                return transferViewModel(clubActivity);
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
                clubActivity.Status = model.Status;
                clubActivity.Description = model.Description;
                clubActivity.Name = model.Name;
                clubActivity.SeedsPoint = model.SeedsPoint;
                clubActivity.CreateTime = DateTime.Now;
                clubActivity.Beginning = model.Beginning;
                clubActivity.Ending = model.Ending;
                clubActivity.SeedsCode = await checkExistCode();
                //

                int result = await _clubActivityRepo.Insert(clubActivity);
                if (result > 0)
                {
                    ClubActivity ca = await _clubActivityRepo.Get(result);
                    ViewClubActivity viewClubActivity = transferViewModel(ca);
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
        public ViewClubActivity transferViewModel(ClubActivity clubActivity)
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

        //Get-List-Club-Activities-By-Conditions
        public async Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions)
        {
            //
            PagingResult<ViewClubActivity> result = await _clubActivityRepo.GetListClubActivitiesByConditions(conditions);
            //
            return result;
        }
    }
}
