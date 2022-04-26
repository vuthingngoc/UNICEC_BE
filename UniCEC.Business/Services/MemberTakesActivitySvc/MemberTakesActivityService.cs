using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
{
    public class MemberTakesActivityService : IMemberTakesActivityService
    {
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        //lấy end date of Club Activity
        private IClubActivityRepo _clubActivityRepo;

        public MemberTakesActivityService(IMemberTakesActivityRepo memberTakesActivityRepo, IClubActivityRepo clubActivityRepo)
        {
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubActivityRepo = clubActivityRepo;
        }

        //transfer View Model
        private ViewMemberTakesActivity TransferViewModel(MemberTakesActivity mta)
        {
            return new ViewMemberTakesActivity()
            {
                Id = mta.Id,
                ClubActivityId = mta.ClubActivityId,
                MemberId = mta.MemberId,
                StartTime = mta.StartTime,
                EndTime = mta.EndTime,
                Deadline = mta.Deadline,
                Status = mta.Status
            };

        }



        public Task<PagingResult<ViewMemberTakesActivity>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewMemberTakesActivity> GetByMemberTakesActivityId(int id)
        {
            //
            MemberTakesActivity mta = await _memberTakesActivityRepo.Get(id);
            //
            if (mta != null)
            {
                return TransferViewModel(mta);
            }
            else
            {
                return null;
            }

        }

        public async Task<ViewMemberTakesActivity> Insert(MemberTakesActivityInsertModel model)
        {

            try
            {
                //
                DateTime DefaultEndTime = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                ClubActivity clubActivity = await _clubActivityRepo.Get(model.ClubActivityId);
                DateTime deadline = clubActivity.Ending;
                //------------------------------------check
                int clubId = clubActivity.ClubId;
                //year
                string year = clubActivity.Beginning.Year.ToString();

                bool MemInClub = await _memberTakesActivityRepo.CheckMemberInClub(clubId, model.MemberId, year);

                if (MemInClub)
                {
                    MemberTakesActivity mta = new MemberTakesActivity()
                    {
                        //club activity id
                        ClubActivityId = model.ClubActivityId,
                        //member id
                        MemberId = model.MemberId,
                        //join mean now
                        StartTime = DateTime.Now,
                        //Submit time chưa có nên cho mặc định 
                        EndTime = DefaultEndTime,
                        //end date of club activity
                        Deadline = deadline,
                        //default status is Doing
                        Status = Data.Enum.MemberTakesActivityStatus.Doing,
                    };

                    int result = await _memberTakesActivityRepo.Insert(mta);
                    if (result > 0)
                    {
                        //
                        MemberTakesActivity m = await _memberTakesActivityRepo.Get(result);
                        return TransferViewModel(m);
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


        //update-task
        public async Task<bool> Update(int id)
        {
            try
            {
                MemberTakesActivity mta = await _memberTakesActivityRepo.Get(id);
                if (mta != null)
                {
                    //check date
                    int result = DateTime.Compare(DateTime.Now,mta.Deadline);
                    //1. earlier 
                    if (result < 0)
                    {
                        //date end time
                        mta.EndTime = DateTime.Now;
                        //
                        mta.Status = Data.Enum.MemberTakesActivityStatus.DoneSoon;
                    }
                    //2. on time
                    if (result == 0)
                    {
                        //date end time
                        mta.EndTime = DateTime.Now;
                        //
                        mta.Status = Data.Enum.MemberTakesActivityStatus.DoneOnTime;
                    }
                    //3. late
                    if (result > 0)
                    {
                        //date end time
                        mta.EndTime = DateTime.Now;
                        //
                        mta.Status = Data.Enum.MemberTakesActivityStatus.Late;
                    }
                    await _memberTakesActivityRepo.Update();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get-All-Taskes-By-Conditions
        public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTaskesByConditions(MemberTakesActivityRequestModel request)
        {
            PagingResult<ViewMemberTakesActivity> result = await _memberTakesActivityRepo.GetAllTaskesByConditions(request);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
