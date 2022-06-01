using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Member;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public class MemberRepo : Repository<Member>, IMemberRepo
    {
        public MemberRepo(UniCECContext context) : base(context)
        {

        }

        //private DateTime GetJoinDate(int memberId, )

        public async Task<PagingResult<ViewMember>> GetMembersByClub(int clubId, int? termId, MemberStatus? status, PagingRequest request)
        {
            var query = from m in context.Members
                        join t in context.Terms on m.TermId equals t.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id                        
                        join u in context.Users on m.UserId equals u.Id
                        where m.ClubId.Equals(clubId)
                        select new { cr, m, u, t };

            if(termId.HasValue) query = query.Where(selector => selector.m.TermId.Equals(termId.Value));



            int totalCount = query.Count();
            List<ViewMember> members = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(selector => new ViewMember()
                                                    {
                                                        Id = selector.m.Id,
                                                        Name = selector.u.Fullname,
                                                        Avatar = selector.u.Avatar,
                                                        ClubRoleId = selector.cr.Id,
                                                        ClubRoleName = selector.cr.Name,
                                                        IsOnline = selector.u.IsOnline,
                                                        TermId = selector.t.Id,
                                                        TermName = selector.t.Name,
                                                        
                                                    }).ToListAsync();

            return (totalCount > 0) ? new PagingResult<ViewMember>(members, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewDetailMember> GetById(int memberId, int clubId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where m.Id.Equals(memberId) && m.Status.Equals(MemberStatus.Active)
                                && m.ClubId.Equals(clubId)
                        select new { m, u, cr};

            ViewDetailMember member = await query.Select(selector => new ViewDetailMember()
            {
                Id = memberId,
                Name = selector.u.Fullname,
                Avatar = selector.u.Avatar,
                ClubRoleId = selector.cr.Id,
                ClubRoleName = selector.cr.Name,
                Email = selector.u.Email,
                //JoinDate = selector.m.JoinDate,
                PhoneNumber = selector.u.PhoneNumber,
                IsOnline = selector.u.IsOnline
            }).FirstOrDefaultAsync();

            return (query.Count() > 0) ? member : null;
        }

        public async Task<bool> CheckExistedMemberInClub(int userId, int clubId)
        {
            var query = from m in context.Members
                        where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                                && m.Status == MemberStatus.Active
                        select m.Id;

            int memberId = await query.FirstOrDefaultAsync();
            return (memberId > 0) ? true : false;
        }

        public async Task<List<ViewMember>> GetLeadersByClub(int clubId)
        {
            var query = from m in context.Members                       
                        join u in context.Users on m.UserId equals u.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where m.ClubId.Equals(clubId)
                                && (m.ClubRoleId == 1 || m.ClubRoleId == 2 || m.ClubRoleId == 3) // Leader, Vice President, Manager
                        select new { m, u, cr };

            List<ViewMember> members = await query.Take(3).Select(x => new ViewMember()
            {
                Id = x.m.Id,
                Name = x.u.Fullname,
                ClubRoleId = x.m.ClubRoleId,
                ClubRoleName = x.cr.Name,
                Avatar = x.u.Avatar,
                IsOnline = x.u.IsOnline
            }).ToListAsync();

            return (members.Count > 0) ? members : null;
        }

        public async Task<int> GetQuantityNewMembersByClub(int clubId)
        {
            var club = await context.Clubs.FirstOrDefaultAsync(c => c.Id.Equals(clubId));
            if (club == null) return -1;

            var query = from m in context.Members
                        where m.ClubId.Equals(clubId) && m.Status.Equals(MemberStatus.Active)
                                && m.StartTime.Month.Equals(DateTime.Now.Month)
                                && m.StartTime.Year.Equals(DateTime.Now.Year)
                        select new { m };

            return await query.CountAsync();
        }

        public async Task<int> GetRoleMemberInClub(int userId, int clubId)
        {
            var query = from m in context.Members
                        where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                                && m.Status.Equals(MemberStatus.Active)
                        select m.ClubRoleId;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> GetRoleMemberInClub(int memberId)
        {
            var query = from m in context.Members 
                        where m.Id.Equals(memberId) && m.Status.Equals(MemberStatus.Active)
                        select m.ClubRoleId;

            return await query.FirstOrDefaultAsync();
        }

        

        //public async Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId)
        //{
        //    ClubHistory clubHistory = await context.ClubHistories.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId)
        //                                                        && x.ClubRoleId.Equals(clubRoleId)
        //                                                        && x.MemberId.Equals(memberId)
        //                                                        && x.TermId.Equals(termId));
        //    return (clubHistory != null) ? clubHistory.Id : 0;
        //}

        //public async Task<PagingResult<ViewClubHistory>> GetByConditions(int clubId, ClubHistoryRequestModel request)
        //{
        //    var query = from ch in context.ClubHistories
        //                join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
        //                join c in context.Clubs on ch.ClubId equals c.Id
        //                join t in context.Terms on ch.TermId equals t.Id
        //                where ch.ClubId.Equals(clubId)
        //                select new { ch, cr, c, t };

        //    if (request.ClubRoleId.HasValue) query = query.Where(x => x.ch.ClubRoleId.Equals(request.ClubRoleId));

        //    if (request.MemberId.HasValue) query = query.Where(x => x.ch.MemberId.Equals(request.MemberId));

        //    if (request.StartTime.HasValue) query = query.Where(x => x.ch.StartTime.Equals(request.StartTime));

        //    if (request.EndTime.HasValue) query = query.Where(x => x.ch.EndTime.Equals(request.EndTime));

        //    if (request.TermId.HasValue) query = query.Where(x => x.ch.TermId.Equals(request.TermId));

        //    if (request.Status.HasValue) query = query.Where(x => x.ch.Status.Equals(request.Status));

        //    int totalCount = query.Count();
        //    List<ViewClubHistory> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
        //                                             .Select(x => new ViewClubHistory()
        //                                             {
        //                                                 Id = x.ch.Id,
        //                                                 ClubId = x.ch.ClubId,
        //                                                 ClubName = x.c.Name,
        //                                                 ClubRoleId = x.ch.ClubRoleId,
        //                                                 ClubRoleName = x.cr.Name,
        //                                                 MemberId = x.ch.MemberId,
        //                                                 TermId = x.ch.TermId,
        //                                                 TermName = x.t.Name,
        //                                                 StartTime = x.ch.StartTime,
        //                                                 EndTime = x.ch.EndTime,
        //                                                 Status = x.ch.Status
        //                                             }).ToListAsync();

        //    return (items.Count > 0) ? new PagingResult<ViewClubHistory>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        //}

        public async Task<bool> CheckMemberInClub(List<int> List_ClubId_In_Competition, User studentInfo, int termId)
        {
            bool result = false;
            //tìm user có là member trong 1 cuộc thi được tổ chức bởi nhiều Club
            // User -> Member -> ClubHistory -> Club 
            foreach (int ClubId_In_Competition in List_ClubId_In_Competition)
            {
                var query = from us in context.Users
                            where us.Id == studentInfo.Id
                            from mem in context.Members
                            where mem.UserId == us.Id
                            //from ch in context.ClubHistories
                            //where mem.Id == ch.MemberId && termId == ch.TermId && ch.Status == MemberStatus.Active
                            from c in context.Clubs
                            where mem.ClubId == c.Id && c.Id == ClubId_In_Competition
                            select us;
                //
                User student = await query.FirstOrDefaultAsync();
                //
                if (student != null)
                {
                    return true;
                }
            }
            return result;
        }

        //user ID ở đây kh phải là MSSV
        //public async Task<ViewClubMember> GetMemberInCLub(GetMemberInClubModel model)
        //{
        //    var query = from us in context.Users
        //                where us.Id == model.UserId
        //                from me in context.Members
        //                where me.StudentId == us.Id
        //                from ch in context.ClubHistories
        //                where ch.ClubId == model.ClubId && ch.TermId == model.TermId && me.Id == ch.MemberId && ch.Status == MemberStatus.Active
        //                select new { ch, us, me };


        //    List<ViewClubMember> viewClubMembers = await query.Select(x => new ViewClubMember()
        //    {
        //        Name = x.us.Fullname,
        //        ClubRoleName = x.ch.ClubRole.Name,
        //        MemberId = x.me.Id,
        //        TermId = x.ch.TermId

        //    }).ToListAsync();

        //    if (viewClubMembers.Count > 0)
        //    {
        //        return viewClubMembers[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public async Task<bool> UpdateMemberRole(int memberId, int clubRoleId)
        //{
        //    ClubHistory record = await (from ch in context.ClubHistories
        //                                where ch.MemberId.Equals(memberId) && ch.Status.Equals(MemberStatus.Active)
        //                                select ch).FirstOrDefaultAsync();

        //    if (record == null) return false;

        //    record.EndTime = DateTime.Now;
        //    record.Status = MemberStatus.Inactive;
        //    await Update();

        //    ClubHistory newRecord = new ClubHistory()
        //    {
        //        ClubId = record.ClubId,
        //        ClubRoleId = clubRoleId, // new role
        //        MemberId = memberId,
        //        StartTime = DateTime.Now,
        //        Status = MemberStatus.Active, // default status
        //        TermId = record.TermId
        //    };

        //    return await Insert(newRecord) > 0;
        //}

        //public async Task DeleteMember(int memberId)
        //{
        //    ClubHistory record = await (from ch in context.ClubHistories
        //                                where ch.MemberId.Equals(memberId) && ch.Status.Equals(MemberStatus.Active)
        //                                select ch).FirstOrDefaultAsync();

        //    if (record != null)
        //    {
        //        record.EndTime = DateTime.Now;
        //        record.Status = MemberStatus.Inactive;
        //        await Update();
        //    }
        //}

        //public async Task UpdateEndTerm(int clubId)
        //{
        //    (from ch in context.ClubHistories
        //     where ch.ClubId.Equals(clubId) && ch.Status.Equals(MemberStatus.Active)
        //     select ch).ToList().ForEach(record =>
        //     {
        //         record.EndTime = DateTime.Now;
        //         record.Status = MemberStatus.Inactive;
        //     });

        //    await Update();
        //}

        //public async Task<List<ClubHistory>> GetCurrentHistoryByClub(int clubId)
        //{
        //    var query = from ch in context.ClubHistories
        //                where ch.ClubId.Equals(clubId) && ch.Status.Equals(MemberStatus.Active)
        //                select ch;

        //    int totalCount = query.Count();
        //    List<ClubHistory> items = await query.Select(ch => new ClubHistory()
        //    {
        //        ClubId = ch.ClubId,
        //        ClubRoleId = ch.ClubRoleId,
        //        MemberId = ch.MemberId,
        //        StartTime = ch.StartTime,
        //        EndTime = ch.EndTime,
        //        TermId = ch.TermId,
        //        Status = ch.Status
        //    }).ToListAsync();

        //    return (items.Count > 0) ? items : null;
        //}
    }
}
