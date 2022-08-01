using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
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


        //use with method top 3 Competition
        public async Task<List<ViewProcessCompetitionActivity>> GetTopCompetitionActivity(int clubId, int topCompetition, int topCompetitionActivity)
        {
            //
            List<ViewProcessCompetitionActivity> listVPCA = new List<ViewProcessCompetitionActivity>();

            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            List<Competition> listCompe = await (from cic in context.CompetitionInClubs
                                                 where cic.ClubId == clubId
                                                 join comp in context.Competitions on cic.CompetitionId equals comp.Id
                                                 where comp.StartTime >= localTime.DateTime && comp.Status != CompetitionStatus.Cancel && comp.Status == CompetitionStatus.Publish
                                                 orderby comp.StartTime
                                                 select comp).ToListAsync();
            //top x compe
            listCompe = listCompe.Take(topCompetition).ToList();

            //cứ mỗi list compe sẽ có top 3 task
            foreach (Competition comp in listCompe)
            {
                var query = from ca in context.CompetitionActivities
                            where ca.CompetitionId == comp.Id && ca.CreateTime >= localTime
                            orderby ca.CreateTime
                            select ca;

                List<CompetitionActivity> list_CompetitionActivity = await query.Take(topCompetitionActivity).ToListAsync();

                //mỗi task sẽ có người tham gia
                foreach (CompetitionActivity activity in list_CompetitionActivity)
                {


                    ViewProcessCompetitionActivity vcpa = new ViewProcessCompetitionActivity()
                    {
                        Id = activity.Id,
                        CompetitionId = comp.Id,
                        CompetitionName = comp.Name,
                        //ProcessStatus = activity.Process,
                        Status = activity.Status

                    };

                    listVPCA.Add(vcpa);
                }//end task


            }//end competition

            return (listVPCA.Count > 0) ? listVPCA : null;
        }

        //dùng để thống kê
        public async Task<PagingResult<ViewCompetitionActivity>> GetListProcessActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {

            List<ViewCompetitionActivity> listVCA = new List<ViewCompetitionActivity>();

            //LocalTime
            //DateTimeOffset localTime = new LocalTime().GetLocalTime();

            var query = from cic in context.CompetitionInClubs
                        where cic.ClubId == conditions.ClubId
                        from c in context.Competitions
                        where c.Id == cic.CompetitionId
                        from ca in context.CompetitionActivities
                        where ca.CompetitionId == c.Id
                        select ca;

            //PriorityStatus
            if (conditions.PriorityStatus.HasValue) query = query.Where(ca => ca.Priority == conditions.PriorityStatus);

            //Statuses
            if (conditions.Statuses != null) query = query.Where(ca => conditions.Statuses.Contains((CompetitionActivityStatus)ca.Status));

            List<CompetitionActivity> list_CompetitionActivity = await query.ToListAsync();

            int totalCount = query.Count();

            foreach (CompetitionActivity activity in list_CompetitionActivity)
            {

                ViewCompetitionActivity vca = new ViewCompetitionActivity()
                {
                    Id = activity.Id,
                    CompetitionId = activity.Competition.Id,
                    Status = activity.Status
                };

                listVCA.Add(vca);
            }//end task

            return (listVCA.Count > 0) ? new PagingResult<ViewCompetitionActivity>(listVCA, totalCount, conditions.CurrentPage, conditions.PageSize) : null;

        }


        //Get List ClubActivity By Conditions
        public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            var query = from c in context.Competitions
                        where c.Id == conditions.CompetitionId
                        from ca in context.CompetitionActivities
                        where ca.CompetitionId == c.Id && ca.CreateTime >= localTime
                        orderby ca.CreateTime
                        select ca;

            //PriorityStatus
            if (conditions.PriorityStatus.HasValue) query = query.Where(ca => ca.Priority == conditions.PriorityStatus);

            //Statuses
            if (conditions.Statuses != null) query = query.Where(ca => conditions.Statuses.Contains((CompetitionActivityStatus)ca.Status));


            int totalCount = query.Count();

            List<ViewCompetitionActivity> Activities = await query.Skip((conditions.CurrentPage - 1) * conditions.PageSize).Take(conditions.PageSize)
                                                    .Select(ca => new ViewCompetitionActivity
                                                    {
                                                        Id = ca.Id,
                                                        CompetitionId = ca.CompetitionId,
                                                        Name = ca.Name,
                                                        CreateTime = ca.CreateTime,
                                                        Ending = ca.Ending,
                                                        Priority = ca.Priority,
                                                        Status = ca.Status,
                                                    }).ToListAsync();

            return (Activities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(Activities, totalCount, conditions.CurrentPage, conditions.PageSize) : null;

        }


        //lấy task cho thằng đó trong competition
        public async Task<PagingResult<ViewCompetitionActivity>> GetListCompetitionActivitiesIsAssigned(PagingRequest request, int competitionId, PriorityStatus? priorityStatus, List<CompetitionActivityStatus>? statuses, int userId)
        {
            //var query = from m in context.Members
            //            where m.UserId == userId
            //            from mta in context.MemberTakesActivities
            //            where mta.MemberId == m.Id
            //            from ca in context.CompetitionActivities
            //            where ca.Id == mta.CompetitionActivityId
            //                  && ca.CompetitionId == competitionId
            //            select ca;

            var query = from m in context.Members
                        join mta in context.MemberTakesActivities on m.Id equals mta.MemberId
                        join ca in context.CompetitionActivities on mta.CompetitionActivityId equals ca.Id
                        where m.UserId == userId && ca.CompetitionId == competitionId
                        select new { ca, mta, m};

            //PriorityStatus
            if (priorityStatus.HasValue) query = query.Where(x => x.ca.Priority == priorityStatus.Value);

            //Statuses
            if (statuses.Count > 0) query = query.Where(x => statuses.Contains((CompetitionActivityStatus)x.ca.Status));


            int totalCount = await query.CountAsync();


            List<ViewCompetitionActivity> Activities = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                   .Select(x => new ViewCompetitionActivity
                                                   {
                                                       Id = x.ca.Id,
                                                       CompetitionId = x.ca.CompetitionId,
                                                       Name = x.ca.Name,
                                                       CreateTime = x.ca.CreateTime,
                                                       Ending = x.ca.Ending,
                                                       Priority = x.ca.Priority,
                                                       Status = x.ca.Status,
                                                   }).ToListAsync();

            return (Activities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(Activities, totalCount, request.CurrentPage, request.PageSize) : null;

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
