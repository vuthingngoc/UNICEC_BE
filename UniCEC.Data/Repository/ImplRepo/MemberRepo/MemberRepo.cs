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
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public class MemberRepo : Repository<Member>, IMemberRepo
    {
        public MemberRepo(UniCECContext context) : base(context) { }

        private async Task<DateTime> GetJoinDate(int userId, int clubId)
        {
            return await (from m in context.Members
                          where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                          select m.StartTime).FirstOrDefaultAsync();
        }

        private async Task<DateTime> GetJoinDate(int memberId)
        {
            var member = await Get(memberId);
            return await GetJoinDate(member.UserId, member.ClubId);
        }

        public async Task<PagingResult<ViewMember>> GetMembersByClub(int clubId, MemberStatus status, PagingRequest request)
        {
            var query = from m in context.Members
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        join u in context.Users on m.UserId equals u.Id
                        where m.ClubId.Equals(clubId) && m.Status.Equals(status)
                        orderby (cr.Id)
                        select new { cr, m, u };

            int totalCount = query.Count();
            List<ViewMember> members = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(selector => new ViewMember()
                                                    {
                                                        Id = selector.m.Id,
                                                        StudentId = selector.u.Id, //add
                                                        Name = selector.u.Fullname,
                                                        StudentCode = selector.u.StudentCode,
                                                        Avatar = selector.u.Avatar,
                                                        ClubRoleId = selector.cr.Id,
                                                        ClubRoleName = selector.cr.Name,
                                                        IsOnline = selector.u.IsOnline,
                                                        StartTime = selector.m.StartTime,
                                                        EndTime = selector.m.EndTime,
                                                        Status = selector.m.Status
                                                    }).ToListAsync();

            return (members.Count > 0) ? new PagingResult<ViewMember>(members, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<List<Member>> GetMembersByClub(int clubId, string searchName, int? roleId)
        {
            var query = from m in context.Members
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        join u in context.Users on m.UserId equals u.Id
                        where m.ClubId.Equals(clubId)
                        && m.Status.Equals(MemberStatus.Active)
                        select new { cr, m, u };

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(x => x.u.Fullname.Contains(searchName));
            }

            if (roleId.HasValue)
            {
                query = query.Where(x => x.m.ClubRoleId == roleId);
            }

            List<Member> members = await query.Select(selector => new Member()
            {
                Id = selector.m.Id,
                ClubId = selector.m.ClubId,
                StartTime = selector.m.StartTime,
                EndTime = selector.m.EndTime,
                UserId = selector.m.UserId,
                ClubRoleId = selector.m.ClubRoleId,
            }).ToListAsync();

            return (members.Count > 0) ? members : null;
        }

        public async Task<ViewDetailMember> GetDetailById(int memberId, MemberStatus? status)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where m.Id.Equals(memberId)
                        select new { m, u, c, cr };

            if (status.HasValue) query = query.Where(selector => selector.m.Status.Equals(status.Value));

            ViewDetailMember member = await query.Select(selector => new ViewDetailMember()
            {
                Id = memberId,
                Name = selector.u.Fullname,
                StudentCode = selector.u.StudentCode,
                Avatar = selector.u.Avatar,
                ClubId = selector.m.ClubId,
                ClubName = selector.c.Name,
                ClubRoleId = selector.cr.Id,
                ClubRoleName = selector.cr.Name,
                Email = selector.u.Email,
                JoinDate = selector.m.StartTime,
                PhoneNumber = selector.u.PhoneNumber,
                Status = selector.m.Status,
                IsOnline = selector.u.IsOnline
            }).FirstOrDefaultAsync();

            return (query.Count() > 0) ? member : null;
        }

        public async Task<ViewMember> GetById(int memberId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where m.Id.Equals(memberId)
                        select new { m, u, cr };

            DateTime joinDate = await GetJoinDate(memberId);
            ViewMember member = await query.Select(selector => new ViewMember()
            {
                Id = memberId,
                StudentId = selector.u.Id, //add
                Name = selector.u.Fullname,
                StudentCode = selector.u.StudentCode,
                Avatar = selector.u.Avatar,
                ClubRoleId = selector.cr.Id,
                ClubRoleName = selector.cr.Name,
                StartTime = selector.m.StartTime,
                EndTime = selector.m.EndTime,
                Status = selector.m.Status,
                IsOnline = selector.u.IsOnline
            }).FirstOrDefaultAsync();

            return (query.Count() > 0) ? member : null;
        }

        public async Task<int> GetClubIdByMember(int memberId) // just use to check the condition => no need to check status active
        {
            return await (from m in context.Members
                          where m.Id.Equals(memberId)
                          select m.ClubId).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExistedMemberInClub(int userId, int clubId)
        {
            var query = from m in context.Members
                        where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                                && !m.Status.Equals(MemberStatus.Inactive)
                        select m.Id;

            int memberId = await query.FirstOrDefaultAsync();
            return (memberId > 0) ? true : false;
        }

       
        public async Task<List<ViewIntroClubMember>> GetLeadersByClub(int clubId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where m.ClubId.Equals(clubId) && m.Status.Equals(MemberStatus.Active)
                        orderby (cr.Id) // Leader -> Vice President -> Manager -> member
                        select new { m, u, cr }; // if the club don't have enough 3 leaders => replace by member            

            List<ViewIntroClubMember> members = await query.Take(3).Select(x => new ViewIntroClubMember()
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
                        select m;

            return await query.CountAsync();
        }

        public async Task<int> GetTotalMembersByClub(int clubId)
        {
            var club = await context.Clubs.FirstOrDefaultAsync(c => c.Id.Equals(clubId));
            if (club == null) return -1;

            var query = from m in context.Members
                        where m.ClubId.Equals(clubId) && m.Status.Equals(MemberStatus.Active)
                        select m;

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

        public async Task<int> GetIdByUser(int userId, int clubId)
        {
            return await (from m in context.Members
                          where m.UserId.Equals(userId) && m.ClubId.Equals(clubId) && m.Status == MemberStatus.Active
                          select m.Id).FirstOrDefaultAsync();
        }

        public async Task<int> CheckDuplicated(int clubId, int clubRoleId, int userId, int termId)
        {
            Member member = await context.Members.FirstOrDefaultAsync(m => m.ClubId.Equals(clubId)
                                                                && m.ClubRoleId.Equals(clubRoleId)
                                                                && m.UserId.Equals(userId));
            return (member != null) ? member.Id : 0;
        }

        public async Task<PagingResult<ViewMember>> GetByConditions(MemberRequestModel request)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        where m.ClubId.Equals(request.ClubId)
                        select new { m, u, cr, c };

            if (request.ClubRoleId.HasValue) query = query.Where(selector => selector.m.ClubRoleId.Equals(request.ClubRoleId));

            if (!string.IsNullOrEmpty(request.SearchString)) query = query.Where(selector => selector.u.Fullname.Contains(request.SearchString)
                                                                                         || selector.u.StudentCode.Contains(request.SearchString));

            if (request.StartTime.HasValue) query = query.Where(selector => selector.m.StartTime.Year.Equals(request.StartTime.Value.Year)
                                                                            && selector.m.StartTime.Month.Equals(request.StartTime.Value.Month)
                                                                            && selector.m.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.EndTime.HasValue) query = query.Where(selector => selector.m.EndTime.Value.Year.Equals(request.EndTime.Value.Year)
                                                                          && selector.m.EndTime.Value.Month.Equals(request.EndTime.Value.Month)
                                                                          && selector.m.EndTime.Value.Day.Equals(request.EndTime.Value.Day));

            if (request.Status.HasValue) query = query.Where(selector => selector.m.Status.Equals(request.Status));

            int totalCount = query.Count();
            List<ViewMember> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                     .Select(x => new ViewMember()
                                                     {
                                                         Id = x.m.Id,
                                                         StudentId = x.u.Id, //add
                                                         Name = x.u.Fullname,
                                                         StudentCode = x.u.StudentCode,
                                                         ClubRoleId = x.m.ClubRoleId,
                                                         ClubRoleName = x.cr.Name,
                                                         StartTime = x.m.StartTime,
                                                         EndTime = x.m.EndTime,
                                                         Status = x.m.Status,
                                                         Avatar = x.u.Avatar,
                                                         IsOnline = x.u.IsOnline
                                                     }).ToListAsync();

            return (items.Count > 0) ? new PagingResult<ViewMember>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //TA
        public async Task<ViewBasicInfoMember> GetBasicInfoMember(GetMemberInClubModel model)
        {
            var query = from m in context.Members
                        join us in context.Users on m.UserId equals us.Id
                        where m.ClubId == model.ClubId && m.UserId == model.UserId && m.Status == MemberStatus.Active
                        select new { us, m };


            return await query.Select(x => new ViewBasicInfoMember()
            {
                Name = x.us.Fullname,
                ClubRoleName = x.m.ClubRole.Name,
                ClubRoleId = x.m.ClubRoleId,
                Id = x.m.Id,
                UserId = x.us.Id,
            }).FirstOrDefaultAsync();
        }
        public async Task<Member> IsMemberInListClubCompetition(List<int> List_ClubId_In_Competition, User studentInfo)
        {

            //tìm user có là member trong 1 cuộc thi được tổ chức bởi nhiều Club
            // User -> Member -> Club 
            foreach (int ClubId_In_Competition in List_ClubId_In_Competition)
            {
                var query = from us in context.Users
                            where us.Id == studentInfo.Id
                            from mem in context.Members
                            where mem.UserId == us.Id && mem.Status == MemberStatus.Active
                            from c in context.Clubs
                            where mem.ClubId == c.Id && c.Id == ClubId_In_Competition
                            select mem;

                Member member = await query.FirstOrDefaultAsync();

                if (member != null)
                {
                    return member;
                }
            }
            return null;
        }

        public async Task<Member> GetLeaderByClub(int clubId)
        {
            //current club leader 
            return await (from m in context.Members
                          where m.ClubId == clubId && m.ClubRoleId == 1 && m.Status == MemberStatus.Active
                          select m).FirstOrDefaultAsync();
        }

        public async Task UpdateMemberRole(int memberId, int clubRoleId)
        {
            Member member = await (from m in context.Members
                                   where m.Id.Equals(memberId) && m.Status.Equals(MemberStatus.Active)
                                   select m).FirstOrDefaultAsync();

            if (member == null) throw new NullReferenceException("Not found this member in club");

            member.EndTime = DateTime.Now;
            member.Status = MemberStatus.Inactive;
            await Update();

            // add new record
            Member newRecord = new Member()
            {
                ClubId = clubRoleId,
                ClubRoleId = member.ClubRoleId,
                StartTime = DateTime.Now,
                UserId = member.UserId,
                Status = MemberStatus.Active, // default status new record                
            };
            await Insert(newRecord);
        }

        //public async Task DeleteMember(int memberId)
        //{
        //    Member record = await (from m in context.Members
        //                           where m.Id.Equals(memberId) && m.Status.Equals(MemberStatus.Active)
        //                           select m).FirstOrDefaultAsync();

        //    if (record != null)
        //    {
        //        record.EndTime = DateTime.Now;
        //        record.Status = MemberStatus.Inactive;
        //        await Update();
        //    }
        //}

        public async Task UpdateStatusDeletedClub(int clubId)
        {
            (from m in context.Members
             where m.ClubId.Equals(clubId) && m.Status.Equals(MemberStatus.Active)
             select m).ToList().ForEach(record =>
             {
                 record.EndTime = DateTime.Now;
                 record.Status = MemberStatus.Inactive;
             });

            await Update();
        }

        // use when insert club
        public int CheckValidLeader(int userId, int universityId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        where m.UserId.Equals(userId) && u.UniversityId.Equals(universityId)
                        select m;

            if (!query.Any()) return -1;

            query = query.Where(member => member.Status.Equals(MemberStatus.Active) && member.ClubRoleId.Equals(1)); // club role leader

            return (query.Any()) ? 1 : 0;
        }

        public async Task<List<ViewDetailMember>> GetMemberInfoByClub(int userId, int? clubId)
        {
            var query = from m in context.Members
                        join u in context.Users on m.UserId equals u.Id
                        join c in context.Clubs on m.ClubId equals c.Id
                        join cr in context.ClubRoles on m.ClubRoleId equals cr.Id
                        where (m.Status.Equals(MemberStatus.Active) || m.Status.Equals(MemberStatus.Pending))
                                && m.UserId.Equals(userId)
                        select new { m, u, c, cr };

            if (clubId.HasValue) query = query.Where(selector => selector.m.ClubId.Equals(clubId.Value));

            List<ViewDetailMember> members = await query.Select(selector => new ViewDetailMember()
            {
                Id = selector.m.Id,
                Name = selector.u.Fullname,
                StudentCode = selector.u.StudentCode,
                Avatar = selector.u.Avatar,
                ClubId = selector.m.ClubId,
                ClubName = selector.c.Name,
                ClubRoleId = selector.cr.Id,
                ClubRoleName = selector.cr.Name,
                Email = selector.u.Email,
                JoinDate = selector.m.StartTime,
                PhoneNumber = selector.u.PhoneNumber,
                Status = selector.m.Status,
                IsOnline = selector.u.IsOnline
            }).ToListAsync();

            return (members.Count() > 0) ? members : null;
        }

        public async Task<Member> GetLeaderClubOwnerByCompetition(int competitionId)
        {
            var query = from cic in context.CompetitionInClubs
                        join c in context.Clubs on cic.ClubId equals c.Id
                        join m in context.Members on c.Id equals m.ClubId
                        where cic.CompetitionId.Equals(competitionId) && cic.IsOwner.Equals(true) && m.ClubRoleId.Equals(1) // club manager
                        select m;

            return (query.Any()) ? await query.FirstOrDefaultAsync() : null;
        }

        public async Task<List<int>> GetUserIdsByMembers(List<int> memberIds)
        {
            return await (from m in context.Members
                          where memberIds.Contains(m.Id)
                          select m.UserId).ToListAsync();
        }

        //TA
        public async Task<bool> CheckExistedMemberInClubWhenInsert(int userId, int clubId)
        {
            var query = from m in context.Members
                        where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                                && m.Status.Equals(MemberStatus.Inactive)
                        select m.Id;

            int memberId = await query.FirstOrDefaultAsync();
            return (memberId > 0) ? true : false;

        }

        public async Task<int> GetIdByUserWhenInsert(int userId, int clubId)
        {
            return await (from m in context.Members
                          where m.UserId.Equals(userId) && m.ClubId.Equals(clubId)
                          select m.Id).FirstOrDefaultAsync();
        }

    }
}
