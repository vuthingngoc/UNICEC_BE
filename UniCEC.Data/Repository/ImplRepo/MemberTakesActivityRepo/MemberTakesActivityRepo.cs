using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo
{
    public class MemberTakesActivityRepo : Repository<MemberTakesActivity>, IMemberTakesActivityRepo
    {
        public MemberTakesActivityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckMemberTakesTask(int clubActivityId, int memberId)
        {
            //
            var query = from mta in context.MemberTakesActivities
                        where mta.ClubActivityId == clubActivityId && mta.MemberId == memberId
                        select mta;
            int check = query.Count();
            if (check > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Get-All-Taskes-By-Conditions
        public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTaskesByConditions(MemberTakesActivityRequestModel request)
        {
            //lấy list tasks của member id
            var query = from MemberTakesActivity mta in context.MemberTakesActivities
                        where mta.MemberId == request.MemberId
                        select mta;
            //club id
            if (request.ClubId.HasValue) query = query.Where(mta => mta.ClubActivity.ClubId == request.ClubId);
            //status
            if (request.Status.HasValue) query = query.Where(mta => mta.Status.Equals(request.Status));
            //
            int totalCount = query.Count();

            List<ViewMemberTakesActivity> memberTakesActivities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(mta => new ViewMemberTakesActivity()
            {
                Id = mta.Id,
                MemberId = mta.MemberId,
                ClubActivityId = mta.ClubActivityId,
                StartTime = mta.StartTime,
                Deadline = mta.Deadline,
                EndTime = mta.EndTime,
                Status = mta.Status,
            }).ToListAsync();

            //
            return (memberTakesActivities.Count != 0) ? new PagingResult<ViewMemberTakesActivity>(memberTakesActivities, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
