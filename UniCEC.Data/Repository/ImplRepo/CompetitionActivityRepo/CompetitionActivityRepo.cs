using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionActivityRepo
{
    public class CompetitionActivityRepo : Repository<CompetitionActivity>, ICompetitionActivityRepo
    {
        public CompetitionActivityRepo(UniCECContext context) : base(context)
        {

        }

        //check exist code
        public async Task<bool> CheckExistCode(string code)
        {
            bool check = false;
            CompetitionActivity competitionActivity = await context.CompetitionActivities.FirstOrDefaultAsync(x => x.SeedsCode.Equals(code));
            if (competitionActivity != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        //use with method top 3 Competition
        public async Task<List<ViewProcessCompetitionActivity>> GetTop3CompetitionActivity(int clubId)
        {
            //
            List<ViewProcessCompetitionActivity> listVPCA = new List<ViewProcessCompetitionActivity>();

            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            List<Competition> listCompe = await (from cic in context.CompetitionInClubs
                                                 where cic.ClubId == clubId
                                                 join comp in context.Competitions on cic.CompetitionId equals comp.Id
                                                 where comp.StartTime >= localTime
                                                 orderby comp.StartTime
                                                 select comp).ToListAsync();
            //top 3 compe
            listCompe = listCompe.Take(3).ToList();

            //cứ mỗi list compe sẽ có top 3 task
            foreach (Competition comp in listCompe)
            {
                var query = from ca in context.CompetitionActivities
                            where ca.CompetitionId == comp.Id && ca.Ending >= localTime
                            orderby ca.Ending
                            select ca;

                List<CompetitionActivity> list_CompetitionActivity = await query.Take(3).ToListAsync();

                //mỗi task sẽ có người tham gia
                foreach (CompetitionActivity activity in list_CompetitionActivity)
                {
                    int numberOfMemberJoin = activity.NumOfMember;
                    //doing
                    var doing = from mta in context.MemberTakesActivities
                                where mta.CompetitionActivityId == activity.Id
                                   && mta.Status == Enum.MemberTakesActivityStatus.Doing
                                select mta;

                    int numberOfMemberDoing = await doing.CountAsync();

                    //finish 
                    var finish = from mta in context.MemberTakesActivities
                                 where mta.CompetitionActivityId == activity.Id
                                    && mta.Status == Enum.MemberTakesActivityStatus.Finished
                                 select mta;

                    int numberOfMemberFinish = await finish.CountAsync();

                    //finishLate
                    var finishLate = from mta in context.MemberTakesActivities
                                     where mta.CompetitionActivityId == activity.Id
                                        && mta.Status == Enum.MemberTakesActivityStatus.FinishedLate
                                     select mta;

                    int numberOfMemberFinishLate = await finishLate.CountAsync();

                    //late
                    var late = from mta in context.MemberTakesActivities
                               where mta.CompetitionActivityId == activity.Id
                                  && mta.Status == Enum.MemberTakesActivityStatus.LateTime
                               select mta;

                    int numberOfMemberLate = await late.CountAsync();

                    ViewProcessCompetitionActivity vcpa = new ViewProcessCompetitionActivity()
                    {
                        Id = activity.Id,
                        CompetitionId = comp.Id,
                        CreateTime = activity.CreateTime,
                        Ending = activity.Ending,
                        Name = activity.Name,
                        ProcessStatus = activity.Process,
                        Priority = activity.Priority,
                        NumOfMemberJoin = numberOfMemberJoin,
                        NumMemberDoingTask = numberOfMemberDoing,
                        NumMemberDoneTask = numberOfMemberFinish,
                        NumMemberDoneLateTask = numberOfMemberFinishLate,
                        NumMemberLateTask = numberOfMemberLate,        
                    };

                    listVPCA.Add(vcpa);
                }//end task


            }//end competition

            return (listVPCA.Count > 0) ? listVPCA : null;  
        }



        ////Get List ClubActivity By Conditions
        public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {

            var query = from c in context.Competitions
                        where c.Id == conditions.CompetitionId
                        from ca in context.CompetitionActivities
                        where ca.CompetitionId == c.Id
                        select ca;


            //ProcessStatus
            if (conditions.ProcessStatus.HasValue) query = query.Where(ca => ca.Process == conditions.ProcessStatus);

            //PriorityStatus
            if (conditions.PriorityStatus.HasValue) query = query.Where(ca => ca.Priority == conditions.PriorityStatus);

            //Status
            if (conditions.Status.HasValue) query = query.Where(ca => ca.Status == conditions.Status);


            int totalCount = query.Count();

            List<ViewCompetitionActivity> Activities = await query.Skip((conditions.CurrentPage - 1) * conditions.PageSize).Take(conditions.PageSize)
                                                    .Select(ca => new ViewCompetitionActivity
                                                    {
                                                        Id = ca.Id,
                                                        CompetitionId = ca.CompetitionId,
                                                        Name = ca.Name,
                                                        CreateTime = ca.CreateTime,
                                                        Ending = ca.Ending,
                                                        ProcessStatus = ca.Process,
                                                        Priority = ca.Priority,
                                                        Status = ca.Status,
                                                    }).ToListAsync();

            return (Activities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(Activities, totalCount, conditions.CurrentPage, conditions.PageSize) : null;

        }

        // Nhat
        public async Task<int> GetTotalActivityByClub(int clubId)
        {
            //var query = from ca in context.CompetitionActivities
            //            where ca.ClubId.Equals(clubId) && ca.Ending.Date >= new LocalTime().GetLocalTime().DateTime  
            //            select new { ca };

            //return await query.CountAsync();
            return 0;
        }
    }
}
