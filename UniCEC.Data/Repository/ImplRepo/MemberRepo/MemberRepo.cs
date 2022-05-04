﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Member;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public class MemberRepo : Repository<Member>, IMemberRepo
    {
        public MemberRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckExistedMemberInClub(int userId, int clubId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.StudentId equals u.Id
                        join ch in context.ClubHistories on m.Id equals ch.MemberId
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId) && u.UserId.Equals(userId)
                                && t.CreateTime.Date <= DateTime.Today && t.EndTime.Date >= DateTime.Today
                        select m.Id;

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
                        select new {ch, m, u, cr};

            List<ViewMember> members = await query.Take(3).Select(x => new ViewMember()
            {
                Id = x.m.Id,
                Name = x.u.Fullname,
                ClubRoleId = x.ch.ClubRoleId,
                ClubRoleName = x.cr.Name,
                Avatar = x.u.Avatar,
                Status = x.u.Status
            }).ToListAsync();
            
            return (members.Count > 0) ? members : null;
        }

        public async Task<int> GetQuantityNewMembersByClub(int clubId)
        {
            var club = await context.ClubHistories.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId));
            if (club == null) return -1;

            var query = from ch in context.ClubHistories
                        where ch.ClubId.Equals(clubId) 
                                && ch.StartTime.Month.Equals(DateTime.Now.Month) 
                                && ch.StartTime.Year.Equals(DateTime.Now.Year)
                        select new { ch };

            return await query.CountAsync();
        }
    }
}
