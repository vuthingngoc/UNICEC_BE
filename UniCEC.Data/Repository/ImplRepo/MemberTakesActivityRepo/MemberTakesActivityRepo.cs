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

        public async Task<bool> CheckMemberInClub(int clubId, int memberId, string year)
        {
            //club thì phải có trong club id và kì luôn 
            var query = from cp in context.ClubPrevious
                        where cp.MemberId == memberId && cp.ClubId == clubId && cp.Year.Equals(year)
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
            if (!request.Status.ToString().Equals("")) query = query.Where(mta => mta.Status.Equals(request.Status));
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
