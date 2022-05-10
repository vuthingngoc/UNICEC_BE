using System;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
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

        //check mem in club
        private IClubHistoryRepo _clubHistoryRepo;

        public MemberTakesActivityService(IMemberTakesActivityRepo memberTakesActivityRepo, IClubActivityRepo clubActivityRepo, IClubHistoryRepo clubHistoryRepo)
        {
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _clubActivityRepo = clubActivityRepo;
            _clubHistoryRepo = clubHistoryRepo;
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
                //------------------------------------check mem in club
                int clubId = clubActivity.ClubId;

                bool MemInClub_Term = await _clubHistoryRepo.CheckMemberInClub(clubId, model.MemberId, model.TermId);

                //------------------------------------Add number of member join this clubActivity     
                bool NumOfMem_InTask = false;
                //get club Activity
                ClubActivity ca = await _clubActivityRepo.Get(model.ClubActivityId);
                if (ca != null)
                {
                    ca.NumOfMember = ca.NumOfMember + 1;
                    await _clubActivityRepo.Update();
                    NumOfMem_InTask = true;
                }
                if (MemInClub_Term && NumOfMem_InTask)
                {
                    //------------------------------------Check mem takes club activity
                    bool MemTakesTask = await _memberTakesActivityRepo.CheckMemberTakesTask(model.ClubActivityId, model.MemberId);
                    if (MemTakesTask)
                    {
                        //------------------------------------Check Club Activity is Happenning Status can add                       
                        bool checkStatusClubActivity = await CheckStatusClubActivity(model.ClubActivityId);
                        if (checkStatusClubActivity)
                        {

                            //LocalTime
                            DateTimeOffset localTime = new LocalTime().GetLocalTime();

                            MemberTakesActivity mta = new MemberTakesActivity()
                            {
                                //club activity id
                                ClubActivityId = model.ClubActivityId,
                                //member id
                                MemberId = model.MemberId,
                                //join mean now
                                StartTime = localTime.DateTime,
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
                        } // end if checkStatusClubActivity
                        else
                        {
                            return null;
                        }
                    }// end if MemTakesTask
                    else
                    {
                        return null;
                    }
                }// end if MemInClub_Term && NumOfMem_InTask
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
                    //LocalTime
                    DateTimeOffset localTime = new LocalTime().GetLocalTime();
                    //
                    int result = DateTime.Compare(localTime.DateTime, mta.Deadline);
                    //1. earlier 
                    if (result < 0)
                    {
                        //date end time
                        mta.EndTime = DateTime.Now;
                        //
                        mta.Status = Data.Enum.MemberTakesActivityStatus.DoneOnTime;
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

        //Check Status Club Activity
        private async Task<bool> CheckStatusClubActivity(int clubActivityId)
        {
            try
            {
                //
                ClubActivity ca = await _clubActivityRepo.Get(clubActivityId);
                //
                if (ca != null)
                {
                    if (ca.Status == ClubActivityStatus.Happenning)
                    {
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
    }
}
