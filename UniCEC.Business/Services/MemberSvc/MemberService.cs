using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.NotificationSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public class MemberService : IMemberService
    {
        private IMemberRepo _memberRepo;
        private IClubRepo _clubRepo;
        private IUserRepo _userRepo;
        private IClubRoleRepo _clubRoleRepo;
        private INotificationService _notificationService;
        private IMemberTakesActivityRepo _memberTakesActivityRepo;
        private DecodeToken _decodeToken;


        public MemberService(INotificationService notificationService, IMemberRepo memberRepo, IClubRepo clubRepo, IUserRepo userRepo, IClubRoleRepo clubRoleRepo, IMemberTakesActivityRepo memberTakesActivityRepo)
        {
            _notificationService = notificationService;
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _userRepo = userRepo;
            _clubRoleRepo = clubRoleRepo;
            _memberTakesActivityRepo = memberTakesActivityRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<PagingResult<ViewMember>> GetByClub(string token, int clubId, MemberStatus? status, PagingRequest request)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            MemberStatus memberStatus = MemberStatus.Active;
            int clubRoldId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            if ((clubRoldId.Equals(1) || clubRoldId.Equals(2)) && status.HasValue) memberStatus = status.Value;

            PagingResult<ViewMember> members = await _memberRepo.GetMembersByClub(clubId, memberStatus, request);
            if (members == null) throw new NullReferenceException("Not found any member in this club");

            return members;
        }

        public async Task<PagingResult<ViewMember>> GetByConditions(string token, MemberRequestModel request)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, request.ClubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int clubRoldId = await _memberRepo.GetRoleMemberInClub(userId, request.ClubId);
            if ((!clubRoldId.Equals(1) && !clubRoldId.Equals(2)) || !request.Status.HasValue) request.Status = MemberStatus.Active;

            PagingResult<ViewMember> members = await _memberRepo.GetByConditions(request);
            if (members == null) throw new NullReferenceException("Not found any member in this club");

            return members;
        }

        public async Task<ViewDetailMember> GetByMemberId(string token, int memberId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int clubId = await _memberRepo.GetClubIdByMember(memberId);
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            MemberStatus? status = null;
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            if (clubRoleId.Equals(3) || clubRoleId.Equals(4)) status = MemberStatus.Active; // manager or member

            ViewDetailMember member = await _memberRepo.GetDetailById(memberId, status);
            if (member == null) throw new NullReferenceException("Not found this member");
            return member;
        }

        public async Task<List<ViewIntroClubMember>> GetLeadersByClub(string token, int clubId)
        {
            int universityId = _decodeToken.Decode(token, "UniversityId");
            bool isExisted = await _clubRepo.CheckExistedClubInUniversity(universityId, clubId);
            if (!isExisted) throw new UnauthorizedAccessException("This club is not in your university");

            List<ViewIntroClubMember> members = await _memberRepo.GetLeadersByClub(clubId);
            if (members == null) throw new NullReferenceException("Not found any Leaders");
            return members;
        }

        public async Task<int> GetQuantityNewMembersByClub(string token, int clubId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int quantity = await _memberRepo.GetQuantityNewMembersByClub(clubId);
            if (quantity < 0) throw new NullReferenceException("Not found this club");
            return quantity;
        }

        //Insert-Member
        public async Task<ViewMember> Insert(string token, int clubId)
        {
            // check valid data 
            if (clubId == 0) throw new ArgumentException("ClubId Null");
            int userId = _decodeToken.Decode(token, "Id");

            Club club = await _clubRepo.Get(clubId);
            if (club == null) throw new NullReferenceException("Not found this club");

            // check valid member
            bool isExistedUniStudent = await _userRepo.CheckExistedUser(club.UniversityId, userId);
            if (!isExistedUniStudent) throw new UnauthorizedAccessException("This user is not in the club's university");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (isMember) throw new ArgumentException("The user has already in this club");

            // processing
            int memberId = 0;
            bool isMemberWithInActive = await _memberRepo.CheckExistedMemberInClubWhenInsert(userId, clubId);
            if (isMemberWithInActive)
            {
                memberId = await _memberRepo.GetIdByUserWhenInsert(userId, clubId);
                if (memberId == 0) throw new DbUpdateException();
                Member mem = await _memberRepo.Get(memberId);
                mem.StartTime = new LocalTime().GetLocalTime().DateTime;
                mem.EndTime = null; // reset lại thời gian 
                mem.Status = MemberStatus.Pending; // default status 
                await _memberRepo.Update();
                club.TotalMember += 1;
                await _clubRepo.Update();
            }
            //kh có record cũ thì mới tạo
            else
            {
                Member member = new Member()
                {
                    UserId = userId,
                    ClubId = clubId,
                    ClubRoleId = 4, // is member by default
                    Status = MemberStatus.Pending, // default status 
                    StartTime = new LocalTime().GetLocalTime().DateTime,
                };
                memberId = await _memberRepo.Insert(member);
                if (memberId == 0) throw new DbUpdateException();

                club.TotalMember += 1;
                await _clubRepo.Update();
            }


            ViewMember viewMember = await _memberRepo.GetById(memberId);

            return viewMember;
        }

        public async Task ConfirmMember(string token, ConfirmMemberModel model)
        {
            if (model.MemberId.Equals(0) || model.ClubId.Equals(0) || model.Status.Equals(MemberStatus.Pending))
                throw new ArgumentException("MemberId Null || ClubId Null || MemberStatus Null");

            // check role
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2))
                throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Member member = await _memberRepo.Get(model.MemberId);
            if (member == null || (member != null && !member.ClubId.Equals(model.ClubId)))
                throw new NullReferenceException("Not found this member");

            if (member.Status.Equals(MemberStatus.Pending))
            {
                member.Status = model.Status;
                await _memberRepo.Update();
                // send notification
                Club club = await _clubRepo.Get(model.ClubId);
                string deviceToken = await _userRepo.GetDeviceTokenByUser(member.UserId);
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    string body = (model.Status.Equals(MemberStatus.Active))
                    ? $"Chúc mừng bạn đã trở thành thành viên câu lạc bộ {club.Name}"
                    : $"Câu lạc bộ {club.Name} đã từ chối bạn";
                    Notification notification = new Notification()
                    {
                        Title = "Thông báo",
                        Body = body,
                        RedirectUrl = "/notification",
                        UserId = member.UserId,
                    };

                    await _notificationService.SendNotification(notification, deviceToken);
                }
            }
            else
            {
                throw new ArgumentException("This user does not apply to the club!");
            }
        }

        //Update-Member
        public async Task Update(string token, MemberUpdateModel model)
        {
            Member member = await _memberRepo.Get(model.Id);
            if (member == null || member.Status.Equals(MemberStatus.Inactive)) throw new NullReferenceException("Not found this member");

            bool isValid = await _clubRoleRepo.CheckExistedClubRole(model.ClubRoleId);
            if (!isValid) throw new ArgumentException("Not found this club role");

            // Admin update club manager
            if (model.ClubRoleId.Equals(1))
            {
                int roleId = _decodeToken.Decode(token, "RoleId");
                if (!roleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to access this resoure");
                int universityId = _decodeToken.Decode(token, "UniversityId");
                // check admin and club is the same uni
                bool isExistedClub = await _clubRepo.CheckExistedClubInUniversity(universityId, member.ClubId);
                if (!isExistedClub) throw new UnauthorizedAccessException("You do not have permission to access this resoure");
                // Get current club manager
                Member clubManager = await _memberRepo.GetLeaderByClub(member.ClubId);
                clubManager.ClubRoleId = 4; // role member
                member.ClubRoleId = model.ClubRoleId;
                await _memberRepo.Update();
                return;
            }

            // if user is leader or vice president
            // proposer
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, member.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2))
                throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (clubRoleId >= member.ClubRoleId)
                throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (member.ClubRoleId.Equals(model.ClubRoleId)) return;

            member.ClubRoleId = model.ClubRoleId;
            await _memberRepo.Update();
        }

        public async Task Delete(string token, int memberId)
        {
            Member member = await _memberRepo.Get(memberId);
            if (member == null
                || member.Status.Equals(MemberStatus.Inactive)
                || member.Status.Equals(MemberStatus.Pending))
                throw new NullReferenceException("Not found this member");

            int userId = _decodeToken.Decode(token, "Id");

            if (!member.UserId.Equals(userId)) // if user is not leader or vice president
            {
                int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, member.ClubId);
                if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

                if (clubRoleId >= member.ClubRoleId) throw new UnauthorizedAccessException("You do not have permission to access this resource");

                if (member.Status.Equals(MemberStatus.Inactive)) return;
            }

            member.Status = MemberStatus.Inactive;
            member.EndTime = new LocalTime().GetLocalTime().DateTime;
            await _memberRepo.Update();

            Club club = await _clubRepo.Get(member.ClubId);
            club.TotalMember -= 1;
            await _clubRepo.Update();

            // xóa member ra khỏi task đang làm 
            await _memberTakesActivityRepo.RemoveMemberTakeAllTaskIsDoing(memberId);

            // send notification
            if (!member.UserId.Equals(userId)) // if user is kicked by club managers
            {
                string deviceToken = await _userRepo.GetDeviceTokenByUser(member.UserId);
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    string body = $"{club.Name} đã mời bạn ra khỏi câu lạc bộ";
                    Notification notification = new Notification()
                    {
                        Title = "Thông báo",
                        Body = body,
                        RedirectUrl = "/notification",
                        UserId = member.UserId,
                    };
                    await _notificationService.SendNotification(notification, deviceToken);
                }
            }
        }

        public async Task<List<ViewDetailMember>> GetMemberInfoByClub(string token, int? clubId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            List<ViewDetailMember> members = await _memberRepo.GetMemberInfoByClub(userId, clubId);
            if (members == null) throw new NullReferenceException("Not found any members info");
            return members;
        }

        // Tien Anh
        public async Task<List<ViewMember>> GetMembersByClub(string token, int clubId, string searchName, int? roleId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            List<Member> members = await _memberRepo.GetMembersByClub(clubId, searchName, roleId);

            if (members == null) throw new NullReferenceException("Not found any member in this club");

            List<ViewMember> viewMembers = new List<ViewMember>();

            foreach (Member member in members)
            {
                User user = await _userRepo.Get(member.UserId);
                ClubRole cr = await _clubRoleRepo.Get(member.ClubRoleId);

                ViewMember vm = new ViewMember()
                {
                    Id = member.Id,
                    StudentId = user.Id,
                    Name = user.Fullname,
                    StudentCode = user.StudentCode,
                    Avatar = user.Avatar,
                    ClubRoleId = member.ClubRoleId,
                    ClubRoleName = cr.Name,
                    IsOnline = user.IsOnline,
                    StartTime = member.StartTime,
                    EndTime = member.EndTime,
                    Status = member.Status
                };

                viewMembers.Add(vm);
            }

            return (viewMembers.Count > 0) ? viewMembers : null;
        }


        //public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        //{
        //    //ViewClubMember result = await _clubHistoryRepo.GetMemberInCLub(model);
        //    //return result;
        //}
    }
}
