using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo
{
    public class ClubPreviousRepo : Repository<ClubPreviou>, IClubPreviousRepo
    {
        public ClubPreviousRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, string year)
        {
            ClubPreviou clubPrevious = await context.ClubPrevious.FirstOrDefaultAsync(x => x.ClubId.Equals(clubId)
                                                                && x.ClubRoleId.Equals(clubRoleId)
                                                                && x.MemberId.Equals(memberId)
                                                                && x.Year.Equals(year));
            return (clubPrevious != null) ? clubPrevious.Id : 0;
        }

        public async Task<PagingResult<ClubPreviou>> GetByConditions(ClubPreviousRequestModel request)
        {
            var query = from cp in context.ClubPrevious
                        select new { cp };
            if (request.ClubId.HasValue) query = query.Where(x => x.cp.ClubId.Equals(request.ClubId));

            if (request.ClubRoleId.HasValue) query = query.Where(x => x.cp.ClubRoleId.Equals(request.ClubRoleId));

            if (request.MemberId.HasValue) query = query.Where(x => x.cp.MemberId.Equals(request.MemberId));

            if (request.StartTime.HasValue) query = query.Where(x => x.cp.StartTime.Equals(request.StartTime));

            if (request.EndTime.HasValue) query = query.Where(x => x.cp.EndTime.Equals(request.EndTime));

            if (!string.IsNullOrEmpty(request.Year)) query = query.Where(x => x.cp.Year.Equals(request.Year));

            if (request.Status.HasValue) query = query.Where(x => x.cp.Status.Equals(request.Status));

            int totalCount = query.Count();
            List<ClubPreviou> previousClubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new ClubPreviou()
            {
                Id = x.cp.Id,
                ClubId = x.cp.ClubId,
                MemberId = x.cp.MemberId,
                StartTime = x.cp.StartTime,
                EndTime = x.cp.EndTime,
                Year = x.cp.Year,
                Status = x.cp.Status
            }).ToListAsync();

            return (previousClubs.Count > 0) ? new PagingResult<ClubPreviou>(previousClubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
