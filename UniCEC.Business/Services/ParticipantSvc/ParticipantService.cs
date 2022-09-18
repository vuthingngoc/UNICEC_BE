using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.ParticipantRepo;
using UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;

namespace UniCEC.Business.Services.ParticipantSvc
{
    public class ParticipantService : IParticipantService
    {
        private IParticipantRepo _participantRepo;
        private ICompetitionRepo _competitionRepo;
        private IClubRepo _clubRepo;
        private IMemberRepo _memberRepo;
        private IUserRepo _userRepo;
        private ISeedsWalletRepo _seedsWalletRepo;
        private DecodeToken _decodeToken;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;

        public ParticipantService(IParticipantRepo participantRepo,
                                  ICompetitionRepo competitionRepo,
                                  IClubRepo clubRepo,
                                  IMemberRepo memberRepo,
                                  ISeedsWalletRepo seedsWalletRepo,
                                  IMemberInCompetitionRepo memberInCompetitionRepo,
                                  IUserRepo userRepo)
        {
            _participantRepo = participantRepo;
            _competitionRepo = competitionRepo;
            _clubRepo = clubRepo;
            _memberRepo = memberRepo;
            _userRepo = userRepo;
            _seedsWalletRepo = seedsWalletRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _decodeToken = new DecodeToken();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewParticipant>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewParticipant> GetByCompetitionId(int competitionId, string token)
        {
            try {
                int userId =  _decodeToken.Decode(token, "Id");
                ViewParticipant result = await _participantRepo.GetByCompetitionId(competitionId, userId);
                if (result == null) throw new NullReferenceException();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagingResult<ViewParticipant>> GetByConditions(ParticipantRequestModel request, string token)
        {
            try {
                bool check = await CheckMemberInCompetition(token, request.CompetitionId, request.ClubId, true);
                if (check == false) throw new ArgumentException("You dont have permission to do this action");
                
                    PagingResult<ViewParticipant> result = await _participantRepo.GetByConditions(request);
                    if (result == null) throw new NullReferenceException();
                    return result;
                     
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> Update(ViewParticipant participant)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewParticipant> Insert(ParticipantInsertModel model, string token)
        {
            try
            {

                int UserId = _decodeToken.Decode(token, "Id");
                int UniversityId = _decodeToken.Decode(token, "UniversityId");

                //get Student Info
                User studentInfo = await _userRepo.Get(UserId);

                //check null insert
                if (model.CompetitionId == 0) throw new ArgumentNullException("Competition Id Null");

                //get Compeition by CompetitionId
                Competition competition = await _competitionRepo.Get(model.CompetitionId);

                //
                if (competition == null) throw new ArgumentException("Competition not found");

                //check add duplicate student 
                //false -> student can add
                //true -> student can't add
                if (await _participantRepo.CheckDuplicateUser(studentInfo, model.CompetitionId))
                    throw new ArgumentException("Student already join in this Competition");

                //ROUND 1 - Kiểm tra User có nằm trong đối tượng tham gia Competition or Event này hay không ?
                bool round1 = false;
                //Inter University
                if (competition.Scope == CompetitionScopeStatus.InterUniversity)
                {
                    //ai cũng có thể join
                    round1 = true;
                }

                //NoPublic -- dành cho nội bộ trường
                if (competition.Scope == CompetitionScopeStatus.University)
                {
                    //get Club in Competition to check university id
                    List<CompetitionInClub> listComp_In_Club = competition.CompetitionInClubs.ToList();
                    int clubId = listComp_In_Club[0].ClubId;
                    Club club = await _clubRepo.Get(clubId);
                    int UniversityIdOfCompetition = club.UniversityId;

                    if (studentInfo.UniversityId == UniversityIdOfCompetition)
                    {
                        round1 = true;
                    }
                    else
                    {
                        throw new ArgumentException("User isn't a student in this University, Competition can't support");
                    }
                }

                //Scope = Club -- dành riêng 1 hoặc nhiều Club
                if (competition.Scope == CompetitionScopeStatus.Club)
                {
                    //chỉ kiểm tra trong trường                                
                    //Khác university thì kh join đc 
                    List<CompetitionInClub> listComp_In_Club = competition.CompetitionInClubs.ToList();

                    //List<Club> listClub = new List<Club>();
                    List<int> listClub_Id = new List<int>();

                    foreach (CompetitionInClub cic in listComp_In_Club)
                    {
                        //listClub.Add(cic.Club);
                        listClub_Id.Add(cic.Club.Id);
                    }
                    //kiểm tra xem có là thành viên trong CLB                                   
                    Member stuIsMember = await _memberRepo.IsMemberInListClubCompetition(listClub_Id, studentInfo);
                    if (stuIsMember != null)
                    {
                        round1 = true;
                    }
                    else
                    {
                        throw new ArgumentException("User isn't a Member, Competition can't support");
                    }
                    //int uniId_In_Comp = listClub.FirstOrDefault().UniversityId;

                    //kiểm tra có trùng Uni hay không
                    //if (uniId_In_Comp == UniversityId)
                    //{
                    //}//end if uniId_In_Comp
                    // else
                    // {
                    //throw new ArgumentException("User isn't a student in this University, Competition can't support");
                    //}
                }

                //ROUND 2 - Check xem Competition is RegisterTime
                bool round2 = false;
                if (round1)
                {
                    if (competition.Status == CompetitionStatus.Register)
                    {
                        round2 = true;
                    }//end check Registering
                    else
                    {
                        throw new ArgumentException("Time Register is end for this Compeititon");
                    }
                }

                //ROUND 3 - Check Number Of Participant is full or Not??
                bool round3 = false;
                if (round1 && round2)
                {
                    //Check Number Of Participant join this competition
                    if (_participantRepo.CheckNumOfParticipant(competition.Id, competition.NumberOfParticipation))
                    {
                        round3 = true;
                    }
                    else
                    {
                        throw new ArgumentException("Competition is full of Participant");
                    }
                }

                //ROUND 4 - Check seed Deposited
                bool round4 = false;
                if (round1 && round2 && round3)
                {
                    if (competition.SeedsDeposited != 0)
                    {
                        SeedsWallet sw = await _seedsWalletRepo.GetByStudentId(_decodeToken.Decode(token,"Id"));
                        //
                        if (sw.Amount - competition.SeedsDeposited < 0) throw new ArgumentException("You don't have enough seeds point");
                        sw.Amount = sw.Amount - competition.SeedsDeposited;
                        await _seedsWalletRepo.Update();
                        round4 = true;
                    }
                    else
                    {
                        round4 = true;
                    }
                }

                //ROUND 5 - Create Participant
                if (round1 && round2 && round3 && round4)
                {
                    //-------------- INSERT PARTICIPANT
                    Participant participant = new Participant();
                    participant.StudentId = studentInfo.Id;
                    participant.RegisterTime = new LocalTime().GetLocalTime().DateTime;                  
                    participant.CompetitionId = competition.Id;
                    participant.Attendance = false;     // not check attendence yet    
                    int result = await _participantRepo.Insert(participant);
                    if (result != 0)
                    {
                        Participant p = await _participantRepo.Get(result);
                        //                              
                        return TransformViewModel(p, studentInfo);
                    }
                }
                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAttendance(string seedsCode, string token)
        {
            try
            {
                //Check seedsCode
                if (string.IsNullOrEmpty(seedsCode)) throw new ArgumentException("seedsCode Null");

                //Get competition
                Competition competition = await _competitionRepo.GetCompetitionBySeedsCode(seedsCode);
                if (competition == null) throw new ArgumentException("Competition not found");

                //Check Participant in this Competition 
                User studentInfo = await _userRepo.Get(_decodeToken.Decode(token, "Id"));
                bool check = await _participantRepo.CheckDuplicateUser(studentInfo, competition.Id);
                if (check == false) throw new ArgumentException("You are not participant in this Competition");

                //Check if they call again
                Participant participant = await _participantRepo.ParticipantInCompetition(_decodeToken.Decode(token, "Id"), competition.Id);
                if (participant.Attendance == true) throw new ArgumentException("You already attendance in Competition");

                participant.Attendance = true;
                await _participantRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ViewParticipant TransformViewModel(Participant participant, User student)
        {
            return new ViewParticipant()
            {
                Id = participant.Id,
                CompetitionId = participant.CompetitionId,
                //MemberId = participant.MemberId,
                RegisterTime = participant.RegisterTime,
                StudentId = participant.StudentId,
                Avatar = student.Avatar,
                StudentCode = student.StudentCode,
                StudentName = student.Fullname,
                Attendance = participant.Attendance
            };
        }

        private async Task<bool> CheckMemberInCompetition(string Token, int CompetitionId, int ClubId, bool isOrganization)
        {
            //------------- CHECK Competition in system
            Competition competition = await _competitionRepo.Get(CompetitionId);
            if (competition == null) throw new ArgumentException("Competition or Event not found ");

            //------------- CHECK Club in system
            Club club = await _clubRepo.Get(ClubId);
            if (club == null) throw new ArgumentException("Club in not found");

            //------------- CHECK Is Member in Club
            int memberId = await _memberRepo.GetIdByUser(_decodeToken.Decode(Token, "Id"), club.Id);
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new UnauthorizedAccessException("You aren't member in Club");

            //------------- CHECK User is in CompetitionManger table                
            MemberInCompetition isAllow = await _memberInCompetitionRepo.GetMemberInCompetition(CompetitionId, memberId);
            if (isAllow == null) throw new UnauthorizedAccessException("You do not in Competition Manager ");

            if (isOrganization)
            {
                //1,2 accept
                if (isAllow.CompetitionRoleId >= 3) throw new UnauthorizedAccessException("Only role Manager can do this action");
                return true;
            }
            else
            {
                return true;
            }
        }


    }
}




//-------------------------------------------------------Sponsor Create Competition Or Event
//if (competition.IsSponsor)
//{
//    //----------------------------------------------Check level Department
//    //dành cho những sinh viên thuộc nghành
//    // lấy list Department có trong competition
//    List<int> listDepartmentId = await _competitionInDepartmentRepo.GetListDepartmentId_In_Competition(competition.Id);
//    if (listDepartmentId != null)
//    {
//        //Pulic
//        if (competition.Public)
//        {
//            //kiểm tra xem trong list DepartmentId đó có trong University của user đó hay kh
//            bool dep_in_uni = await _departmentInUniversityRepo.checkDepartmentBelongToUni(listDepartmentId, UniversityId);
//            if (dep_in_uni)
//            {
//                //xong mới tới kiểm tra xem UserId ~ Student đó có thuộc Department đó kh
//                // userID - MajorID -> Student thuộc nghành đó
//                bool stu_in_dep = await _participantRepo.CheckStudentInCom_In_Dep(listDepartmentId, studentInfo);
//                //có thì qua ải
//                if (stu_in_dep)
//                {
//                    round1 = true;
//                }
//                //end check stu_in_dep
//                else
//                {
//                    throw new ArgumentException("Student don't studying in that Major belong to this Department, Competition can't support");
//                }

//            }
//            //end if dep_in_uni
//            else
//            {
//                throw new ArgumentException("University don't have this Department, Competition can't support");
//            }
//        }
//    }//end if listDepartmentId

//    //Cuộc thi mà nhà tài trợ tạo ra cho tất cả ai muốn vào thì vào                            
//}


////----------------------------------------------Check level Department
//// dành cho những sinh viên thuộc nghành 
//// lấy list Department có trong competition
//List<int> listDepartmentId = await _competitionInDepartmentRepo.GetListDepartmentId_In_Competition(competition.Id);
//if (listDepartmentId != null)
//{
//    //Pulic
//    if (competition.Public)
//    {
//        //kiểm tra xem trong list DepartmentId đó có trong University của user đó hay kh
//        bool dep_in_uni = await _departmentInUniversityRepo.CheckDepartmentBelongToUni(listDepartmentId, UniversityId);
//        if (dep_in_uni)
//        {
//            //xong mới tới kiểm tra xem UserId ~ Student đó có thuộc Department đó kh
//            // userID - MajorID -> Student thuộc nghành đó
//            bool stu_in_dep = await _participantRepo.CheckStudentInCom_In_Dep(listDepartmentId, studentInfo);
//            if (stu_in_dep)
//            {
//                round1 = true;
//            }
//            //end check stu_in_dep
//            else
//            {
//                throw new ArgumentException("Student don't studying in that Major belong to this Department, Competition can't support");
//            }

//        }
//        //end if dep_in_uni
//        else
//        {
//            throw new ArgumentException("University don't have this Department, Competition can't support");
//        }
//    }
//    //NoPublic
//    if (competition.Public == false)
//    {
//        //Khác university thì kh join đc 
//        Club club = competition.CompetitionInClubs.ToList().FirstOrDefault().Club;
//        int uniId_In_Comp = club.UniversityId;
//        //kiểm tra có trùng Uni hay không
//        if (uniId_In_Comp == UniversityId)
//        {
//            //kiểm tra xem UserId ~ Student đó có thuộc Department đó kh                                  
//            bool stu_in_dep = await _participantRepo.CheckStudentInCom_In_Dep(listDepartmentId, studentInfo);
//            if (stu_in_dep)
//            {
//                round1 = true;
//            }
//            //end check stu_in_dep
//            else
//            {
//                throw new ArgumentException("Student don't studying in that Major belong to this Department, Competition can't support");
//            }

//        }
//        else
//        {
//            throw new ArgumentException("User is not a student in this University, Competition can't support");
//        }
//    }
//}
//----------------------------------------------Check level Club
//dành cho sinh viên có thể thuộc hoặc kh thuộc clb
//if (listDepartmentId == null)
//{ }
