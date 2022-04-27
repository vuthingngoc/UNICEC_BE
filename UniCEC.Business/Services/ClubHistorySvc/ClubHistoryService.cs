using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Business.Services.ClubHistorySvc
{
    public class ClubHistoryService : IClubHistoryService
    {
        private IClubHistoryRepo _clubHistoryRepo;

        public ClubHistoryService(IClubHistoryRepo clubHistoryRepo)
        {
            _clubHistoryRepo = clubHistoryRepo;
        }

        private ViewClubHistory TransformViewClubHistory(ClubHistory clubHistory)
        {
            return new ViewClubHistory()
            {
                Id = clubHistory.Id,
                ClubId = clubHistory.ClubId,
                ClubRoleId = clubHistory.ClubRoleId,
                MemberId = clubHistory.MemberId,
                TermId = clubHistory.TermId,
                StartTime = clubHistory.StartTime,
                EndTime = clubHistory.EndTime,
                Status = clubHistory.Status
            };
        }

        public async Task<PagingResult<ViewClubHistory>> GetAllPaging(PagingRequest request)
        {
            PagingResult<ClubHistory> listClubHistory = await _clubHistoryRepo.GetAllPaging(request);
            if (listClubHistory == null) throw new NullReferenceException("Not found any previous clubs");
            
            List<ViewClubHistory> items = new List<ViewClubHistory>();
            listClubHistory.Items.ForEach(element =>
            {
                ViewClubHistory clubHistory = TransformViewClubHistory(element);
                items.Add(clubHistory);
            });

            return new PagingResult<ViewClubHistory>(items, listClubHistory.TotalCount, listClubHistory.CurrentPage, listClubHistory.PageSize);
        }

        public async Task<ViewClubHistory> GetByClubHistory(int id)
        {
            ClubHistory clubHistory = await _clubHistoryRepo.Get(id);
            if (clubHistory == null) throw new NullReferenceException("Not found this club history");
            return TransformViewClubHistory(clubHistory);

        }

        public async Task<PagingResult<ViewClubHistory>> GetByContitions(ClubHistoryRequestModel request)
        {
            PagingResult<ClubHistory> listClubHistory = await _clubHistoryRepo.GetByConditions(request);
            if (listClubHistory == null) throw new NullReferenceException("Not found any previous clubs");

            List<ViewClubHistory> items = new List<ViewClubHistory>();
            listClubHistory.Items.ForEach(element =>
            {
                ViewClubHistory clubHistory = TransformViewClubHistory(element);
                items.Add(clubHistory);
            });

            return new PagingResult<ViewClubHistory>(items, listClubHistory.TotalCount, listClubHistory.CurrentPage, listClubHistory.PageSize);            
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
            clubHistoryObject.Id = id;
            return TransformViewClubHistory(clubHistoryObject);
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
    }
}
