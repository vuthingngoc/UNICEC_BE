using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.Data.Repository.ImplRepo.ClubHistoryRepo
{
    public class ClubHistoryRepo : Repository<ClubHistory>, IClubHistoryRepo
    {
        public ClubHistoryRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, int termId)
        {
            ClubHistory clubHistory = await context.ClubHistories.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId)
                                                                && x.ClubRoleId.Equals(clubRoleId)
                                                                && x.MemberId.Equals(memberId)
                                                                && x.TermId.Equals(termId));
            return (clubHistory != null) ? clubHistory.Id : 0;
        }

        public async Task<PagingResult<ClubHistory>> GetByConditions(ClubHistoryRequestModel request)
        {
            var query = from cp in context.ClubHistories
                        select new { cp };
            if (request.ClubId.HasValue) query = query.Where(x => x.cp.ClubId.Equals(request.ClubId));

            if (request.ClubRoleId.HasValue) query = query.Where(x => x.cp.ClubRoleId.Equals(request.ClubRoleId));

            if (request.MemberId.HasValue) query = query.Where(x => x.cp.MemberId.Equals(request.MemberId));

            if (request.StartTime.HasValue) query = query.Where(x => x.cp.StartTime.Equals(request.StartTime));

            if (request.EndTime.HasValue) query = query.Where(x => x.cp.EndTime.Equals(request.EndTime));

            if (request.TermId.HasValue) query = query.Where(x => x.cp.TermId.Equals(request.TermId));

            if (request.Status.HasValue) query = query.Where(x => x.cp.Status.Equals(request.Status));

            int totalCount = query.Count();
            List<ClubHistory> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new ClubHistory()
            {
                Id = x.cp.Id,
                ClubId = x.cp.ClubId,
                MemberId = x.cp.MemberId,
                TermId = x.cp.TermId,
                StartTime = x.cp.StartTime,
                EndTime = x.cp.EndTime,
                Status = x.cp.Status
            }).ToListAsync();

            return (items.Count > 0) ? new PagingResult<ClubHistory>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<List<int>> GetIdsByMember(int memberID)
        {
            var query = from cp in context.ClubHistories
                        where cp.MemberId == memberID
                        select new { cp };
            List<int> previousClubs = await query.Select(x => x.cp.Id).ToListAsync();

            return (previousClubs.Count > 0) ? previousClubs : null;
        }

        public async Task<PagingResult<ViewClubMember>> GetMembersByClub(int clubId, int termId, PagingRequest request)
        {
            var query = from cp in context.ClubHistories
                        join m in context.Members on cp.MemberId equals m.Id
                        join u in context.Users on m.StudentId equals u.Id
                        join r in context.ClubRoles on cp.ClubRoleId equals r.Id
                        where cp.ClubId.Equals(clubId) && cp.TermId.Equals(termId)
                        select new { cp, u, r };
            
            int totalCount = query.Count();
            List<ViewClubMember> clubMembers = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new ViewClubMember()
            {
                MemberId = x.cp.MemberId,
                ClubRoleName = x.r.Name,
                Name = x.u.Fullname
            }).ToListAsync();

            return (clubMembers.Count > 0) ? new PagingResult<ViewClubMember>(clubMembers, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
