using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Business.Services.MemberSvc
{
    public class MemberService : IMemberService
    {
        private IMemberRepo _memberRepo;
        private IClubRepo _clubRepo;
        private IClubHistoryRepo _clubHistoryRepo;
        private JwtSecurityTokenHandler _tokenHandler;

        public MemberService(IMemberRepo memberRepo, IClubRepo clubRepo, IClubHistoryRepo clubHistoryRepo)
        {
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _clubHistoryRepo = clubHistoryRepo;
        }

        public int DecodeToken(string token, string nameClaim)
        {
            if(_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public async Task<PagingResult<ViewMember>> GetByClub(string token, int clubId, PagingRequest request)
        {
            int userId = DecodeToken(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            PagingResult<ViewMember> members = await _memberRepo.GetAllMemberByClub(clubId, request);
            if (members == null) throw new NullReferenceException("Not found any member in this club");
            return members;
        }

        public async Task<ViewDetailMember> GetByMemberId(string token, int id, int clubId)
        {
            int userId = DecodeToken(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ViewDetailMember member = await _memberRepo.GetById(id, clubId);
            if (member == null) throw new NullReferenceException("Not found this member");
            return member;
        }

        public async Task<List<ViewMember>> GetLeadersByClub(int clubId)
        {
            List<ViewMember> members = await _memberRepo.GetLeadersByClub(clubId);
            if (members == null) throw new NullReferenceException("Not found any Leaders");
            return members;
        }

        public async Task<int> GetQuantityNewMembersByClub(string token, int clubId)
        {
            int userId = DecodeToken(token, "Id");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (!isMember) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int quantity = await _memberRepo.GetQuantityNewMembersByClub(clubId);
            if (quantity < 0) throw new NullReferenceException("Not found this club");
            return quantity;
        }

        //Insert-Member
        public async Task<ViewDetailMember> Insert(string token, MemberInsertModel model)
        {
            Club club = await _clubRepo.Get(model.ClubId);
            if (club == null) throw new NullReferenceException("Not found this club");
            // check user call this api
            int userId = DecodeToken(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            // check duplicated member
            if (model.StudentId == 0) throw new ArgumentException("StudentId Null");
            bool isMember = await _memberRepo.CheckExistedMemberInClub(model.StudentId, model.ClubId);
            if (isMember) throw new ArgumentException("The user has already in this club");

            Member member = new Member()
            {
                StudentId = model.StudentId,
                JoinDate = DateTime.Now
            };
            int memberId = await _memberRepo.Insert(member);
            if (memberId == 0) throw new DbUpdateException();

            club.TotalMember += 1;

            int termId = await _clubHistoryRepo.GetCurrentTermByClub(model.ClubId);
            ClubHistory record = new ClubHistory()
            {
                ClubId = model.ClubId,
                ClubRoleId = 4, // default role is member
                MemberId = memberId,
                Status = ClubHistoryStatus.Active, // default status
                StartTime = DateTime.Now,
                TermId = termId
            };
            int clubHistoryId = await _clubHistoryRepo.Insert(record);
            if (clubHistoryId == 0) throw new DbUpdateException();

            await _clubRepo.Update();
            return await _memberRepo.GetById(memberId, model.ClubId);
        }

        //Update-Member
        public async Task Update(string token, MemberUpdateModel model)
        {
            if (model.ClubRoleId == 0) throw new ArgumentException("ClubRole Null");
            int userId = DecodeToken(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, model.ClubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            ViewDetailMember member = await _memberRepo.GetById(model.Id, model.ClubId);
            if (member == null) throw new NullReferenceException("Not found this member");

            bool isSuccess = await _clubHistoryRepo.UpdateMemberRole(model.Id, model.ClubRoleId);
            if (!isSuccess) throw new NullReferenceException("Not found this member in the club");
        }

        public async Task Delete(string token, int clubId, int id)
        {
            ViewDetailMember member = await _memberRepo.GetById(id, clubId);
            if (member == null) throw new NullReferenceException("Not found this member");
            int userId = DecodeToken(token, "Id");
            int clubRoleId = await _memberRepo.GetRoleMemberInClub(userId, clubId);
            if (!clubRoleId.Equals(1) && !clubRoleId.Equals(2)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            await _clubHistoryRepo.DeleteMember(id);
            
            Club club = await _clubRepo.Get(clubId);
            club.TotalMember -= 1;
            await _clubRepo.Update();
        }
    }
}
