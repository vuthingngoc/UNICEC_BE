using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Services.FileSvc;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.MemberSvc
{
    public class MemberService : IMemberService
    {
        private IMemberRepo _memberRepo;
        private IClubRepo _clubRepo;
        private IUserRepo _userRepo;
        private ITermRepo _termRepo;
        private IClubRoleRepo _clubRoleRepo;

        private IFileService _fileService;
        private DecodeToken _decodeToken;

        public MemberService(IMemberRepo memberRepo, IClubRepo clubRepo, IFileService fileService
                                , IUserRepo userRepo, ITermRepo termRepo, IClubRoleRepo clubRoleRepo)
        {
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _fileService = fileService;
            _userRepo = userRepo;
            _termRepo = termRepo;
            _clubRoleRepo = clubRoleRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<PagingResult<ViewMember>> GetByClub(string token, int clubId, int? termId, MemberStatus? status, PagingRequest request)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            MemberStatus memberStatus = MemberStatus.Active;
            int clubRoldId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            if ((clubRoldId.Equals(1) || clubRoldId.Equals(2)) && status.HasValue) memberStatus = status.Value;

            int clubTermId = await _termRepo.GetCurrentTermIdByClub(clubId);
            if (termId.HasValue) clubTermId = termId.Value;

            PagingResult<ViewMember> members = await _memberRepo.GetMembersByClub(clubId, clubTermId, memberStatus, request);
            if (members == null) throw new NullReferenceException("Not found any member in this club");
            return members;
        }

        public async Task<ViewDetailMember> GetByMemberId(string token, int memberId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int clubId = await _memberRepo.GetClubIdByMember(memberId);
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ViewDetailMember member = await _memberRepo.GetDetailById(memberId);
            if (member == null) throw new NullReferenceException("Not found this member");
            return member;
        }

        public async Task<List<ViewIntroClubMember>> GetLeadersByClub(int clubId)
        {
            List<ViewIntroClubMember> members = await _memberRepo.GetLeadersByClub(clubId);
            //foreach(var member in members)
            //{
            //    member.Avatar = (member.Avatar.Contains("firebase")) ? 
            //        await _fileService.GetUrlFromFilenameAsync(member.Avatar) : member.Avatar;
            //}
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
            if (model.ClubId == 0 || model.UserId == 0 || model.ClubRoleId == 0 || model.StartTime.Equals(DateTime.MinValue))
                throw new ArgumentException("ClubId Null || UserId Null || ClubRoleId Null || TermId Null || StartTime Null");

            // check role
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Club club = await _clubRepo.Get(model.ClubId);
            if (club == null) throw new NullReferenceException("Not found this club");

            // check valid member
            bool isExistedUniStudent = await _userRepo.CheckExistedUser(club.UniversityId, model.UserId);
            if (!isExistedUniStudent) throw new UnauthorizedAccessException("This user is not in the club's university");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(model.UserId, model.ClubId);
            if (isMember) throw new ArgumentException("The user has already in this club");

            int currentTerm = (await _termRepo.GetCurrentTermByClub(model.ClubId)).Id;
            bool isExistedClubRoleId = await _clubRoleRepo.CheckExistedClubRole(model.ClubRoleId);
            if (!isExistedClubRoleId) throw new ArgumentException("Not found this club role");

            Member member = new Member()
            {
                UserId = model.UserId,
                ClubId = model.ClubId,
                ClubRoleId = model.ClubRoleId,
                Status = MemberStatus.Active, // default status
                StartTime = DateTime.Now,
                TermId = currentTerm,
            };
            int memberId = await _memberRepo.Insert(member);
            if (memberId == 0) throw new DbUpdateException();

            club.TotalMember += 1;
            await _clubRepo.Update();

            return await _memberRepo.GetById(memberId);
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

            if (clubRoleId >= member.ClubRoleId) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int currentTerm = (await _termRepo.GetCurrentTermByClub(member.ClubId)).Id;
            if (currentTerm != member.TermId || member.Status.Equals(MemberStatus.Inactive)) throw new ArgumentException("Can not update member in the past");

            if (member.ClubRoleId.Equals(model.ClubRoleId)) return;

            var currentTime = new LocalTime().GetLocalTime().DateTime;            
            member.EndTime = currentTime;
            member.Status = MemberStatus.Inactive;
            await _memberRepo.Update();

            Member newRecord = new Member()
            {
                ClubId = member.ClubId,
                ClubRoleId = model.ClubRoleId,
                StartTime = currentTime,
                TermId = member.TermId,
                UserId = member.UserId,
                Status = MemberStatus.Active
            };
            await _memberRepo.Insert(newRecord);
        }

        public async Task Delete(string token, int memberId)
        {
            Member member = await _memberRepo.Get(memberId);
            if (member == null) throw new NullReferenceException("Not found this member");

            // if user is not leader or vice president
            int userId = _decodeToken.Decode(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, member.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (clubRoleId >= member.ClubRoleId) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (member.Status.Equals(MemberStatus.Inactive)) throw new ArgumentException("This member is inactive already");

            member.Status = MemberStatus.Inactive;
            member.EndTime = new LocalTime().GetLocalTime().DateTime;
            await _memberRepo.Update();

            Club club = await _clubRepo.Get(member.ClubId);
            club.TotalMember -= 1;
            await _clubRepo.Update();
        }

        public async Task InsertForNewTerm(int clubId, int termId)
        {
            // check role in term service already 
            // if use for another => please check role in here

            List<Member> members = await _memberRepo.GetMembersByClub(clubId);
            if (members != null)
            {
                await _memberRepo.UpdateEndTerm(clubId);
                // insert new records
                foreach (Member record in members)
                {
                    record.TermId = termId;
                    await _memberRepo.Insert(record);
                }
            }
        }

        // Tien Anh
        //public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        //{
        //    //ViewClubMember result = await _clubHistoryRepo.GetMemberInCLub(model);
        //    //return result;
        //}
    }
}
