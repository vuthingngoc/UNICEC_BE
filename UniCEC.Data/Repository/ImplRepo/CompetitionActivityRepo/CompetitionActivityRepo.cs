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
            CompetitionActivity clubActivity = await context.CompetitionActivities.FirstOrDefaultAsync(x => x.SeedsCode.Equals(code));
            if (clubActivity != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        //Get Top 4 Club Activities depend on create time
        public async Task<List<ViewDetailCompetitionActivity>> GetClubActivitiesByCreateTime(int universityId, int clubId)
        {

            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();
            //
            var query = from u in context.Universities
                        where u.Id == universityId
                        from c in context.Clubs
                        where c.UniversityId == u.Id 
                        from ca in context.CompetitionActivities
                        //where ca.ClubId == clubId && ca.CreateTime <= localTime.DateTime && ca.Status == Enum.ClubActivityStatus.Happenning
                        orderby ca.CreateTime descending
                        select ca;

            List<ViewDetailCompetitionActivity> clubActivities = await query.Take(4).Select(ca => new ViewDetailCompetitionActivity()
            {
                Id = ca.Id,
                //ClubId = ca.ClubId,
                CreateTime = ca.CreateTime,
                Description = ca.Description,
                Ending = ca.Ending,
                Name = ca.Name,
                SeedsCode = ca.SeedsCode,
                SeedsPoint = ca.SeedsPoint,
                //Status = ca.Status,
                NumOfMember = ca.NumOfMember,

            }).ToListAsync();

            return (clubActivities.Count > 0) ? clubActivities : null;
        }

        



        ////Get List ClubActivity By Conditions
        public async Task<PagingResult<ViewDetailCompetitionActivity>> GetListClubActivitiesByConditions(CompetitionActivityRequestModel conditions)
        {

            //lấy tất cả các task của 1 trường      
            //var query = from u in context.Universities
            //            where u.Id == conditions.UniversityId
            //            from c in context.Clubs
            //            where c.UniversityId == u.Id
            //            //join ca in context.CompetitionActivities on c.Id equals ca.ClubId
            //            select ca;

            ////Club-Id
            //if (conditions.ClubId.HasValue) query = query.Where(ca => ca.ClubId.Equals(conditions.ClubId));
            ////Seeds-Point
            //if (conditions.SeedsPoint.HasValue) query = query.Where(ca => ca.SeedsPoint == conditions.SeedsPoint);
            ////Number-of-member
            //if (conditions.NumberOfMember.HasValue) query = query.Where(ca => ca.NumOfMember == conditions.NumberOfMember);
            ////Status
            ////if (conditions.Status.HasValue) query = query.Where(ca => ca.Status == conditions.Status);

            //int totalCount = query.Count();
            //List<ViewCompetitionActivity> clubActivities = await query.Skip((conditions.CurrentPage - 1) * conditions.PageSize).Take(conditions.PageSize)
            //                                        .Select(ca => new ViewCompetitionActivity
            //                                        {
            //                                            Id = ca.Id,
            //                                            ClubId = ca.ClubId,                                                        
            //                                            CreateTime = ca.CreateTime,
            //                                            Description = ca.Description,
            //                                            Ending = ca.Ending,
            //                                            Name = ca.Name,
            //                                            SeedsCode = ca.SeedsCode,
            //                                            SeedsPoint = ca.SeedsPoint,
            //                                            //Status = ca.Status,
            //                                            NumOfMember = ca.NumOfMember,
            //                                        }).ToListAsync();

            //return (clubActivities.Count > 0) ? new PagingResult<ViewCompetitionActivity>(clubActivities, totalCount, conditions.CurrentPage, conditions.PageSize) : null;
            return null;
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
