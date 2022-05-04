using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public class ClubHistoryService : IClubHistoryService // Test ...
    {
        private IClubHistoryRepo _clubHistoryRepo;
        private IMemberRepo _memberRepo;

        public ClubHistoryService(IClubHistoryRepo clubHistoryRepo, IMemberRepo memberRepo)
        {
            _clubHistoryRepo = clubHistoryRepo;
            _memberRepo = memberRepo;   
        }

        public async Task<PagingResult<ViewClubHistory>> GetAllPaging(int clubId, string token, PagingRequest request)
        {
            // Test ...
            var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jsonToken.Claims.FirstOrDefault(x => x.Equals("Id"));
            var userId = Int32.Parse(claim.Value);
            bool isMember = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            if (isMember == false) return null;

            PagingResult<ViewClubHistory> clubHistories = await _clubHistoryRepo.GetAll(clubId, request);
            if (clubHistories == null) throw new NullReferenceException("Not found any previous clubs");
            return clubHistories;
        }

        public async Task<ViewClubHistory> GetByClubHistory(int id)
        {
            ViewClubHistory clubHistory = await _clubHistoryRepo.GetById(id);
            if (clubHistory == null) throw new NullReferenceException("Not found this club history");
            return clubHistory;
        }

        public async Task<PagingResult<ViewClubHistory>> GetByContitions(ClubHistoryRequestModel request)
        {
            PagingResult<ViewClubHistory> ClubHistories = await _clubHistoryRepo.GetByConditions(request);
            if (ClubHistories == null) throw new NullReferenceException("Not found any previous clubs");
            return ClubHistories;
        }

        public async Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request)
        {
            PagingResult<ViewClubMember> clubMembers = await _clubHistoryRepo.GetMembersByClub(clubId, termId, request);
            if (clubMembers == null) throw new NullReferenceException("This club has no any members in this term");
            return clubMembers;
        }

        public async Task<ViewClubHistory> Insert(ClubHistoryInsertModel clubHistory)
        {
            if (clubHistory.ClubRoleId == 0 || clubHistory.ClubId == 0 || clubHistory.TermId == 0 
                || clubHistory.MemberId == 0 || clubHistory.StartTime == DateTime.Parse("1/1/0001 12:00:00 AM")) 
                    throw new ArgumentNullException("ClubRoleId Null || ClubId Null || TermId Null || MemberId Null || StartTime Null");

            int checkId = await _clubHistoryRepo.CheckDuplicated(clubHistory.ClubId, clubHistory.ClubRoleId, clubHistory.MemberId, clubHistory.TermId);
            if (checkId > 0) throw new ArgumentException("Duplicated record");

            ClubHistory clubHistoryObject = new ClubHistory()
            {
                ClubId = clubHistory.ClubId,
                ClubRoleId = clubHistory.ClubRoleId,
                MemberId = clubHistory.MemberId,
                TermId = clubHistory.TermId,
                StartTime = clubHistory.StartTime,
                EndTime = clubHistory.EndTime,
                Status = ClubHistoryStatus.Active
            };
            int id = await _clubHistoryRepo.Insert(clubHistoryObject);
            return await _clubHistoryRepo.GetById(id);
        }

        public async Task Update(ClubHistoryUpdateModel clubHistory)
        {
            ClubHistory clubHistoryObject = await _clubHistoryRepo.Get(clubHistory.Id);
            if (clubHistoryObject == null) throw new NullReferenceException("Not found this club previous");
            // check duplicated record when change role member
            if(clubHistoryObject.ClubRoleId != clubHistory.ClubRoleId)
            {
                int checkId = await _clubHistoryRepo.CheckDuplicated(clubHistoryObject.ClubId, clubHistory.ClubRoleId, clubHistoryObject.MemberId, clubHistory.TermId);
                if (checkId > 0) throw new ArgumentException("Duplicated record");
            }

            if(clubHistory.ClubRoleId != 0) clubHistoryObject.ClubRoleId = clubHistory.ClubRoleId;
            if(clubHistory.StartTime != DateTime.Parse("1/1/0001 12:00:00 AM")) clubHistoryObject.StartTime = clubHistory.StartTime;
            if(clubHistory.EndTime != null) clubHistoryObject.EndTime = clubHistory.EndTime;
            clubHistoryObject.Status = clubHistory.Status;
            if(clubHistory.TermId != 0) clubHistoryObject.TermId = clubHistory.TermId;            

            await _clubHistoryRepo.Update();
        }

        public async Task Delete(int memberId)
        {
            List<int> clubHistoryIds = await _clubHistoryRepo.GetIdsByMember(memberId);
            if (clubHistoryIds == null) throw new NullReferenceException("Not found this member");
            foreach(int id in clubHistoryIds)
            {
                ClubHistory element = await _clubHistoryRepo.Get(id);
                if (!element.EndTime.HasValue) element.EndTime = DateTime.Now;
                element.Status = ClubHistoryStatus.Inactive;
            }

            await _clubHistoryRepo.Update();
        }

        public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        {
            ViewClubMember result = await _clubHistoryRepo.GetMemberInCLub(model);
            return result;
        }
    }
}
