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

        public async Task<PagingResult<ViewMember>> GetAllMemberByClub(int clubId, PagingRequest request)
        {
            var query = from ch in context.ClubHistories
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        join m in context.Members on ch.MemberId equals m.Id
                        join u in context.Users on m.StudentId equals u.Id
                        where ch.ClubId.Equals(clubId) && ch.Status.Equals(ClubHistoryStatus.Active)
                        select new { ch, cr, m, u };

            int totalCount = query.Count();
            List<ViewMember> members = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(x => new ViewMember()
                                                    {
                                                        Id = x.m.Id,
                                                        Name = x.u.Fullname,
                                                        Avatar = x.u.Avatar,
                                                        ClubRoleId = x.cr.Id,
                                                        ClubRoleName = x.cr.Name,
                                                        IsOnline = x.u.IsOnline
                                                    }).ToListAsync();

            return (totalCount > 0) ? new PagingResult<ViewMember>(members, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewMember> GetById(int memberId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.StudentId equals u.Id
                        join ch in context.ClubHistories on m.Id equals ch.MemberId
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        where m.Id.Equals(memberId) && ch.Status.Equals(ClubHistoryStatus.Active)
                        select new { m, u, cr, ch };

            ViewMember member = await query.Select(x => new ViewMember()
            {
                Id = memberId,
                Name = x.u.Fullname,
                Avatar = x.u.Avatar,
                ClubRoleId = x.cr.Id,
                ClubRoleName = x.cr.Name,
                IsOnline = x.u.IsOnline
            }).FirstOrDefaultAsync();

            return (query.Count() > 0) ? member : null;
        }

        public async Task<bool> CheckExistedMemberInClub(int userId, int clubId)
        {
            var query = from m in context.Members
                        join ch in context.ClubHistories on m.Id equals ch.MemberId
                        where m.StudentId.Equals(userId) && ch.ClubId.Equals(clubId)
                        select ch.MemberId;

            int memberId = await query.FirstOrDefaultAsync();
            return (memberId > 0) ? true : false;
        }

        public async Task<List<ViewMember>> GetLeadersByClub(int clubId)
        {
            var query = from ch in context.ClubHistories
                        join m in context.Members on ch.MemberId equals m.Id
                        join u in context.Users on m.StudentId equals u.Id
                        join cr in context.ClubRoles on ch.ClubRoleId equals cr.Id
                        where ch.ClubId.Equals(clubId)
                                && (ch.ClubRoleId == 1 || ch.ClubRoleId == 2 || ch.ClubRoleId == 3) // Leader, Vice President, Manager
                        select new { ch, m, u, cr };

            List<ViewMember> members = await query.Take(3).Select(x => new ViewMember()
            {
                Id = x.m.Id,
                Name = x.u.Fullname,
                ClubRoleId = x.ch.ClubRoleId,
                ClubRoleName = x.cr.Name,
                Avatar = x.u.Avatar,
                IsOnline = x.u.IsOnline
            }).ToListAsync();

            return (members.Count > 0) ? members : null;
        }

        public async Task<int> GetQuantityNewMembersByClub(int clubId)
        {
            var club = await context.ClubHistories.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId));
            if (club == null) return -1;

            var query = from ch in context.ClubHistories
                        where ch.ClubId.Equals(clubId) && ch.Status.Equals(ClubHistoryStatus.Active)
                                && ch.StartTime.Month.Equals(DateTime.Now.Month)
                                && ch.StartTime.Year.Equals(DateTime.Now.Year)
                        select new { ch };

            return await query.CountAsync();
        }
    }
}
