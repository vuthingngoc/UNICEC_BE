using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
{
    public class MemberTakesActivityService : IMemberTakesActivityService
    {
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        //lấy end date of Club Activity
        private ICompetitionActivityRepo _competitionActivityRepo;

        private ICompetitionRepo _competitionRepo;
        private IClubRepo _clubRepo;
       
        private IMemberRepo _memberRepo;
       
        private IUserRepo _userRepo;
        private IFileService _fileService;
        private DecodeToken _decodeToken;



        public MemberTakesActivityService(IMemberTakesActivityRepo memberTakesActivityRepo,
                                          ICompetitionActivityRepo clubActivityRepo,
                                          ICompetitionRepo competitionRepo,
                                          IClubRepo clubRepo,
                                         
                                          IMemberRepo memberRepo,
                                          IUserRepo userRepo,
                                         
                                          IFileService fileService)
        {
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _competitionActivityRepo = clubActivityRepo;
            _competitionRepo = competitionRepo;
            _clubRepo = clubRepo;
            
            _memberRepo = memberRepo;
            _userRepo = userRepo;
           
            _fileService = fileService;
            _decodeToken = new DecodeToken();
        }

        //Get-All-Taskes-Member-By-Conditions 
        //public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksMemberByConditions(MemberTakesActivityRequestModel request, string token)
        //{
        //    try
        //    {
        //        if (request.CompetitionActivityId == 0 || request.ClubId == 0)
        //            throw new ArgumentNullException("CompetitionActivityId Null || ClubId Null");

        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(request.CompetitionActivityId);
        //        //Check Competition Activity
        //        if (competitionActivity == null) throw new ArgumentException("Competition Activity not found");


        //        // Check Member của club đó 
        //        Club club = await _clubRepo.Get(request.ClubId);
        //        if (club == null) throw new ArgumentException("Club not found in system !");

        //        ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(request.ClubId);
        //        if (CurrentTermOfCLub == null) throw new ArgumentException("Term of ClubId is End");

        //        GetMemberInClubModel conditions = new GetMemberInClubModel()
        //        {
        //            UserId = _decodeToken.Decode(token, "Id"),
        //            ClubId = club.Id,
        //            TermId = CurrentTermOfCLub.Id
        //        };
        //        ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
        //        if (infoClubMem == null) throw new UnauthorizedAccessException("You are not member in Club");

        //        //

        //        PagingResult<ViewMemberTakesActivity> result = await _memberTakesActivityRepo.GetAllTasksMemberByConditions(request, _decodeToken.Decode(token, "Id"));
        //        if (result != null)
        //        {
        //            return result;
        //        }
        //        else
        //        {
        //            throw new NullReferenceException();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        ////Get-All-Taskes-By-Conditions - Competition Manager Role
        //public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksByConditions(MemberTakesActivityRequestModel request, string token)
        //{
        //    try
        //    {
        //        if (request.CompetitionActivityId == 0 || request.ClubId == 0)
        //            throw new ArgumentNullException("CompetitionActivityId Null || ClubId Null");

        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(request.CompetitionActivityId);
        //        //Check Competition Activity
        //        if (competitionActivity == null) throw new ArgumentException("Competition Activity not found");


        //        bool check = await CheckConditions(token, competitionActivity.CompetitionId, request.ClubId);

        //        if (check)
        //        {
        //            PagingResult<ViewMemberTakesActivity> result = await _memberTakesActivityRepo.GetAllTasksByConditions(request);
        //            if (result != null)
        //            {
        //                return result;

        //            }
        //            else
        //            {
        //                throw new NullReferenceException();
        //            }
        //        }
        //        else
        //        {
        //            throw new NullReferenceException();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


        //public async Task<ViewDetailMemberTakesActivity> GetByMemberTakesActivityId(int memberTakeActivityId, int clubId, string token)
        //{
        //    try
        //    {
        //        if (memberTakeActivityId == 0 || clubId == 0)
        //            throw new ArgumentNullException("Member take activity Null || ClubId Null");

        //        //CHECK Task belong to this user
        //        if (await _memberTakesActivityRepo.CheckTaskBelongToStudent(memberTakeActivityId, _decodeToken.Decode(token, "Id"), clubId))
        //        {
        //            MemberTakesActivity mta = await _memberTakesActivityRepo.Get(memberTakeActivityId);
        //            //
        //            if (mta != null)
        //            {
        //                return await TransferViewDetailMTA(mta);
        //            }
        //            else
        //            {
        //                throw new NullReferenceException();
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException("This task not belong to this Student");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //public async Task<ViewDetailMemberTakesActivity> Insert(MemberTakesActivityInsertModel model, string token)
        //{
        //    try
        //    {
        //        if (model.MemberId == 0
        //            || model.CompetitionActivityId == 0
        //            || model.ClubId == 0)
        //            throw new ArgumentNullException("MemberId Null || CompetitionActivityId Null || ClubId Null");

        //        //
        //        //DateTime DefaultEndTime = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        //        CompetitionActivity competitionActivity = await _competitionActivityRepo.Get(model.CompetitionActivityId);
        //        //Check Competition Activity
        //        if (competitionActivity == null) throw new ArgumentException("Competition Activity not found");

        //        //Canceling // Ending
        //        if (competitionActivity.Status == CompetitionActivityStatus.Canceling || competitionActivity.Status == CompetitionActivityStatus.Ending) throw new ArgumentException("Competition Activity is canceling or ending");

        //        //BookerId == userId
        //        bool check = await CheckConditions(token, competitionActivity.CompetitionId, model.ClubId);

        //        if (check)
        //        {
        //            //Check Member Id có status true
        //            Member member = await _memberRepo.Get(model.MemberId);
        //            if (member == null && member.Status == MemberStatus.Active) throw new ArgumentException("Member not found");

        //            //Check member is belong to club
        //            if (member.ClubId != model.ClubId) throw new ArgumentException("Member are not belong to this club");

        //            //Check booker can not assign task for yourself 
        //            int memId = await _memberRepo.GetIdByUser(_decodeToken.Decode(token,"Id"),model.ClubId);
        //            ViewMember vdm = await _memberRepo.GetById(memId);
        //            if (vdm.Id == model.MemberId) throw new ArgumentException("Booker can not assign task for yourself"); 

        //            //Check add the same task for this member 
        //            bool mtaCheck = await _memberTakesActivityRepo.CheckMemberTakesTask(competitionActivity.Id, model.MemberId);
        //            if (mtaCheck) throw new ArgumentException("This member is already take this task");

        //            //Check add in task when task id Status Endtime
        //            if (competitionActivity.Status == CompetitionActivityStatus.Ending) throw new ArgumentException("This task is End can not add member take task");

        //            MemberTakesActivity mtaInsert = new MemberTakesActivity();
        //            mtaInsert.CompetitionActivityId = model.CompetitionActivityId;
        //            mtaInsert.MemberId = model.MemberId;
        //            mtaInsert.UserId = _decodeToken.Decode(token, "Id");
        //            mtaInsert.StartTime = new LocalTime().GetLocalTime().DateTime;
        //            //mtaInsert.EndTime = DefaultEndTime; //-> Null
        //            mtaInsert.Deadline = competitionActivity.Ending;
        //            mtaInsert.Status = MemberTakesActivityStatus.Doing;
        //            int result = await _memberTakesActivityRepo.Insert(mtaInsert);

        //            if (result == 0) throw new ArgumentException("Insert Failed !");

        //            //do mới add thêm nên tiến trình chưa hoàn thành 
        //            competitionActivity.Process = CompetitionActivityProcessStatus.NotComplete;
        //            //+ 1 number Of Member
        //            competitionActivity.NumOfMember = competitionActivity.NumOfMember + 1;
        //            await _competitionActivityRepo.Update();

        //            return await TransferViewDetailMTA(mtaInsert);

        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> RemoveMemberTakeActivity(RemoveMemberTakeActivityModel model, string token)
        //{
        //    try
        //    {
        //        if (model.ClubId == 0) throw new ArgumentNullException("ClubId Null");

        //        MemberTakesActivity mta = await _memberTakesActivityRepo.Get(model.MemberTakesActivityId);

        //        if (mta == null) throw new ArgumentException("This task not found !");

        //        CompetitionActivity ca = await _competitionActivityRepo.Get(mta.CompetitionActivityId);
        //        //Canceling
        //        if (ca.Status == CompetitionActivityStatus.Canceling) throw new ArgumentException("Competition Activity is canceling");

        //        bool check = await CheckConditions(token, ca.CompetitionId, model.ClubId);

        //        if (check)
        //        {
        //            //Update Status của Competition Activity
        //            int numberOfMemberHasSubmit = await _memberTakesActivityRepo.GetNumberOfMemberIsSubmitted(ca.Id);
        //            if (numberOfMemberHasSubmit == ca.NumOfMember)
        //            {
        //                ca.Process = CompetitionActivityProcessStatus.Complete;                   
        //            }
        //            // -1 Number Of Participant
        //            ca.NumOfMember = ca.NumOfMember - 1;
        //            await _competitionActivityRepo.Update();

        //            return await _memberTakesActivityRepo.RemoveMemberTakeTask(model.MemberTakesActivityId);

        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////Submit Task
        //public async Task<bool> SubmitTask(SubmitMemberTakesActivity model, string token)
        //{
        //    try
        //    {
        //        if (model.ClubId == 0 || model.MemberTakesActivityId == 0) throw new ArgumentNullException("ClubId Null || Member Takes Activity Id Null");

        //        //CHECK Task belong to this user
        //        if (await _memberTakesActivityRepo.CheckTaskBelongToStudent(model.MemberTakesActivityId, _decodeToken.Decode(token, "Id"), model.ClubId))
        //        {
        //            MemberTakesActivity mta = await _memberTakesActivityRepo.Get(model.MemberTakesActivityId);
        //            if (mta != null)
        //            {
        //                //Check Competition Activity
        //                CompetitionActivity ca = await _competitionActivityRepo.Get(mta.CompetitionActivityId);
        //                //Canceling
        //                if (ca.Status == CompetitionActivityStatus.Canceling) throw new ArgumentException("Competition Activity is canceling");
        //                //Check Date
        //                //LocalTime
        //                DateTimeOffset localTime = new LocalTime().GetLocalTime();
        //                //
        //                int result = DateTime.Compare(localTime.DateTime, mta.Deadline);
        //                //1. earlier or same
        //                if (result <= 0)
        //                {
        //                    //date end times
        //                    mta.EndTime = localTime.DateTime;
        //                    //
        //                    mta.Status = Data.Enum.MemberTakesActivityStatus.Finished;
        //                }
        //                //2. late
        //                if (result > 0)
        //                {
        //                    //date end time
        //                    mta.EndTime = localTime.DateTime;
        //                    //
        //                    mta.Status = Data.Enum.MemberTakesActivityStatus.FinishedLate;
        //                }
        //                await _memberTakesActivityRepo.Update();
        //                return true;
        //            }
        //        }//end check task belong to this user
        //        else
        //        {
        //            throw new ArgumentException("This task not belong to this Student");
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


        //public async Task<bool> ApprovedOrRejectedTask(ConfirmMemberTakesActivity model, string token)
        //{
        //    try
        //    {

        //        if (model.MemberTakesActivityId == 0 || ((int)model.Status) < 3)
        //            throw new ArgumentNullException("Member Takes Activity Id can't Null ! || Status < 3 can't accept, 4.Approved , 5.Rejected");

        //        //
        //        MemberTakesActivity mta = await _memberTakesActivityRepo.Get(model.MemberTakesActivityId);
        //        if (mta == null) throw new ArgumentException("Not found this Task in System");

        //        //Check Competition Activity
        //        CompetitionActivity ca = await _competitionActivityRepo.Get(mta.CompetitionActivityId);
        //        //Canceling
        //        if (ca.Status == CompetitionActivityStatus.Canceling) throw new ArgumentException("Competition Activity is canceling");



        //        bool check = await CheckConditions(token, ca.CompetitionId, model.ClubId);

        //        //Check role CompetitionManager --> chắc chắn nó sẽ là task của competition
        //        if (check)
        //        {
        //            //Approved
        //            if (((int)model.Status) == 4)
        //            {
        //                mta.Status = MemberTakesActivityStatus.Approved;
        //                await _memberTakesActivityRepo.Update();

        //            }
        //            //Rejected
        //            if (((int)model.Status) == 5)
        //            {
        //                mta.Status = MemberTakesActivityStatus.Rejected;
        //                await _memberTakesActivityRepo.Update();

        //            }

        //            //Update Status của Competition Activity
        //            int numberOfMemberHasSubmit = await _memberTakesActivityRepo.GetNumberOfMemberIsSubmitted(ca.Id);
        //            if (numberOfMemberHasSubmit == ca.NumOfMember)
        //            {
        //                ca.Process = CompetitionActivityProcessStatus.Complete;
        //                await _competitionActivityRepo.Update();
        //            }

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private async Task<ViewDetailMemberTakesActivity> TransferViewDetailMTA(MemberTakesActivity mta)
        //{
        //    // Booker           
        //    User booker = await _userRepo.Get(mta.UserId);

        //    // Member
        //    Member m2 = await _memberRepo.Get(mta.MemberId);
        //    User member = await _userRepo.Get(m2.UserId);

        //    return new ViewDetailMemberTakesActivity()
        //    {
        //        Id = mta.Id,
        //        MemberId = mta.MemberId,
        //        MemberName = member.Fullname,
        //        CompetitionActivityId = mta.CompetitionActivityId,
        //        StartTime = mta.StartTime,
        //        EndTime = mta.EndTime,
        //        Deadline = mta.Deadline,
        //        Status = mta.Status,
        //        BookerId = mta.UserId,
        //        BookerName = booker.Fullname
        //    };
        //}


        ////private int DecodeToken(string token, string nameClaim)
        ////{
        ////    if (_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
        ////    var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
        ////    return Int32.Parse(claim.Value);
        ////}

        ////Check Conditition will be return BookerId
        //private async Task<bool> CheckConditions(string Token, int CompetitionId, int ClubId)
        //{
        //    //
        //    int UserId = _decodeToken.Decode(Token, "Id");

        //    //------------- CHECK Competition is have in system or not
        //    Competition competition = await _competitionRepo.Get(CompetitionId);
        //    if (competition != null)
        //    {
        //        //------------- CHECK Club in system
        //        Club club = await _clubRepo.Get(ClubId);
        //        if (club != null)
        //        {
        //            ViewTerm CurrentTermOfCLub = await _termRepo.GetCurrentTermByClub(ClubId);
        //            if (CurrentTermOfCLub != null)
        //            {
        //                GetMemberInClubModel conditions = new GetMemberInClubModel()
        //                {
        //                    UserId = UserId,
        //                    ClubId = ClubId,
        //                    TermId = CurrentTermOfCLub.Id
        //                };
        //                ViewBasicInfoMember infoClubMem = await _memberRepo.GetBasicInfoMember(conditions);
        //                //------------- CHECK Mem in that club
        //                if (infoClubMem != null)
        //                {
        //                    //------------- CHECK is in CompetitionManger table                
        //                    CompetitionManager isAllow = await _competitionManagerRepo.GetMemberInCompetitionManager(CompetitionId, infoClubMem.UserId, ClubId);
        //                    if (isAllow != null)
        //                    {
        //                        return true ;
        //                    }
        //                    else
        //                    {
        //                        throw new UnauthorizedAccessException("You do not in Competition Manager");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new UnauthorizedAccessException("You are not member in Club");
        //                }
        //            }
        //            else
        //            {
        //                throw new ArgumentException("Term of ClubId is End");
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Club is not found");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Competition or Event not found ");
        //    }
        //}
    }
}
