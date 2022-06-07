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
        public MemberTakesActivityRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<int> GetNumOfMemInTask_Status(int competitionActivityId, MemberTakesActivityStatus Status)
        {
            var query = from mta in context.MemberTakesActivities
                        where mta.CompetitionActivityId == competitionActivityId
                        select mta;

            query = query.Where(m => m.Status == Status);

            List<MemberTakesActivity> list = await query.ToListAsync();

            int totalMemTakesTask_Status = list.Count();

            return totalMemTakesTask_Status;
        }

        public async Task<int> GetNumOfMemInTask(int competitionActivityId)
        {
            //
            var query = from mta in context.MemberTakesActivities
                        where mta.CompetitionActivityId == competitionActivityId
                        select mta;

            List<MemberTakesActivity> list = await query.ToListAsync();

            int totalMemTakesTask = list.Count();

            return totalMemTakesTask;
        }

        public async Task<bool> CheckMemberTakesTask(int competitionActivityId, int memberId)
        {
            //
            var query = from mta in context.MemberTakesActivities
                        where mta.CompetitionActivityId == competitionActivityId && mta.MemberId == memberId
                        select mta;
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

        public async Task<bool> CheckTaskBelongToStudent(int MemberTakeActivityId, int UserId, int UniversityId)
        {
            //User -> Uni -> Member -> ClubHistory -> Club -> ClubActivity -> MemberTakeTaskActivity
            var query = from us in context.Users
                        where us.Id == UserId
                        from uni in context.Universities
                        where uni.Id == UniversityId
                        from mem in context.Members
                        where mem.UserId == us.Id
                        //from ch in context.ClubHistories
                        //where mem.Id == ch.MemberId
                        from c in context.Clubs
                        where mem.ClubId == c.Id
                        from mta in context.MemberTakesActivities
                        where mta.Id == MemberTakeActivityId
                        select mta;

            MemberTakesActivity result = await query.FirstOrDefaultAsync();
            if (result != null)
            {
                return true;
            }
            return false;
        }



        //Get-All-Taskes-By-Conditions
        public async Task<PagingResult<ViewMemberTakesActivity>> GetAllTaskesByConditions(MemberTakesActivityRequestModel request)
        {
            //lấy list tasks của member id
            var query = from MemberTakesActivity mta in context.MemberTakesActivities
                        where mta.MemberId == request.MemberId
                        select mta;
            //status
            //if (request.Status.HasValue) query = query.Where(mta => mta.Status == request.Status);
            //
            int totalCount = query.Count();

            List<ViewMemberTakesActivity> memberTakesActivities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(mta => new ViewMemberTakesActivity()
            {
                Id = mta.Id,
                MemberId = mta.MemberId,
                CompetitionActivityId = mta.CompetitionActivityId,
                StartTime = mta.StartTime,
                Deadline = mta.Deadline,
                EndTime = mta.EndTime,
                //Status = mta.Status,
            }).ToListAsync();

            //
            return (memberTakesActivities.Count != 0) ? new PagingResult<ViewMemberTakesActivity>(memberTakesActivities, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task UpdateDeadlineDate(int ClubActivityId, DateTime deadline)
        {
            List<MemberTakesActivity> memberTakesActivities = await (from mta in context.MemberTakesActivities
                                                                     where mta.CompetitionActivityId == ClubActivityId
                                                                     select mta).ToListAsync();

            if (memberTakesActivities.Count != 0)
            {
                foreach (MemberTakesActivity memberTakesActivity in memberTakesActivities)
                {
                    memberTakesActivity.Deadline = deadline;
                }
            }
            context.SaveChanges();


        }


    }
}
