using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Entities.ClubHistory;
using UniCEC.Data.Enum;
using System;

namespace UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo
{
    public class ClubHistoryRepo : Repository<ClubHistory>, IClubHistoryRepo
    {
        public ClubHistoryRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<ViewClubHistory>> GetAll(int clubId, PagingRequest request)
        {
            var query = from ch in context.ClubHistories
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        join c in context.Clubs on ch.ClubId equals c.Id
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId)
                        select new { ch, cr, c, t };

            int totalCount = query.Count();
            List<ViewClubHistory> clubHistories = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                       .Select(x => new ViewClubHistory()
                                                       {
                                                           Id = x.ch.Id,
                                                           ClubId = x.ch.ClubId,
                                                           ClubName = x.c.Name,
                                                           ClubRoleId = x.ch.ClubRoleId,
                                                           ClubRoleName = x.cr.Name,
                                                           MemberId = x.ch.MemberId,
                                                           TermId = x.ch.TermId,
                                                           TermName = x.t.Name,
                                                           StartTime = x.ch.StartTime,
                                                           EndTime = x.ch.EndTime,
                                                           Status = x.ch.Status
                                                       }).ToListAsync();

            return (clubHistories.Count() > 0) ? new PagingResult<ViewClubHistory>(clubHistories, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewClubHistory> GetById(int id)
        {
            var query = from ch in context.ClubHistories
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        join c in context.Clubs on ch.ClubId equals c.Id
                        join t in context.Terms on ch.TermId equals t.Id
                        select new { ch, cr, c, t };

            int totalCount = query.Count();
            ViewClubHistory clubHistory = await query.Select(x => new ViewClubHistory()
            {
                Id = x.ch.Id,
                ClubId = x.ch.ClubId,
                ClubName = x.c.Name,
                ClubRoleId = x.ch.ClubRoleId,
                ClubRoleName = x.cr.Name,
                MemberId = x.ch.MemberId,
                TermId = x.ch.TermId,
                TermName = x.t.Name,
                StartTime = x.ch.StartTime,
                EndTime = x.ch.EndTime,
                Status = x.ch.Status
            }).FirstOrDefaultAsync();

            return (totalCount > 0) ? clubHistory : null;
        }

        public async Task<int> GetCurrentTermByClub(int clubId)
        {
            return await (from ch in context.ClubHistories
                          where ch.ClubId.Equals(clubId) && ch.Status == Enum.ClubHistoryStatus.Active
                          select ch.TermId).FirstOrDefaultAsync();
        }

        public async Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId)
        {
            ClubHistory clubHistory = await context.ClubHistories.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId)
                                                                && x.ClubRoleId.Equals(clubRoleId)
                                                                && x.MemberId.Equals(memberId)
                                                                && x.TermId.Equals(termId));
            return (clubHistory != null) ? clubHistory.Id : 0;
        }

        public async Task<PagingResult<ViewClubHistory>> GetByConditions(int clubId, ClubHistoryRequestModel request)
        {
            var query = from ch in context.ClubHistories
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        join c in context.Clubs on ch.ClubId equals c.Id
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId)
                        select new { ch, cr, c, t };

            if (request.ClubRoleId.HasValue) query = query.Where(x => x.ch.ClubRoleId.Equals(request.ClubRoleId));

            if (request.MemberId.HasValue) query = query.Where(x => x.ch.MemberId.Equals(request.MemberId));

            if (request.StartTime.HasValue) query = query.Where(x => x.ch.StartTime.Equals(request.StartTime));

            if (request.EndTime.HasValue) query = query.Where(x => x.ch.EndTime.Equals(request.EndTime));

            if (request.TermId.HasValue) query = query.Where(x => x.ch.TermId.Equals(request.TermId));

            if (request.Status.HasValue) query = query.Where(x => x.ch.Status.Equals(request.Status));

            int totalCount = query.Count();
            List<ViewClubHistory> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                     .Select(x => new ViewClubHistory()
                                                     {
                                                         Id = x.ch.Id,
                                                         ClubId = x.ch.ClubId,
                                                         ClubName = x.c.Name,
                                                         ClubRoleId = x.ch.ClubRoleId,
                                                         ClubRoleName = x.cr.Name,
                                                         MemberId = x.ch.MemberId,
                                                         TermId = x.ch.TermId,
                                                         TermName = x.t.Name,
                                                         StartTime = x.ch.StartTime,
                                                         EndTime = x.ch.EndTime,
                                                         Status = x.ch.Status
                                                     }).ToListAsync();

            return (items.Count > 0) ? new PagingResult<ViewClubHistory>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request)
        {
            var query = from ch in context.ClubHistories
                        join m in context.Members on ch.MemberId equals m.Id
                        join u in context.Users on m.StudentId equals u.Id
                        join r in context.ClubRoles on ch.ClubRoleId equals r.Id
                        where ch.ClubId.Equals(clubId) && ch.TermId.Equals(termId)
                        select new { ch, u, r };

            int totalCount = query.Count();
            List<ViewClubMember> clubMembers = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                          .Select(x => new ViewClubMember()
                                                          {
                                                              MemberId = x.ch.MemberId,
                                                              ClubRoleName = x.r.Name,
                                                              Name = x.u.Fullname
                                                          }).ToListAsync();

            return (clubMembers.Count > 0) ? new PagingResult<ViewClubMember>(clubMembers, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<bool> CheckMemberInClub(int clubId, int memberId, int termId)
        {
            //club thì phải có trong club id và kì luôn 
            var query = from cp in context.ClubHistories
                        where cp.MemberId == memberId && cp.ClubId == clubId && cp.TermId == termId && cp.Status == ClubHistoryStatus.Active
                        select cp;
            //
            int check = query.Count();
            if (check > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //user ID ở đây kh phải là MSSV
        public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        {
            var query = from us in context.Users
                        where us.Id == model.UserId
                        from me in context.Members
                        where me.StudentId == us.Id
                        from ch in context.ClubHistories
                        where ch.ClubId == model.ClubId && ch.TermId == model.TermId && me.Id == ch.MemberId && ch.Status == ClubHistoryStatus.Active
                        select new { ch, us, me };


            List<ViewClubMember> viewClubMembers = await query.Select(x => new ViewClubMember()
            {
                Name = x.us.Fullname,
                ClubRoleName = x.ch.ClubRole.Name,
                MemberId = x.me.Id,
                TermId = x.ch.TermId

            }).ToListAsync();

            if (viewClubMembers.Count > 0)
            {
                return viewClubMembers[0];
            }
            else
            {
                return null;
            }
            //ClubHistory clubHistory = await query.FirstOrDefaultAsync();
            //if (clubHistory != null)
            //{
            //    return new ViewClubMember()
            //    {
            //        Name = clubHistory.Member.Student.Fullname,
            //        ClubRoleName = clubHistory.ClubRole.Name,
            //        MemberId = clubHistory.MemberId,
            //        TermId = clubHistory.TermId
            //    };
            //}
        }

        public async Task<bool> UpdateMemberRole(int memberId, int clubRoleId)
        {
            ClubHistory record = await (from ch in context.ClubHistories
                                        where ch.MemberId.Equals(memberId) && ch.Status.Equals(ClubHistoryStatus.Active)
                                        select ch).FirstOrDefaultAsync();

            if (record == null) return false;

            record.EndTime = DateTime.Now;
            record.Status = ClubHistoryStatus.Inactive;
            await Update();

            ClubHistory newRecord = new ClubHistory()
            {
                ClubId = record.ClubId,
                ClubRoleId = clubRoleId, // new role
                MemberId = memberId,
                StartTime = DateTime.Now,
                Status = ClubHistoryStatus.Active, // default status
                TermId = record.TermId
            };

            return await Insert(newRecord) > 0;
        }

        public async Task<int> DeleteMember(int memberId)
        {
            ClubHistory record = await (from ch in context.ClubHistories
                                        where ch.MemberId.Equals(memberId) && ch.Status.Equals(ClubHistoryStatus.Active)
                                        select ch).FirstOrDefaultAsync();

            if (record == null) return 0;

            record.EndTime = DateTime.Now;
            record.Status = ClubHistoryStatus.Inactive;
            await Update();

            return record.ClubId;
        }

        public async Task UpdateEndTerm(int clubId)
        {
            (from ch in context.ClubHistories
             where ch.ClubId.Equals(clubId) && ch.Status.Equals(ClubHistoryStatus.Active)
             select ch).ToList().ForEach(record =>
             {
                 record.EndTime = DateTime.Now;
                 record.Status = ClubHistoryStatus.Inactive;
             });

            await Update();
        }

        public async Task<List<ClubHistory>> GetCurrentHistoryByClub(int clubId)
        {
            var query = from ch in context.ClubHistories
                        where ch.ClubId.Equals(clubId) && ch.Status.Equals(ClubHistoryStatus.Active)
                        select ch;

            int totalCount = query.Count();
            List<ClubHistory> items = await query.Select(ch => new ClubHistory()
            {
                ClubId = ch.ClubId,
                ClubRoleId = ch.ClubRoleId,
                MemberId = ch.MemberId,
                StartTime = ch.StartTime,
                EndTime = ch.EndTime,
                TermId = ch.TermId,
                Status = ch.Status
            }).ToListAsync();

            return (items.Count > 0) ? items : null;
        }
    }
}
