using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo
{
    public class ClubPreviousRepo : Repository<ClubPreviou>, IClubPreviousRepo
    {
        public ClubPreviousRepo(UniCECContext context) : base(context)
        {

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

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.cp.Name.Contains(request.Name));

            int totalCount = query.Count();
            List<ClubPreviou> previousClubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new ClubPreviou()
            {
                Id = x.cp.Id,
                ClubId = x.cp.ClubId,
                MemberId = x.cp.MemberId,
                StartTime = x.cp.StartTime,
                EndTime = x.cp.EndTime,
                Name = x.cp.Name                
            }).ToListAsync();

            return new PagingResult<ClubPreviou>(previousClubs, totalCount, request.CurrentPage, request.PageSize);
        }
    }
}
