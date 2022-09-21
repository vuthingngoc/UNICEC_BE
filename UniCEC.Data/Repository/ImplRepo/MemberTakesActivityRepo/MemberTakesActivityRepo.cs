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
using System;

namespace UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo
{
    public class MemberTakesActivityRepo : Repository<MemberTakesActivity>, IMemberTakesActivityRepo
    {
        public MemberTakesActivityRepo(UniCECContext context) : base(context) { }




        public async Task<bool> CheckMemberTakesTask(int competitionActivityId, int memberId)
        {
            var query = await (from mta in context.MemberTakesActivities
                               where mta.CompetitionActivityId == competitionActivityId && mta.MemberId == memberId
                               select mta).FirstOrDefaultAsync();

            return (query != null) ? true : false;
        }



        public async Task<bool> RemoveMemberTakeTask(int memberTakeActivityId)
        {
            MemberTakesActivity result = await (from mta in context.MemberTakesActivities where mta.Id == memberTakeActivityId select mta).FirstOrDefaultAsync();
            context.MemberTakesActivities.Remove(result);
            await Update();
            return true;
        }

        public async Task<List<MemberTakesActivity>> ListMemberTakesActivity(int competitionActivityId)
        {
            List<MemberTakesActivity> result = await (from mta in context.MemberTakesActivities
                                                      where mta.CompetitionActivityId == competitionActivityId
                                                      select mta).ToListAsync();
            return (result.Count > 0) ? result : null;
        }

        public async Task RemoveMemberTakeAllTaskIsDoing(int memberId)
        {
            List<MemberTakesActivity> result = await(from mta in context.MemberTakesActivities
                                                     join ca in context.CompetitionActivities on mta.CompetitionActivityId equals ca.Id
                                                     where mta.MemberId == memberId 
                                                     && ca.Status != CompetitionActivityStatus.Completed
                                                     && ca.Status != CompetitionActivityStatus.Cancelling
                                                     select mta).ToListAsync();
            if (result.Count > 0)
            {
                foreach (MemberTakesActivity activity in result)
                {
                    context.MemberTakesActivities.Remove(activity);
                }
            }
            await Update();
        }

        //Get-All-Taskes-By-Conditions 
        //lấy tất cả task được assigned cho member và thuộc Competition Activity - CompetitionManager Role
        //public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksByConditions(MemberTakesActivityRequestModel request)
        //{
        //    //lấy list tasks của member id
        //    var query = from MemberTakesActivity mta in context.MemberTakesActivities
        //                where mta.CompetitionActivityId == request.CompetitionActivityId
        //                select mta;
        //    //status
        //    if (request.Status.HasValue) query = query.Where(mta => mta.Status == request.Status);

        //    int totalCount = query.Count();

        //    List<ViewMemberTakesActivity> memberTakesActivities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(mta => new ViewMemberTakesActivity()
        //    {
        //        Id = mta.Id,
        //        MemberId = mta.MemberId,
        //        MemberName = mta.Member.User.Fullname,
        //        CompetitionActivityId = mta.CompetitionActivityId,
        //        StartTime = mta.StartTime,
        //        Deadline = mta.Deadline,
        //        Status = mta.Status,
        //    }).ToListAsync();

        //    //
        //    return (memberTakesActivities.Count != 0) ? new PagingResult<ViewMemberTakesActivity>(memberTakesActivities, totalCount, request.CurrentPage, request.PageSize) : null;
        //}


        //public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksMemberByConditions(MemberTakesActivityRequestModel request, int userId)
        //{
        //    //hàm này cho dù Member có qua term khác thì vẫn tìm ra task Member đó lấy từ của chính nó khi ở term cũ
        //    //ví dụ member id = 1 sang term mới thì member id = 3 nhưng vẫn còn thông tin ở task cũ 

        //    var query = from m in context.Members
        //                where m.UserId == userId && m.ClubId == request.ClubId
        //                from mta in context.MemberTakesActivities
        //                where mta.MemberId == m.Id
        //                select mta;

        //    //status
        //    if (request.Status.HasValue) query = query.Where(mta => mta.Status == request.Status);

        //    int totalCount = query.Count();

        //    List<ViewMemberTakesActivity> memberTakesActivities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(mta => new ViewMemberTakesActivity()
        //    {
        //        Id = mta.Id,
        //        MemberId = mta.MemberId,
        //        MemberName = mta.Member.User.Fullname,
        //        CompetitionActivityId = mta.CompetitionActivityId,
        //        StartTime = mta.StartTime,
        //        Deadline = mta.Deadline,
        //        Status = mta.Status,
        //    }).ToListAsync();

        //    //
        //    return (memberTakesActivities.Count != 0) ? new PagingResult<ViewMemberTakesActivity>(memberTakesActivities, totalCount, request.CurrentPage, request.PageSize) : null;

        //}

        //public async Task<int> GetNumberOfMemberIsSubmitted(int competitionActivityId)
        //{
        //   var query = from mta in context.MemberTakesActivities
        //               where mta.CompetitionActivityId == competitionActivityId && mta.Status != MemberTakesActivityStatus.Doing
        //                                                                        && mta.Status != MemberTakesActivityStatus.LateTime
        //                                                                        && mta.Status != MemberTakesActivityStatus.Finished
        //                                                                        && mta.Status != MemberTakesActivityStatus.FinishedLate
        //                                                                        select mta;
        //    int result = await query.CountAsync();
        //    return (result > 0) ? result : 0;    
        //}

        //Check task belong to student 
        //public async Task<bool> CheckTaskBelongToStudent(int MemberTakeActivityId, int UserId, int ClubId)
        //{

        //    MemberTakesActivity query = await (from m in context.Members
        //                                       where m.UserId == UserId && m.ClubId == ClubId
        //                                       from mta in context.MemberTakesActivities
        //                                       where mta.MemberId == m.Id && mta.Id == MemberTakeActivityId
        //                                       select mta).FirstOrDefaultAsync();

        //    return (query != null) ? true : false;
        //}



        ////Competition Activity call this method 
        //public async Task UpdateDeadlineDate(int CompetitionActivityId, DateTime deadline)
        //{
        //    List<MemberTakesActivity> memberTakesActivities = await (from mta in context.MemberTakesActivities
        //                                                             where mta.CompetitionActivityId == CompetitionActivityId
        //                                                             select mta).ToListAsync();

        //    if (memberTakesActivities.Count != 0)
        //    {
        //        foreach (MemberTakesActivity memberTakesActivity in memberTakesActivities)
        //        {
        //            memberTakesActivity.Deadline = deadline;
        //        }
        //    }
        //    context.SaveChanges();
        //}

    }
}
