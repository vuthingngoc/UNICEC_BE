using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo
{
    public class MemberTakesActivityRepo : Repository<MemberTakesActivity>, IMemberTakesActivityRepo
    {
        public MemberTakesActivityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<int> GetNumOfMemInTask_Status(int ClubActivityId, MemberTakesActivityStatus Status)
        {
            var query = from mta in context.MemberTakesActivities
                        where mta.ClubActivityId == ClubActivityId
                        select mta;

            query = query.Where(m => m.Status == Status);

            List<MemberTakesActivity> list = await query.ToListAsync();

            int totalMemTakesTask_Status = list.Count();

            return totalMemTakesTask_Status;
        }

        public async Task<int> GetNumOfMemInTask(int ClubActivityId)
        {
            //
            var query = from mta in context.MemberTakesActivities
                        where mta.ClubActivityId == ClubActivityId
                        select mta;

            List<MemberTakesActivity> list = await query.ToListAsync();
            
            int totalMemTakesTask = list.Count();

            return totalMemTakesTask;
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

        //public async Task<bool> AddNumOfMemInTask(int ClubActivityId)
        //{
        //    var query1 = from ca in context.ClubActivities
        //                 where ca.Id == ClubActivityId
        //                 select ca;

        //    int CurrentTotal = query1.FirstOrDefaultAsync().Result.NumOfMember;

        //    ////đếm bn member task activity đó
        //    //var query2 = from mta in context.MemberTakesActivities
        //    //             where mta.ClubActivityId == ClubActivityId
        //    //             select mta;

        //    //int totalMemTakesTask = query2.Count();



        //    if ()
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

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
            if (request.Status.HasValue) query = query.Where(mta => mta.Status == request.Status);
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
