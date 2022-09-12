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


        //use with method top  Competition
        public async Task<List<ViewProcessCompetitionActivity>> GetTopCompetitionActivity(int clubId, int topCompetition, int topCompetitionActivity)
        {
            //
            List<ViewProcessCompetitionActivity> listVPCA = new List<ViewProcessCompetitionActivity>();

            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            List<Competition> listCompe = await (from cic in context.CompetitionInClubs
                                                 join comp in context.Competitions on cic.CompetitionId equals comp.Id
                                                 where comp.StartTime > localTime.DateTime 
                                                  && cic.ClubId == clubId
                                                  && comp.Status != CompetitionStatus.Cancel
                                                  && comp.Status != CompetitionStatus.Draft
                                                  && comp.Status != CompetitionStatus.Pending
                                                  && comp.Status != CompetitionStatus.PendingReview
                                                  && comp.Status != CompetitionStatus.Approve
                                                 orderby comp.StartTime
                                                 select comp).ToListAsync();
            //top x compe
            listCompe = listCompe.Take(topCompetition).ToList();


            //vẫn thiếu chưa đủ theo Top thì sẽ chạy lấy bù
            if (listCompe.Count < topCompetition)
            {
                var subQuery = from cic in context.CompetitionInClubs
                               join comp in context.Competitions on cic.CompetitionId equals comp.Id
                               where comp.StartTime < localTime.DateTime
                                && cic.ClubId == clubId
                                && comp.Status != CompetitionStatus.Cancel
                                && comp.Status != CompetitionStatus.Draft
                                && comp.Status != CompetitionStatus.Pending
                                && comp.Status != CompetitionStatus.PendingReview
                                && comp.Status != CompetitionStatus.Approve
                               orderby comp.StartTime
                               select comp;


                List<Competition> list_SubQuery = await subQuery.Take(topCompetition - listCompe.Count).ToListAsync();

                foreach (Competition comp in list_SubQuery)
                {
                    listCompe.Add(comp);
                }
            }


            //cứ mỗi list compe sẽ có  thống kê task
            foreach (Competition comp in listCompe)
            {                          
                   //Open Task
                    var queryOpen = from ca in context.CompetitionActivities
                                    where ca.CompetitionId == comp.Id
                                     && ca.Status == CompetitionActivityStatus.Open
                                    select ca;
                    int numOpen = await queryOpen.CountAsync();

                    //Pending Task
                    var queryPending = from ca in context.CompetitionActivities
                                    where ca.CompetitionId == comp.Id
                                     && ca.Status == CompetitionActivityStatus.Pending
                                    select ca;
                    int numPending = await queryPending.CountAsync();

                    //Finished Task
                    var queryFinished = from ca in context.CompetitionActivities
                                    where ca.CompetitionId == comp.Id
                                     && ca.Status == CompetitionActivityStatus.Finished
                                    select ca;
                    int numFinished = await queryFinished.CountAsync();

                    //Completed Task
                    var queryCompleted = from ca in context.CompetitionActivities
                                    where ca.CompetitionId == comp.Id
                                     && ca.Status == CompetitionActivityStatus.Completed
                                    select ca;
                    int numCompleted = await queryCompleted.CountAsync();

                    //Cancelling Task
                    var queryCancelling = from ca in context.CompetitionActivities
                                    where ca.CompetitionId == comp.Id
                                     && ca.Status == CompetitionActivityStatus.Cancelling
                                    select ca;
                    int numCancelling = await queryCancelling.CountAsync();


                    ViewProcessCompetitionActivity vcpa = new ViewProcessCompetitionActivity()
                    {                      
                        CompetitionId = comp.Id,
                        CompetitionName = comp.Name,                        
                        numberOfOpen = numOpen,
                        numberOfPending = numPending,
                        numberOfFinished = numFinished,
                        numberOfCompleted = numCompleted,
                        numberOfCancelling = numCancelling,                       
                    };

                listVPCA.Add(vcpa);
                
            }//end competition

            return (listVPCA.Count > 0) ? listVPCA : null;
        }

       
        //// cái này không có truyền competition Id thì trả ra cái gì ???
        //public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions2(CompetitionActivityRequestModel conditions)
        //{

        //    List<ViewCompetitionActivity> listVCA = new List<ViewCompetitionActivity>();

        //    //LocalTime
        //    //DateTimeOffset localTime = new LocalTime().GetLocalTime();

        //    var query = from cic in context.CompetitionInClubs
        //                where cic.ClubId == conditions.ClubId
        //                from c in context.Competitions
        //                where c.Id == cic.CompetitionId
        //                from ca in context.CompetitionActivities
        //                where ca.CompetitionId == c.Id
        //                select ca;

        //    //PriorityStatus
        //    if (conditions.PriorityStatus.HasValue) query = query.Where(ca => ca.Priority == conditions.PriorityStatus);

        //    //Statuses
        //    if (conditions.Statuses != null) query = query.Where(ca => conditions.Statuses.Contains((CompetitionActivityStatus)ca.Status));

        //    List<CompetitionActivity> list_CompetitionActivity = await query.ToListAsync();

        //    int totalCount = query.Count();

        //    foreach (CompetitionActivity activity in list_CompetitionActivity)
        //    {

        //        ViewCompetitionActivity vca = new ViewCompetitionActivity()
        //        {
        //            Id = activity.Id,
        //            CompetitionId = activity.Competition.Id,
        //            Status = activity.Status
        //        };

        //        listVCA.Add(vca);
        //    }//end task

        //    return (listVCA.Count > 0) ? new PagingResult<ViewCompetitionActivity>(listVCA, totalCount, conditions.CurrentPage, conditions.PageSize) : null;

        //}


        //Get List ClubActivity By Conditions
        public async Task<PagingResult<ViewCompetitionActivity>> GetListActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            var query = from c in context.Competitions
                        where c.Id == conditions.CompetitionId
                        from ca in context.CompetitionActivities
                        where ca.CompetitionId == c.Id 
                        select ca;

            //PriorityStatus
            if (conditions.PriorityStatus.HasValue) query = query.Where(ca => ca.Priority == conditions.PriorityStatus);

            //Statuses
            if (conditions.Statuses != null) query = query.Where(ca => conditions.Statuses.Contains(ca.Status)); //((CompetitionActivityStatus)ca.Status));

            //search name
            if(!string.IsNullOrEmpty(conditions.name)) query.Where(ca => ca.Name.ToLower().Contains(conditions.name.ToLower()));

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
                                                        CreatorId = ca.CreatorId,                                                    
                                                    }).ToListAsync();

           
            //thêm Creator Name
            foreach (ViewCompetitionActivity vca in Activities)
            {
                User creator = await (from u in context.Users
                                      where u.Id == vca.CreatorId
                                      select u).FirstOrDefaultAsync();

                vca.CreatorName = creator.Fullname;
            }

            return (Activities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(Activities, totalCount, conditions.CurrentPage, conditions.PageSize) : null;

        }


        //lấy task cho thằng đó trong competition
        public async Task<PagingResult<ViewCompetitionActivity>> GetListCompetitionActivitiesIsAssigned(PagingRequest request, int competitionId, PriorityStatus? priorityStatus, List<CompetitionActivityStatus> statuses, string name, int userId)
        {
            
            var query = from m in context.Members
                        join mta in context.MemberTakesActivities on m.Id equals mta.MemberId
                        join ca in context.CompetitionActivities on mta.CompetitionActivityId equals ca.Id
                        where m.UserId == userId && ca.CompetitionId == competitionId
                        select new { ca, mta, m };

            //PriorityStatus
            if (priorityStatus.HasValue) query = query.Where(x => x.ca.Priority == priorityStatus.Value);

            //Statuses
            if (statuses.Count > 0) query = query.Where(x => statuses.Contains(x.ca.Status));//(CompetitionActivityStatus)x.ca.Status));

            //name
            if(!string.IsNullOrEmpty(name)) query = query.Where(x => x.ca.Name.ToLower().Contains(name.ToLower()));

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
                                                       CreatorId = x.ca.CreatorId,                                                    
                                                   }).ToListAsync();


            //thêm Creator Name
            foreach (ViewCompetitionActivity vca in Activities)
            {
                User creator = await (from u in context.Users
                                            where u.Id == vca.CreatorId
                                            select u).FirstOrDefaultAsync();

                vca.CreatorName = creator.Fullname;
            }

            return (Activities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(Activities, totalCount, request.CurrentPage, request.PageSize) : null;

        }


        // Nhat
        public async Task<int> GetTotalActivityByClub(int clubId)
        {
            var query = from cic in context.CompetitionInClubs
                        join comp in context.Competitions on cic.CompetitionId equals comp.Id
                        join ca in context.CompetitionActivities on comp.Id equals ca.CompetitionId
                        where !comp.Status.Equals(CompetitionStatus.Cancel) && !ca.Status.Equals(CompetitionActivityStatus.Cancelling)
                                && cic.ClubId.Equals(clubId)
                        select ca;

            return await query.CountAsync();
            //return 0;
        }


    }
}
