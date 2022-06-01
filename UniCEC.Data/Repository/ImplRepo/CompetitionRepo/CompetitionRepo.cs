using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.RequestModels;
using System.Linq;
using System.Collections.Generic;
using UniCEC.Data.Enum;
using System;
using UniCEC.Data.Common;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRepo
{
    public class CompetitionRepo : Repository<Competition>, ICompetitionRepo
    {
        public CompetitionRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CheckExistCode(string code)
        {
            bool check = false;
            Competition competition = await context.Competitions.FirstOrDefaultAsync(x => x.SeedsCode.Equals(code));
            if (competition != null)
            {
                check = true;
                return check;
            }
            return check;
        }

        //Get EVENT or COMPETITION by conditions
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            //
            var query = from cic in context.CompetitionInClubs
                        where cic.ClubId == request.ClubId
                        from comp in context.Competitions
                        where cic.CompetitionId == comp.Id
                        select comp;
            //status
            //if (request.Status.HasValue) query = query.Where(comp => comp.Status == request.Status);
            //Public
            if (request.Public.HasValue) query = query.Where(comp => comp.Public == request.Public);
            //Serach Event
            if (request.Event.HasValue)
            {
                if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfTeam == 0);
            }
            int totalCount = query.Count();
            //
            List<ViewCompetition> Competitions = new List<ViewCompetition>();

            List<Competition> list_Competition = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (Competition compe in list_Competition)
            {
                //lấy department ID
                List<ViewDeparmentInComp> list_View_DeparmentInComp = new List<ViewDeparmentInComp>();

                List<CompetitionInDepartment> list_CompetitionInDepartment = (List<CompetitionInDepartment>)compe.CompetitionInDepartments;

                //lấy Club Owner
                List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                          where compe.Id == cic.CompetitionId
                                                          select cic).ToListAsync();

                List<ViewClubInComp> List_vcip = new List<ViewClubInComp>();

                if (clubList.Count > 0)
                {
                    foreach (var competitionInClub in clubList)
                    {
                        Club club = await (from c in context.Clubs
                                           where c.Id == competitionInClub.ClubId
                                           select c).FirstOrDefaultAsync();

                        ViewClubInComp vcip = new ViewClubInComp()
                        {
                            Id = club.Id,
                            Name = club.Name,
                            Image = club.Image
                        };

                        List_vcip.Add(vcip);
                    }
                }

                foreach (CompetitionInDepartment competitionInDepartment in list_CompetitionInDepartment)
                {
                    Department dep = await (from d in context.Departments
                                            where d.Id == competitionInDepartment.DepartmentId
                                            select d).FirstOrDefaultAsync();
                    if (dep != null)
                    {
                        ViewDeparmentInComp vdic = new ViewDeparmentInComp()
                        {
                            Id = dep.Id,
                            Name = dep.Name,
                        };
                        list_View_DeparmentInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewCompetition vc = new ViewCompetition()
                {
                    CompetitionId = compe.Id,
                    Name = compe.Name,
                    CompetitionTypeId = compe.CompetitionTypeId,
                    Public = compe.Public,
                    View = compe.View,
                    CreateTime = compe.CreateTime,
                    IsSponsor = compe.IsSponsor,
                    DepartmentInCompetition = list_View_DeparmentInComp,
                    ClubInCompetition = List_vcip

                };
                Competitions.Add(vc);
            }//end each competition

            return (Competitions.Count != 0) ? new PagingResult<ViewCompetition>(Competitions, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //Get top 3 EVENT or COMPETITION by Status
        //gần ngày hiện tại
        //Thuộc Club
        public async Task<List<ViewCompetition>> GetTop3CompOrEve(int? ClubId, bool? Event, CompetitionStatus? Status, bool? Public)
        {
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();
            //
            IQueryable<Competition> query;


            if (ClubId.HasValue)
            {
                query = from cic in context.CompetitionInClubs
                        where cic.ClubId == ClubId
                        join comp in context.Competitions on cic.CompetitionId equals comp.Id
                        where comp.StartTime >= localTime.DateTime
                        orderby comp.StartTime
                        select comp;
            }
            else
            {
                query = from comp in context.Competitions
                        where comp.StartTime >= localTime.DateTime
                        orderby comp.StartTime
                        select comp;
            }

            //Serach Event
            if (Event.HasValue)
            {
                if (Event.Value == true) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.NumberOfTeam == 0);
            }
            //Public
            if (Public.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Public == Public);
            //Status
            //if (Status.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Status == Status);

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            List<Competition> list_Competition = await query.Take(3).ToListAsync();

            foreach (Competition compe in list_Competition)
            {
                //lấy department ID
                List<ViewDeparmentInComp> list_View_DeparmentInComp = new List<ViewDeparmentInComp>();

                List<CompetitionInDepartment> list_CompetitionInDepartment = (List<CompetitionInDepartment>)compe.CompetitionInDepartments;

                //lấy Club Owner
                List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                          where compe.Id == cic.CompetitionId
                                                          select cic).ToListAsync();

                List<ViewClubInComp> List_vcip = new List<ViewClubInComp>();

                if (clubList.Count > 0)
                {
                    foreach (var competitionInClub in clubList)
                    {
                        Club club = await (from c in context.Clubs
                                           where c.Id == competitionInClub.ClubId
                                           select c).FirstOrDefaultAsync();

                        ViewClubInComp vcip = new ViewClubInComp()
                        {
                            Id = club.Id,
                            Name = club.Name,
                            Image = club.Image
                        };

                        List_vcip.Add(vcip);
                    }
                }

                //lấy competition type name
                CompetitionType competitionType = await (from c in context.Competitions
                                                         where c.Id == compe.Id
                                                         from ct in context.CompetitionTypes
                                                         where ct.Id == c.CompetitionTypeId
                                                         select ct).FirstOrDefaultAsync();

                string competitionTypeName = competitionType.TypeName;

                foreach (CompetitionInDepartment competitionInDepartment in list_CompetitionInDepartment)
                {
                    Department dep = await (from d in context.Departments
                                            where d.Id == competitionInDepartment.DepartmentId
                                            select d).FirstOrDefaultAsync();
                    if (dep != null)
                    {
                        ViewDeparmentInComp vdic = new ViewDeparmentInComp()
                        {
                            Id = dep.Id,
                            Name = dep.Name,
                        };
                        list_View_DeparmentInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewCompetition vc = new ViewCompetition()
                {
                    CompetitionId = compe.Id,
                    Name = compe.Name,
                    CompetitionTypeId = compe.CompetitionTypeId,
                    CompetitionTypeName = competitionTypeName,
                    Public = compe.Public,
                    View = compe.View,
                    CreateTime = compe.CreateTime,
                    IsSponsor = compe.IsSponsor,
                    DepartmentInCompetition = list_View_DeparmentInComp,
                    ClubInCompetition = List_vcip
                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count > 0) ? competitions : null;
        }

        // Nhat
        public async Task<bool> CheckIsPublic(int id)
        {
            var query = from c in context.Competitions
                        where c.Id.Equals(id)
                        select c.Public;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetUniversityByCompetition(int id)
        {
            var query = from cic in context.CompetitionInClubs
                        join c in context.Clubs on cic.ClubId equals c.Id
                        where cic.CompetitionId.Equals(id)
                        select new { c };

            List<int> universityIds = await query.Select(x => x.c.UniversityId).ToListAsync();

            return (universityIds.Count() > 0) ? universityIds : null;
        }
    }
}
