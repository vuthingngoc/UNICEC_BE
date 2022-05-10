using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public MemberService(IMemberRepo memberRepo, IClubRepo clubRepo, IClubHistoryRepo clubHistoryRepo)
        {
            _memberRepo = memberRepo;
            _clubRepo = clubRepo;
            _clubHistoryRepo = clubHistoryRepo;
        }

        public Task<PagingResult<ViewMember>> GetAllPaging(int clubId, PagingRequest request)
        {

            throw new NotImplementedException();
        }

        public async Task<ViewMember> GetByMemberId(int id)
        {
            ViewMember member = await _memberRepo.GetById(id);
            if (member == null) throw new NullReferenceException("Not found this member");
            return member;
        }

        public async Task<List<ViewMember>> GetLeadersByClub(int clubId)
        {
            List<ViewMember> members = await _memberRepo.GetLeadersByClub(clubId);
            if (members == null) throw new NullReferenceException("Not found any Leaders");
            return members;
        }

        public async Task<int> GetQuantityNewMembersByClub(int clubId)
        {
            int quantity = await _memberRepo.GetQuantityNewMembersByClub(clubId);
            if (quantity < 0) throw new NullReferenceException("Not found this club");
            return quantity;
        }

        //Insert-Member
        public async Task<ViewMember> Insert(MemberInsertModel model)
        {
            Club club = await _clubRepo.Get(model.ClubId);
            if (club == null) throw new NullReferenceException("Not found this club");
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
            return await _memberRepo.GetById(memberId);
        }

        //Update-Member
        public async Task Update(MemberUpdateModel model)
        {
            Member member = await _memberRepo.Get(model.Id);
            if (member == null) throw new NullReferenceException("Not found this member");

            bool success = await _clubHistoryRepo.UpdateMemberRole(model.Id, model.ClubRoleId);
            if (!success) throw new NullReferenceException("Not found this record in history");
        }

        public async Task Delete(int id)
        {
            Member member = await _memberRepo.Get(id);
            if (member == null) throw new NullReferenceException("Not found this member");

            int clubId = await _clubHistoryRepo.DeleteMember(id);
            if (clubId == 0) throw new NullReferenceException("Not found this record in history");

            Club club = await _clubRepo.Get(clubId);
            club.TotalMember -= 1;
            await _clubRepo.Update();
        }
    }
}
