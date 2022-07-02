using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
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

        private IFileService _fileService;
        private DecodeToken _decodeToken;

        public MemberService(IMemberRepo memberRepo, IClubRepo clubRepo, IFileService fileService
                                , IUserRepo userRepo, IClubRoleRepo clubRoleRepo)
        {
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _fileService = fileService;
            _userRepo = userRepo;
            _clubRoleRepo = clubRoleRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task<string> GetUrlImageAsync(string filename)
        {
            string urlImage;
            try
            {
                urlImage = await _fileService.GetUrlFromFilenameAsync(filename);
            }
            catch (Exception)
            {
                urlImage = "";
            }

            return urlImage;
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

            foreach (var member in members.Items)
            {
                member.Avatar = await GetUrlImageAsync(member.Avatar);
            }            

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
            
            foreach (var member in members.Items)
            {
                member.Avatar = await GetUrlImageAsync(member.Avatar);
            }            
            
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
            foreach (var member in members)
            {
                member.Avatar = await GetUrlImageAsync(member.Avatar);
            }
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
        public async Task<ViewMember> Insert(string token, MemberInsertModel model)
        {
            // check valid data 
            if (model.ClubId == 0 || model.UserId == 0)
                throw new ArgumentException("ClubId Null || UserId Null");            

            Club club = await _clubRepo.Get(model.ClubId);
            if (club == null) throw new NullReferenceException("Not found this club");

            // check valid member
            bool isExistedUniStudent = await _userRepo.CheckExistedUser(club.UniversityId, model.UserId);
            if (!isExistedUniStudent) throw new UnauthorizedAccessException("This user is not in the club's university");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(model.UserId, model.ClubId);
            if (isMember) throw new ArgumentException("The user has already in this club");

            Member member = new Member()
            {
                UserId = model.UserId,
                ClubId = model.ClubId,
                ClubRoleId = 4, // is member by default
                Status = MemberStatus.Active, // default status
                StartTime = new LocalTime().GetLocalTime().DateTime,
            };
            int memberId = await _memberRepo.Insert(member);
            if (memberId == 0) throw new DbUpdateException();

            club.TotalMember += 1;
            await _clubRepo.Update();

            ViewMember viewMember = await _memberRepo.GetById(memberId);
            if (viewMember != null) viewMember.Avatar = await GetUrlImageAsync(viewMember.Avatar);

            return viewMember;
        }

        public async Task ConfirmMember(string token, ConfirmMemberModel model)
        {
            if (model.MemberId.Equals(0) || model.ClubId.Equals(0) || model.status.Equals(MemberStatus.Pending))
                throw new ArgumentException("MemberId Null || ClubId Null || MemberStatus Null");

            // check role
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) 
                throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Member member = await _memberRepo.Get(model.MemberId);
            if (member == null || (member != null && !member.ClubId.Equals(model.ClubId))) 
                throw new NullReferenceException("Not found this member");

            member.Status = model.status;
            await _memberRepo.Update();
        }

        //Update-Member
        public async Task Update(string token, MemberUpdateModel model)
        {
            Member member = await _memberRepo.Get(model.Id);
            if (member == null) throw new NullReferenceException("Not found this member");

            bool isValid = await _clubRoleRepo.CheckExistedClubRole(model.ClubRoleId);
            if (!isValid) throw new ArgumentException("Not found this club role");

            // if user is not leader or vice president
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, member.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (clubRoleId <= member.ClubRoleId) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (member.ClubRoleId.Equals(model.ClubRoleId)) return;

            member.ClubRoleId = clubRoleId;
            await _memberRepo.Update();
        }

        public async Task Delete(string token, int memberId)
        {
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new NullReferenceException("Not found this member");

            // if user is not leader or vice president
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, member.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (clubRoleId <= member.ClubRoleId) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (member.Status.Equals(MemberStatus.Inactive)) return;

            member.Status = MemberStatus.Inactive;
            member.EndTime = new LocalTime().GetLocalTime().DateTime;
            await _memberRepo.Update();

            Club club = await _clubRepo.Get(member.ClubId);
            club.TotalMember -= 1;
            await _clubRepo.Update();
        }

        // Tien Anh
        //public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        //{
        //    //ViewClubMember result = await _clubHistoryRepo.GetMemberInCLub(model);
        //    //return result;
        //}
    }
}
