using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using System.Linq;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;
using System.Collections.Generic;
using UniCEC.Data.Enum;
using System;

namespace UniCEC.Data.Repository.ImplRepo.ClubActivityRepo
{
    public class ClubActivityRepo : Repository<ClubActivity>, IClubActivityRepo
    {
        public ClubActivityRepo(UniCECContext context) : base(context)
        {

        }

        //check exist code
        public async Task<bool> CheckExistCode(string code)
        {
            bool check = false;
            ClubActivity clubActivity = await context.ClubActivities.FirstOrDefaultAsync(x => x.SeedsCode.Equals(code));
            if (clubActivity != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        //Get Top 4 Club Activities depend on create time
        public async Task<List<ViewClubActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId)
        {
            
            //LocalTime
            var info = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info); //.AddHours(23).AddMinutes(59).AddSeconds(59).AddTicks(999);
            //
            var query = from u in context.Universities
                        where u.Id == universityId
                        from c in context.Clubs
                        where c.UniversityId == u.Id
                        from ca in context.ClubActivities
                        where c.Id == clubId && ca.CreateTime <= localTime.DateTime
                        orderby ca.CreateTime descending
                        select ca;

            List<ViewClubActivity> clubActivities = await query.Take(4).Select(ca => new ViewClubActivity()
            {
                Id = ca.Id,
                ClubId = ca.ClubId,
                Beginning = ca.Beginning,
                CreateTime = ca.CreateTime,
                Description = ca.Description,
                Ending = ca.Ending,
                Name = ca.Name,
                SeedsCode = ca.SeedsCode,
                SeedsPoint = ca.SeedsPoint,
                Status = ca.Status,
                NumOfMember = ca.NumOfMember,

            }).ToListAsync();

            return (clubActivities.Count > 0) ? clubActivities : null;
        }

        ////Get List ClubActivity By Conditions
        public async Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions)
        {

            //lấy tất cả các task của 1 trường      
            var query = from u in context.Universities
                        where u.Id == conditions.UniversityId
                        from c in context.Clubs
                        where c.UniversityId == u.Id
                        join ca in context.ClubActivities on c.Id equals ca.ClubId
                        select ca;

            //Club-Id
            if (conditions.ClubId.HasValue) query = query.Where(ca => ca.ClubId.Equals(conditions.ClubId));
            //Seeds-Point
            if (conditions.SeedsPoint.HasValue) query = query.Where(ca => ca.SeedsPoint == conditions.SeedsPoint);
            //Number-of-member
            if (conditions.NumberOfMember.HasValue) query = query.Where(ca => ca.NumOfMember == conditions.NumberOfMember);
            //Status
            if (conditions.Status.HasValue) query = query.Where(ca => ca.Status == conditions.Status);
            //-------------------------------------------------Time-------------------------------------------------
            //chỉ search theo ngày
            ////Begin-Time
            //if (conditions.BeginTime.HasValue) query = query.Where(ca => ca.Beginning.Date.Equals(conditions.BeginTime));
            ////End-Time
            //if (conditions.EndTime.HasValue) query = query.Where(ca => ca.Ending.Date.Equals(conditions.EndTime));
            ////Creatime
            //if (conditions.CreateTime.HasValue) query = query.Where(ca => ca.CreateTime.Date.Equals(conditions.CreateTime));

            int totalCount = query.Count();
            List<ViewClubActivity> clubActivities = await query.Skip((conditions.CurrentPage - 1) * conditions.PageSize).Take(conditions.PageSize)
                                                    .Select(ca => new ViewClubActivity
                                                    {
                                                        Id = ca.Id,
                                                        ClubId = ca.ClubId,
                                                        Beginning = ca.Beginning,
                                                        CreateTime = ca.CreateTime,
                                                        Description = ca.Description,
                                                        Ending = ca.Ending,
                                                        Name = ca.Name,
                                                        SeedsCode = ca.SeedsCode,
                                                        SeedsPoint = ca.SeedsPoint,
                                                        Status = ca.Status,
                                                        NumOfMember = ca.NumOfMember,
                                                    }).ToListAsync();
            //

            return (clubActivities.Count > 0) ? new PagingResult<ViewClubActivity>(clubActivities, totalCount, conditions.CurrentPage, conditions.PageSize) : null;
        }

    }
}
