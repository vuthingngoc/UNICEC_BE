using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
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
        private ICompetitionActivityRepo _competitionActivityRepo;

        

        public MemberTakesActivityService(IMemberTakesActivityRepo memberTakesActivityRepo, ICompetitionActivityRepo clubActivityRepo)
        {
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _competitionActivityRepo = clubActivityRepo;     
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
                //Status = mta.Status
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
                throw new NullReferenceException();
            }

        }

        public async Task<ViewMemberTakesActivity> Insert(MemberTakesActivityInsertModel model, string token)
        {
            try
            {
                if (model.MemberId == 0
                    || model.ClubActivityId == 0
                    || model.TermId == 0)
                    throw new ArgumentNullException("MemberId Null || ClubActivityId Null || TermId Null");

                //
                DateTime DefaultEndTime = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                CompetitionActivity clubActivity = await _competitionActivityRepo.Get(model.ClubActivityId);
                DateTime deadline = clubActivity.Ending;
                //------------------------------------check mem in club
                //int clubId = clubActivity.ClubId;
                //bool MemInClub_Term = await _clubHistoryRepo.CheckMemberInClub(clubId, model.MemberId, model.TermId);

                //------------------------------------Add number of member join this clubActivity     
                bool NumOfMem_InTask = false;
                if (clubActivity != null)
                {
                    clubActivity.NumOfMember = clubActivity.NumOfMember + 1;
                    await _competitionActivityRepo.Update();
                    NumOfMem_InTask = true;
                }
                //if (MemInClub_Term && NumOfMem_InTask)
                //{
                //    //------------------------------------Check mem takes club activity
                //    bool MemTakesTask = await _memberTakesActivityRepo.CheckMemberTakesTask(model.ClubActivityId, model.MemberId);
                //    //true  -> created
                //    //false -> can insert
                //    if (MemTakesTask == false)
                //    {
                //        //------------------------------------Check Club Activity is Happenning Status can add                       
                //        bool checkStatusClubActivity = await CheckStatusClubActivity(model.ClubActivityId);
                //        if (checkStatusClubActivity)
                //        {
                //            //LocalTime
                //            DateTimeOffset localTime = new LocalTime().GetLocalTime();

                //            MemberTakesActivity mta = new MemberTakesActivity()
                //            {
                //                //club activity id
                //                ClubActivityId = model.ClubActivityId,
                //                //member id
                //                MemberId = model.MemberId,
                //                //join mean now
                //                StartTime = localTime.DateTime,
                //                //Submit time chưa có nên cho mặc định 
                //                EndTime = DefaultEndTime,
                //                //end date of club activity
                //                Deadline = deadline,
                //                //default status is Doing
                //                Status = Data.Enum.MemberTakesActivityStatus.Doing,
                //            };

                //            int result = await _memberTakesActivityRepo.Insert(mta);
                //            if (result > 0)
                //            {
                //                //
                //                MemberTakesActivity m = await _memberTakesActivityRepo.Get(result);
                //                return TransferViewModel(m);
                //            }
                //            else
                //            {
                //                throw new ArgumentException("Eror when insert");
                //            }
                //        } // end if checkStatusClubActivity
                //        else
                //        {
                //            throw new ArgumentException("This task is Ending");
                //        }
                //    }// end if MemTakesTask
                //    else
                //    {
                //        throw new ArgumentException("You are already in this task");
                //    }
                //}// end if MemInClub_Term && NumOfMem_InTask
                //else
                //{
                //    throw new UnauthorizedAccessException("You aren't member in Club");
                //}
                return new ViewMemberTakesActivity();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //update-task
        public async Task<bool> Update(SubmitMemberTakesActivity model, string token)
        {
            try
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));
                var UniversityIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("UniversityId"));

                int UserId = Int32.Parse(UserIdClaim.Value);
                int UniversityId = Int32.Parse(UniversityIdClaim.Value);

                //CHECK Member-Take-Activity ID can't null
                if (model.MemberTakesActivityId == 0) throw new ArgumentNullException("Member Take Activity can't Null !");

                //CHECK Task belong to this user
                if (await _memberTakesActivityRepo.CheckTaskBelongToStudent(model.MemberTakesActivityId, UserId, UniversityId))
                {
                    MemberTakesActivity mta = await _memberTakesActivityRepo.Get(model.MemberTakesActivityId);
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
                            mta.EndTime = localTime.DateTime;
                            //
                            //mta.Status = Data.Enum.MemberTakesActivityStatus.Finished;
                        }
                        //2. on time
                        if (result == 0)
                        {
                            //date end time
                            mta.EndTime = localTime.DateTime;
                            //
                            //mta.Status = Data.Enum.MemberTakesActivityStatus.Finished;
                        }
                        //3. late
                        if (result > 0)
                        {
                            //date end time
                            mta.EndTime = localTime.DateTime;
                            //
                            //mta.Status = Data.Enum.MemberTakesActivityStatus.FinishedLate;
                        }
                        await _memberTakesActivityRepo.Update();
                        return true;
                    }
                }//end check task belong to this user
                else
                {
                    throw new ArgumentException("This task not belong to this Student");
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //role leader can update
        public async Task<bool> ApprovedOrRejectedTask(ConfirmMemberTakesActivity model, string token)
        {
            try
            {
                //var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                //var UserIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type.ToString().Equals("Id"));


                //int UserId = Int32.Parse(UserIdClaim.Value);

                //if (model.MemberTakesActivityId == 0 || model.TermId == 0 || model.MemberId == 0 || ((int)model.Status) < 3)
                //    throw new ArgumentNullException("Member Takes Activity Id can't Null ! || Term Id can't null || Member Id can't Null || Status < 3 can't accept, 4.Approved , 5.Rejected");

                ////check Member Take Activity
                //MemberTakesActivity mta = await _memberTakesActivityRepo.Get(model.MemberTakesActivityId);
                //if (mta != null)
                //{
                //    //check role 
                //    bool role = false;
                //    //------------------------------------check mem 
                //    CompetitionActivity clubActivity = await _competitionActivityRepo.Get(mta.ClubActivityId);
                //    GetMemberInClubModel conditions = new GetMemberInClubModel()
                //    {
                //        UserId = UserId,
                //        TermId = model.TermId
                //    };
                //    //ViewClubMember infoClubMem = await _clubHistoryRepo.GetMemberInCLub(conditions);
                //    ViewClubMember infoClubMem = new ViewClubMember();
                //    //------------ Check Mem in that club
                //    if (infoClubMem != null)
                //    {
                //        //------------ Check Role Member Is Leader, 
                //        if (infoClubMem.ClubRoleName.Equals("Leader"))
                //        {
                //            role = true;
                //        }
                //        if (role)
                //        {
                //            //Approved
                //            if (((int)model.Status) == 4)
                //            {
                //                //mta.Status = MemberTakesActivityStatus.Approved;
                //                await _memberTakesActivityRepo.Update();
                //                return true;
                //            }
                //            //Rejected
                //            if (((int)model.Status) == 5)
                //            {
                //                //mta.Status = MemberTakesActivityStatus.Rejected;
                //                await _memberTakesActivityRepo.Update();
                //                return true;
                //            }
                //            return false;

                //        }//end check role
                //        else
                //        {
                //            throw new ArgumentException("You don't have permision to confirm this task");
                //        }
                //    }//end check mem in club
                //    else
                //    {
                //        throw new UnauthorizedAccessException("You aren't a member in this club");
                //    }
                //}//end check 
                //else
                //{
                //    throw new ArgumentException("Not found this Member Take Activity Id in system");
                //}

                throw new NullReferenceException();
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
                throw new NullReferenceException();
            }
        }

        //Check Status Club Activity Happenning or Open can join
        private async Task<bool> CheckStatusClubActivity(int competitionActivityId)
        {
            try
            {
                //
                CompetitionActivity ca = await _competitionActivityRepo.Get(competitionActivityId);
                //
                if (ca != null)
                {
                    //if (ca.Status == ClubActivityStatus.Happenning || ca.Status == ClubActivityStatus.Open)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    return true;
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
