using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.Enum;
using System;
using UniCEC.Data.Common;

namespace UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo
{
    public class SponsorInCompetitionRepo : Repository<SponsorInCompetition>, ISponsorInCompetitionRepo
    {
        public SponsorInCompetitionRepo(UniCECContext context) : base(context)
        {

        }

        //Check SponsorDeny
        public async Task<SponsorInCompetition> GetNewestApply(int sponsorId, int competitionId, int userId)
        {
            SponsorInCompetition query = await (from sic in context.SponsorInCompetitions
                                                where sic.SponsorId == sponsorId
                                                      && sic.CompetitionId == competitionId
                                                      && sic.UserId == userId
                                                      && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                                                orderby sic.CreateTime descending
                                                select sic).FirstOrDefaultAsync();
            if (query != null)
            {

                return query;
            }
            else
            {
                return null;
            }
        }


        //Check to addSponsor , Leader delete, 
        public async Task<SponsorInCompetition> CheckSponsorInCompetition(int sponsorId, int competitionId, int userId)
        {
            SponsorInCompetition query = null;


            // Advoid this User belong to this Sponsor is apply again       (has UserId) != 0
            if (userId != 0)
            {
                query = await (from sic in context.SponsorInCompetitions
                               where sic.SponsorId == sponsorId
                                     && sic.CompetitionId == competitionId
                                     && sic.UserId == userId && sic.Status != SponsorInCompetitionStatus.Rejected
                               select sic).FirstOrDefaultAsync();
            }

            // Advoid another User belong to this Sponsor is apply again    (not UserId)  = 0
            if (userId == 0)
            {
                query = await (from sic in context.SponsorInCompetitions
                               where sic.SponsorId == sponsorId
                                     && sic.CompetitionId == competitionId
                                     && sic.Status != SponsorInCompetitionStatus.Rejected
                               select sic).FirstOrDefaultAsync();
            }

            if (query != null)
            {

                return query;
            }
            else
            {
                return null;
            }
        }


        public async Task DeleteSponsorInCompetition(int sponsorInCompetitionId)
        {
            SponsorInCompetition result = await (from sic in context.SponsorInCompetitions
                                                 where sic.Id == sponsorInCompetitionId
                                                 select sic).FirstOrDefaultAsync();
            context.SponsorInCompetitions.Remove(result);
            await Update();
        }


        //get sponsor is applied with status Approved - bên trong View Detail Competition
        public async Task<List<ViewSponsorInComp>> GetListSponsor_In_Competition(int CompetitionId)
        {
            List<SponsorInCompetition> sponsor_In_Competition_List = await (from sic in context.SponsorInCompetitions
                                                                            where CompetitionId == sic.CompetitionId && sic.Status == SponsorInCompetitionStatus.Approved
                                                                            select sic).ToListAsync();

            List<ViewSponsorInComp> listViewSponsor = new List<ViewSponsorInComp>();

            if (sponsor_In_Competition_List.Count > 0)
            {
                foreach (var sponsor_In_Competition in sponsor_In_Competition_List)
                {
                    Sponsor sponsor = await (from s in context.Sponsors
                                             where s.Id == sponsor_In_Competition.SponsorId
                                             select s).FirstOrDefaultAsync();

                    ViewSponsorInComp vsc = new ViewSponsorInComp()
                    {
                        Id = sponsor.Id,
                        Name = sponsor.Name,
                        Logo = sponsor.Logo,
                    };

                    listViewSponsor.Add(vsc);
                }

                if (listViewSponsor.Count > 0)
                {
                    return listViewSponsor;
                }
            }
            return null;
        }

        //get sponsor apply with status(optional) - ViewSponsorInCompetition
        public async Task<PagingResult<ViewSponsorInCompetition>> GetListSponsor_In_Competition(SponsorApplyRequestModel request)
        {
            var query = from sic in context.SponsorInCompetitions
                        where request.CompetitionId == sic.CompetitionId //&& sic.Status != SponsorInCompetitionStatus.Rejected
                        select sic;

            if (request.status.HasValue) query = query.Where(s => s.Status == request.status);

            int totalCount = query.Count();

            List<ViewSponsorInCompetition> list_vsic = new List<ViewSponsorInCompetition>();

            List<SponsorInCompetition> sponsorInCompetitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (SponsorInCompetition sponsorInCompetition in sponsorInCompetitions)
            {
                //user 
                User user = await (from us in context.Users
                                   where us.Id == sponsorInCompetition.UserId
                                   select us).FirstOrDefaultAsync();
                //sponsor
                Sponsor sponsor = await (from s in context.Sponsors
                                         where s.Id == sponsorInCompetition.SponsorId
                                         select s).FirstOrDefaultAsync();

                ViewSponsorInCompetition vsic = new ViewSponsorInCompetition()
                {
                    Id = sponsorInCompetition.Id,
                    CompetitionId = sponsorInCompetition.CompetitionId,
                    SponsorId = sponsorInCompetition.SponsorId,
                    UserId = sponsorInCompetition.UserId,
                    Email = user.Email,
                    Fullname = user.Fullname,
                    CreateTime = (DateTime)sponsorInCompetition.CreateTime,
                    SponsorName = sponsor.Name,
                    SponsorLogo = sponsor.Logo,
                    Status = sponsorInCompetition.Status
                };
                list_vsic.Add(vsic);
            }
            return (list_vsic.Count > 0) ? new PagingResult<ViewSponsorInCompetition>(list_vsic, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //
        public async Task<PagingResult<ViewSponsorInCompetition>> GetSponsorViewAllApplyInCompOrEve(SponsorInCompetitionRequestModel request, int userId, int sponsorId)
        {
            //Take List Apply Of User Sort Data by Date
            IQueryable<SponsorInCompetition> query = null;

            if (request.UnversityId.HasValue)
            {
                query = from c in context.Clubs
                        where c.UniversityId == request.UnversityId.Value
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == c.Id && cic.IsOwner == true 
                        from compe in context.Competitions
                        where compe.Id == cic.CompetitionId
                        from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId
                              && sic.CompetitionId == compe.Id
                              && sic.UserId == userId
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }

            if (request.UnversityId.HasValue && request.ClubId.HasValue)
            {
                query = from c in context.Clubs
                        where c.UniversityId == request.UnversityId.Value && c.Id == request.ClubId.Value   
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == c.Id && cic.IsOwner == true 
                        from compe in context.Competitions
                        where compe.Id == cic.CompetitionId
                        from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId
                              && sic.CompetitionId == compe.Id
                              && sic.UserId == userId
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }

            if (request.UnversityId.HasValue && request.status.HasValue)
            {
                query = from c in context.Clubs
                        where c.UniversityId == request.UnversityId.Value
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == c.Id && cic.IsOwner == true
                        from compe in context.Competitions
                        where compe.Id == cic.CompetitionId
                        from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId
                              && sic.CompetitionId == compe.Id
                              && sic.UserId == userId
                              && sic.Status == request.status
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }

            if (request.UnversityId.HasValue && request.ClubId.HasValue && request.status.HasValue)
            {
                query = from c in context.Clubs
                        where c.UniversityId == request.UnversityId.Value && c.Id == request.ClubId.Value
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == c.Id && cic.IsOwner == true
                        from compe in context.Competitions
                        where compe.Id == cic.CompetitionId
                        from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId
                              && sic.CompetitionId == compe.Id
                              && sic.UserId == userId
                              && sic.Status == request.status
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }

            if (!request.UnversityId.HasValue && !request.ClubId.HasValue && request.status.HasValue)
            {
                query = from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId
                              && sic.UserId == userId
                              && sic.Status == request.status.Value
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }

            if (!request.UnversityId.HasValue && !request.ClubId.HasValue && !request.status.HasValue)
            {
                query = from sic in context.SponsorInCompetitions
                        where sic.SponsorId == sponsorId                              
                              && sic.UserId == userId                             
                              && sic.CreateTime < new LocalTime().GetLocalTime().DateTime
                        orderby sic.CreateTime descending
                        select sic;
            }


     
            int totalCount = await query.CountAsync();

            List<ViewSponsorInCompetition> list_vsic = new List<ViewSponsorInCompetition>();

            List<SponsorInCompetition> sponsorInCompetitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (SponsorInCompetition sponsorInCompetition in sponsorInCompetitions)
            {
                //user 
                User user = await (from us in context.Users
                                   where us.Id == sponsorInCompetition.UserId
                                   select us).FirstOrDefaultAsync();
                //sponsor
                Sponsor sponsor = await (from s in context.Sponsors
                                         where s.Id == sponsorInCompetition.SponsorId
                                         select s).FirstOrDefaultAsync();

                ViewSponsorInCompetition vsic = new ViewSponsorInCompetition()
                {
                    Id = sponsorInCompetition.Id,
                    CompetitionId = sponsorInCompetition.CompetitionId,
                    SponsorId = sponsorInCompetition.SponsorId,
                    UserId = sponsorInCompetition.UserId,
                    Email = user.Email,
                    Fullname = user.Fullname,
                    CreateTime = (DateTime)sponsorInCompetition.CreateTime,
                    SponsorName = sponsor.Name,
                    SponsorLogo = sponsor.Logo,
                    Status = sponsorInCompetition.Status
                };
                list_vsic.Add(vsic);
            }
            return (list_vsic.Count > 0) ? new PagingResult<ViewSponsorInCompetition>(list_vsic, totalCount, request.CurrentPage, request.PageSize) : null;


        }
    }
}
