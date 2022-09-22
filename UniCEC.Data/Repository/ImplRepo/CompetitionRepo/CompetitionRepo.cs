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
using UniCEC.Data.ViewModels.Entities.Competition;


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
        //Data return sort by Creatime
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request, int universityId)
        {
            //
            //IQueryable<Competition> query;
            //LocalTime
            DateTimeOffset currentTime = new LocalTime().GetLocalTime().DateTime;

            var query = (!request.ClubId.HasValue)
                        ? (from comp in context.Competitions
                           join ct in context.CompetitionTypes on comp.CompetitionTypeId equals ct.Id
                           where comp.CreateTime < currentTime
                           orderby comp.CreateTime descending
                           select new { comp, ct })
                        : (from comp in context.Competitions
                           join ct in context.CompetitionTypes on comp.CompetitionTypeId equals ct.Id
                           join cic in context.CompetitionInClubs on comp.Id equals cic.CompetitionId
                           where comp.CreateTime < currentTime && cic.ClubId.Equals(request.ClubId.Value)
                           orderby comp.CreateTime descending
                           select new { comp, ct });

            //if (request.ClubId.HasValue)
            //{
            //    //join cic in context.CompetitionInClubs on comp.Id equals cic.CompetitionId
            //    query = query.Join(context.CompetitionInClubs, selector => selector.comp.Id, cic => cic.CompetitionId
            //    , (selector, cic) => new { selector, cic }).Where(selector => selector.cic.Id.Equals(request.ClubId.Value));
            //}

            ////search có clubId 
            //if (request.ClubId.HasValue)
            //{
            //    query = from c in context.Clubs
            //            where c.Id == request.ClubId
            //            from cic in context.CompetitionInClubs
            //            where cic.ClubId == c.Id
            //            from comp in context.Competitions
            //            where cic.CompetitionId == comp.Id //&& comp.UniversityId == ((request.UniversityId.HasValue) ? request.UniversityId : universityId)
            //            orderby comp.CreateTime descending
            //            select comp;
            //}

            //else
            //{
            //    query = from comp in context.Competitions
            //                // where comp.UniversityId == ((request.UniversityId.HasValue) ? request.UniversityId : universityId) // nếu truyền vào id Uni thì ra còn kh thì thôi lấy trường mình
            //            orderby comp.CreateTime descending
            //            select comp;
            //}

            //status     
            if (request.Statuses != null) query = query.Where(selector => request.Statuses.Contains(selector.comp.Status));

            //Scope
            if (request.Scope.HasValue) query = query.Where(selector => selector.comp.Scope.Equals(request.Scope.Value));

            //UniversityId
            if (request.UniversityId.HasValue) query = query.Where(selector => selector.comp.UniversityId == request.UniversityId.Value);


            //if (request.Scope.HasValue)
            //{
            //    //Liên Trường
            //    if (request.Scope == CompetitionScopeStatus.InterUniversity)
            //    {
            //        query = query.Where(selector => selector.comp.Scope == request.Scope);
            //        if (request.UniversityId.HasValue)
            //        {
            //            query = query.Where(selector => selector.comp.UniversityId == request.UniversityId.Value);
            //        }
            //    }
            //    //Trong Trường 
            //    if (request.Scope == CompetitionScopeStatus.University)
            //    {
            //        query = query.Where(comp => comp.Scope == request.Scope);

            //        if (request.UniversityId.HasValue)
            //        {
            //            query = query.Where(comp => comp.UniversityId == request.UniversityId.Value);
            //        }

            //    }
            //    //trong Câu Lạc Bộ
            //    if (request.Scope == CompetitionScopeStatus.Club)
            //    {
            //        query = query.Where(comp => comp.Scope == request.Scope);

            //        if (request.UniversityId.HasValue)
            //        {
            //            query = query.Where(comp => comp.UniversityId == request.UniversityId.Value);
            //        }
            //    }
            //}

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(selector => selector.comp.Name.ToLower().Contains(request.Name.ToLower()));

            //Serach Event
            if (request.Event.HasValue)
            {
                query = (request.Event.Value.Equals(true))
                    ? query.Where(selector => selector.comp.NumberOfTeam.Equals(0))
                    : query = query.Where(selector => !selector.comp.NumberOfTeam.Equals(0));
            }

            //{
            //    if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfTeam == 0);

            //    if (request.Event.Value == false) query = query.Where(comp => comp.NumberOfTeam != 0);
            //}

            //View Most
            if (request.ViewMost.Equals(true)) query = query.OrderByDescending(selector => selector.comp.View);


            int totalCount = query.Count();
            //
            //List<ViewCompetition> competitions = new List<ViewCompetition>();

            List<ViewCompetition> competitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                        .Select(selector => new ViewCompetition()
                                                        {
                                                            Id = selector.comp.Id,
                                                            Name = selector.comp.Name,
                                                            CompetitionTypeId = selector.comp.CompetitionTypeId,
                                                            CompetitionTypeName = selector.ct.TypeName,
                                                            Scope = selector.comp.Scope,
                                                            Status = selector.comp.Status,
                                                            View = selector.comp.View,
                                                            CreateTime = selector.comp.CreateTime,
                                                            StartTime = selector.comp.StartTime,
                                                            IsSponsor = selector.comp.IsSponsor,
                                                            UniversityId = selector.comp.UniversityId,
                                                            IsEvent = (selector.comp.NumberOfTeam == 0) ? true : false
                                                        }).ToListAsync();

            foreach (ViewCompetition compe in competitions)
            {
                compe.ClubInCompetition = await (from cic in context.CompetitionInClubs
                                                 join club in context.Clubs on cic.ClubId equals club.Id
                                                 where cic.CompetitionId.Equals(compe.Id)
                                                 select new ViewClubInComp()
                                                 {
                                                     Id = cic.Id,
                                                     ClubId = cic.ClubId,
                                                     Fanpage = club.ClubFanpage,
                                                     Image = club.Image,
                                                     IsOwner = cic.IsOwner,
                                                     Name = club.Name
                                                 }).ToListAsync();

                compe.MajorInCompetition = await (from cim in context.CompetitionInMajors
                                                  join m in context.Majors on cim.MajorId equals m.Id
                                                  where cim.CompetitionId.Equals(compe.Id)
                                                  select new ViewMajorInComp()
                                                  {
                                                      Id = cim.Id,
                                                      MajorId = cim.MajorId,
                                                      Name = m.Name
                                                  }).ToListAsync();

                //total activity
                int totalCompetitionActivity = await (from ca in context.CompetitionActivities
                                                      where ca.CompetitionId == compe.Id
                                                      select ca).CountAsync();

                //total activity completed
                int totalCompetitionActivityCompleted = await (from ca in context.CompetitionActivities
                                                               where ca.CompetitionId == compe.Id && ca.Status == CompetitionActivityStatus.Completed
                                                               select ca).CountAsync();

                compe.totalCompetitionActivity = totalCompetitionActivity;
                compe.totalCompetitionActivityCompleted = totalCompetitionActivityCompleted;

                ////lấy department ID 
                //List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

                //var query_List_CompetitionInDepartment = compe.CompetitionInMajors;
                //List<CompetitionInMajor> listCompetitionInMajor = query_List_CompetitionInDepartment.ToList();

                ////lấy Club Owner
                //List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                //                                          where compe.Id == cic.CompetitionId
                //                                          select cic).ToListAsync();

                //List<ViewClubInComp> List_vcip = new List<ViewClubInComp>();

                //if (clubList.Count > 0)
                //{
                //    foreach (var competitionInClub in clubList)
                //    {
                //        Club club = await (from c in context.Clubs
                //                           where c.Id == competitionInClub.ClubId
                //                           select c).FirstOrDefaultAsync();

                //        ViewClubInComp vcip = new ViewClubInComp()
                //        {
                //            Id = competitionInClub.Id,
                //            ClubId = club.Id,
                //            Name = club.Name,
                //            Image = club.Image,
                //            Fanpage = club.ClubFanpage,
                //            IsOwner = competitionInClub.IsOwner
                //        };

                //        List_vcip.Add(vcip);
                //    }
                //}

                //foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
                //{
                //    Major major = await (from m in context.Majors
                //                         where m.Id == competitionInMajor.MajorId
                //                         select m).FirstOrDefaultAsync();
                //    if (major != null)
                //    {
                //        ViewMajorInComp vdic = new ViewMajorInComp()
                //        {
                //            Id = major.Id,
                //            Name = major.Name,
                //        };
                //        listViewMajorInComp.Add(vdic);
                //    }
                //}



                ////cb tạo View
                //ViewCompetition vc = new ViewCompetition()
                //{
                //    Id = compe.Id,
                //    CompetitionTypeName = compe.CompetitionType.TypeName,
                //    Name = compe.Name,
                //    CompetitionTypeId = compe.CompetitionTypeId,
                //    Scope = compe.Scope,
                //    Status = compe.Status,
                //    View = compe.View,
                //    CreateTime = compe.CreateTime,
                //    StartTime = compe.StartTime,
                //    IsSponsor = compe.IsSponsor,
                //    MajorInCompetition = listViewMajorInComp,
                //    ClubInCompetition = List_vcip,
                //    UniversityId = compe.UniversityId,
                //    IsEvent = (compe.NumberOfTeam == 0) ? true : false,
                //    totalCompetitionActivity = totalCompetitionActivity,
                //    totalCompetitionActivityCompleted = totalCompetitionActivityCompleted
                //};
                //competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //Get top EVENT or COMPETITION by Status       
        public async Task<List<ViewTopCompetition>> GetTopCompOrEve(int ClubId, bool? Event/*, CompetitionStatus? Status*/, CompetitionScopeStatus? Scope, int Top)
        {
            List<ViewTopCompetition> competitions = new List<ViewTopCompetition>();

            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();
            //
            IQueryable<Competition> query;

            query = from cic in context.CompetitionInClubs
                    join comp in context.Competitions on cic.CompetitionId equals comp.Id
                    where comp.StartTime > localTime.DateTime
                     && cic.ClubId == ClubId
                     && comp.Status != CompetitionStatus.Cancel
                     && comp.Status != CompetitionStatus.Draft
                     && comp.Status != CompetitionStatus.Pending
                     && comp.Status != CompetitionStatus.PendingReview
                     && comp.Status != CompetitionStatus.Approve
                    orderby comp.StartTime
                    select comp;

            //Serach Event
            if (Event.HasValue)
            {
                if (Event.Value == true) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.NumberOfTeam == 0);
                if (Event.Value == false) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.NumberOfTeam != 0);

            }
            //Scope
            if (Scope.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Scope == Scope);



            List<Competition> list_Competition = await query.Take(Top).ToListAsync();

            //vẫn thiếu chưa đủ theo Top thì sẽ chạy lấy bù
            if (list_Competition.Count < Top)
            {
                var subQuery = from cic in context.CompetitionInClubs
                               join comp in context.Competitions on cic.CompetitionId equals comp.Id
                               where comp.StartTime < localTime.DateTime
                                && cic.ClubId == ClubId
                                && comp.Status != CompetitionStatus.Cancel
                                && comp.Status != CompetitionStatus.Draft
                                && comp.Status != CompetitionStatus.Pending
                                && comp.Status != CompetitionStatus.PendingReview
                                && comp.Status != CompetitionStatus.Approve
                               orderby comp.StartTime
                               select comp;

                //Serach Event
                if (Event.HasValue)
                {
                    if (Event.Value == true) subQuery = (IOrderedQueryable<Competition>)subQuery.Where(comp => comp.NumberOfTeam == 0);
                    if (Event.Value == false) subQuery = (IOrderedQueryable<Competition>)subQuery.Where(comp => comp.NumberOfTeam != 0);
                }
                //Scope
                if (Scope.HasValue) subQuery = (IOrderedQueryable<Competition>)subQuery.Where(comp => comp.Scope == Scope);

                List<Competition> list_SubQuery = await subQuery.Take(Top - list_Competition.Count).ToListAsync();

                foreach (Competition comp in list_SubQuery)
                {
                    list_Competition.Add(comp);
                }
            }

            foreach (Competition compe in list_Competition)
            {

                //lấy major ID
                List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

                var queryListCompetitionInMajor = compe.CompetitionInMajors;
                List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

                //lấy Club Owner
                List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                          where compe.Id == cic.CompetitionId
                                                          select cic).ToListAsync();

                List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

                if (clubList.Count > 0)
                {
                    foreach (var competitionInClub in clubList)
                    {
                        Club club = await (from c in context.Clubs
                                           where c.Id == competitionInClub.ClubId
                                           select c).FirstOrDefaultAsync();

                        ViewClubInComp vcip = new ViewClubInComp()
                        {
                            Id = competitionInClub.Id,
                            ClubId = club.Id,
                            Name = club.Name,
                            Image = club.Image,
                            Fanpage = club.ClubFanpage,
                            IsOwner = competitionInClub.IsOwner
                        };

                        listVcip.Add(vcip);
                    }
                }

                //lấy competition type name
                CompetitionType competitionType = await (from c in context.Competitions
                                                         where c.Id == compe.Id
                                                         from ct in context.CompetitionTypes
                                                         where ct.Id == c.CompetitionTypeId
                                                         select ct).FirstOrDefaultAsync();

                string competitionTypeName = competitionType.TypeName;

                foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
                {
                    Major major = await (from m in context.Majors
                                         where m.Id == competitionInMajor.MajorId
                                         select m).FirstOrDefaultAsync();
                    if (major != null)
                    {
                        ViewMajorInComp vdic = new ViewMajorInComp()
                        {
                            Id = competitionInMajor.Id, //fix
                            MajorId = competitionInMajor.MajorId,
                            Name = major.Name,
                        };
                        listViewMajorInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewTopCompetition vc = new ViewTopCompetition()
                {
                    Id = compe.Id,
                    Name = compe.Name,
                    CompetitionTypeId = compe.CompetitionTypeId,
                    CompetitionTypeName = competitionTypeName,
                    Scope = compe.Scope,
                    Status = compe.Status,
                    View = compe.View,
                    CreateTime = compe.CreateTime,
                    StartTime = compe.StartTime,
                    IsSponsor = compe.IsSponsor,
                    MajorInCompetition = listViewMajorInComp,
                    ClubInCompetition = listVcip,
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false
                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count > 0) ? competitions : null;
        }

        public async Task<PagingResult<ViewCompetition>> GetCompOrEveByAdminUni(AdminUniGetCompetitionRequestModel request, int universityId)
        {
            var query = from c in context.Competitions
                        join ct in context.CompetitionTypes on c.CompetitionTypeId equals ct.Id
                        where c.UniversityId == universityId && c.Status == CompetitionStatus.PendingReview
                        select new { c, ct };
            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(selector => selector.c.Name.ToLower().Contains(request.Name.ToLower()));

            int totalCount = query.Count();

            List<ViewCompetition> competitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(selector => new ViewCompetition()
                                                    {
                                                        Id = selector.c.Id,
                                                        Name = selector.c.Name,
                                                        CompetitionTypeId = selector.c.CompetitionTypeId,
                                                        CompetitionTypeName = selector.ct.TypeName,
                                                        Scope = selector.c.Scope,
                                                        Status = selector.c.Status,
                                                        View = selector.c.View,
                                                        CreateTime = selector.c.CreateTime,
                                                        StartTime = selector.c.StartTime,
                                                        IsSponsor = selector.c.IsSponsor,
                                                        UniversityId = selector.c.UniversityId,
                                                        IsEvent = (selector.c.NumberOfTeam == 0) ? true : false
                                                    }).ToListAsync();

            foreach (var competition in competitions)
            {
                competition.ClubInCompetition = await (from cic in context.CompetitionInClubs
                                                       join club in context.Clubs on cic.ClubId equals club.Id
                                                       where cic.CompetitionId.Equals(competition.Id)
                                                       select new ViewClubInComp()
                                                       {
                                                           Id = cic.Id,
                                                           ClubId = cic.ClubId,
                                                           Fanpage = club.ClubFanpage,
                                                           Image = club.Image,
                                                           IsOwner = cic.IsOwner,
                                                           Name = club.Name
                                                       }).ToListAsync();

                competition.MajorInCompetition = await (from cim in context.CompetitionInMajors
                                                        join m in context.Majors on cim.MajorId equals m.Id
                                                        where cim.CompetitionId.Equals(competition.Id)
                                                        select new ViewMajorInComp()
                                                        {
                                                            Id = cim.Id,
                                                            MajorId = cim.MajorId,
                                                            Name = m.Name
                                                        }).ToListAsync();
            }

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

            //foreach (ViewCompetition compe in competitions)
            //{

            //    //lấy major ID
            //    List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

            //    var queryListCompetitionInMajor = compe.CompetitionInMajors;
            //    List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

            //    //lấy Club Owner
            //    List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
            //                                              where compe.Id == cic.CompetitionId
            //                                              select cic).ToListAsync();

            //    List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

            //    if (clubList.Count > 0)
            //    {
            //        foreach (var competitionInClub in clubList)
            //        {
            //            Club club = await (from c in context.Clubs
            //                               where c.Id == competitionInClub.ClubId
            //                               select c).FirstOrDefaultAsync();

            //            ViewClubInComp vcip = new ViewClubInComp()
            //            {
            //                Id = competitionInClub.Id,
            //                ClubId = club.Id,
            //                Name = club.Name,
            //                Image = club.Image,
            //                Fanpage = club.ClubFanpage,
            //                IsOwner = competitionInClub.IsOwner
            //            };

            //            listVcip.Add(vcip);
            //        }
            //    }

            //    //lấy competition type name
            //    CompetitionType competitionType = await (from c in context.Competitions
            //                                             where c.Id == compe.Id
            //                                             from ct in context.CompetitionTypes
            //                                             where ct.Id == c.CompetitionTypeId
            //                                             select ct).FirstOrDefaultAsync();

            //    string competitionTypeName = competitionType.TypeName;

            //    foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
            //    {
            //        Major major = await (from m in context.Majors
            //                             where m.Id == competitionInMajor.MajorId
            //                             select m).FirstOrDefaultAsync();
            //        if (major != null)
            //        {
            //            ViewMajorInComp vdic = new ViewMajorInComp()
            //            {
            //                Id = major.Id,
            //                Name = major.Name,
            //            };
            //            listViewMajorInComp.Add(vdic);
            //        }
            //    }

            //    //cb tạo View
            //    ViewCompetition vc = new ViewCompetition()
            //    {
            //        Id = compe.Id,
            //        Name = compe.Name,
            //        CompetitionTypeId = compe.CompetitionTypeId,
            //        CompetitionTypeName = competitionTypeName,
            //        Scope = compe.Scope,
            //        Status = compe.Status,
            //        View = compe.View,
            //        CreateTime = compe.CreateTime,
            //        StartTime = compe.StartTime,
            //        IsSponsor = compe.IsSponsor,
            //        MajorInCompetition = listViewMajorInComp,
            //        ClubInCompetition = listVcip,
            //        UniversityId = compe.UniversityId,
            //        IsEvent = (compe.NumberOfTeam == 0) ? true : false
            //    };
            //    competitions.Add(vc);
            //}//end each competition

            //return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<Competition> GetCompetitionBySeedsCode(string seedsCode)
        {
            return await (from c in context.Competitions
                          where c.SeedsCode == seedsCode
                          select c).FirstOrDefaultAsync();
        }

        public async Task<PagingResult<ViewCompetition>> GetCompOrEveUnAuthorize(CompetitionUnAuthorizeRequestModel request, List<CompetitionStatus> listCompetitionStatus)
        {
            // new part
            var query = from c in context.Competitions
                        join ct in context.CompetitionTypes on c.CompetitionTypeId equals ct.Id
                        where c.Scope.Equals(CompetitionScopeStatus.InterUniversity)
                        select new { c, ct };

            if (request.MostView.Equals(true)) query = query.OrderByDescending(selector => selector.c.View);

            if (request.NearlyDate.Equals(true))
                query = query.Where(selector =>
                    selector.c.CreateTime < new LocalTime().GetLocalTime().DateTime).OrderByDescending(selector => selector.c.CreateTime);


            if (listCompetitionStatus.Count > 0) query = query.Where(selector => listCompetitionStatus.Contains(selector.c.Status));

            if (request.Sponsor.HasValue) query = query.Where(selector => selector.c.IsSponsor.Equals(request.Sponsor.Value));

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(selector => selector.c.Name.ToLower().Contains(request.Name.ToLower()));

            int totalCount = query.Count();

            //
            List<ViewCompetition> competitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(selector => new ViewCompetition()
                                                    {
                                                        Id = selector.c.Id,
                                                        Name = selector.c.Name,
                                                        CompetitionTypeId = selector.c.CompetitionTypeId,
                                                        CompetitionTypeName = selector.ct.TypeName,
                                                        Scope = selector.c.Scope,
                                                        Status = selector.c.Status,
                                                        View = selector.c.View,
                                                        CreateTime = selector.c.CreateTime,
                                                        StartTime = selector.c.StartTime,
                                                        IsSponsor = selector.c.IsSponsor,
                                                        UniversityId = selector.c.UniversityId,
                                                        IsEvent = (selector.c.NumberOfTeam == 0) ? true : false
                                                    }).ToListAsync();

            foreach (var competition in competitions)
            {
                competition.ClubInCompetition = await (from cic in context.CompetitionInClubs
                                                       join club in context.Clubs on cic.ClubId equals club.Id
                                                       where cic.CompetitionId.Equals(competition.Id)
                                                       select new ViewClubInComp()
                                                       {
                                                           Id = cic.Id,
                                                           ClubId = cic.ClubId,
                                                           Fanpage = club.ClubFanpage,
                                                           Image = club.Image,
                                                           IsOwner = cic.IsOwner,
                                                           Name = club.Name
                                                       }).ToListAsync();

                competition.MajorInCompetition = await (from cim in context.CompetitionInMajors
                                                        join m in context.Majors on cim.MajorId equals m.Id
                                                        where cim.CompetitionId.Equals(competition.Id)
                                                        select new ViewMajorInComp()
                                                        {
                                                            Id = cim.Id,
                                                            MajorId = cim.MajorId,
                                                            Name = m.Name
                                                        }).ToListAsync();
            }

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

            // old part

            //List<Competition> listCompetition = new List<Competition>();

            ////lấy cả 2 
            ////có vấn đề 
            //if (request.MostView.HasValue && request.NearlyDate.HasValue)
            //{
            //    listCompetition = await (from c in context.Competitions
            //                             where c.Scope == CompetitionScopeStatus.InterUniversity
            //                             orderby c.View descending, c.CreateTime descending
            //                             select c).ToListAsync();
            //}

            ////lấy thời gian đăng ký gần hiện tại
            //if (!request.MostView.HasValue && request.NearlyDate.HasValue)
            //{
            //    listCompetition = await (from c in context.Competitions
            //                             where c.Scope == CompetitionScopeStatus.InterUniversity
            //                             orderby c.CreateTime descending
            //                             select c).ToListAsync();
            //}

            ////lấy số lượng view nhiều 
            //if (request.MostView.HasValue && !request.NearlyDate.HasValue)
            //{
            //    listCompetition = await (from c in context.Competitions
            //                             where c.Scope == CompetitionScopeStatus.InterUniversity
            //                             orderby c.View descending
            //                             select c).ToListAsync();
            //}

            ////Không sort theo cái nào cả
            //if (!request.MostView.HasValue && !request.NearlyDate.HasValue)
            //{
            //    listCompetition = await (from c in context.Competitions
            //                             where c.Scope == CompetitionScopeStatus.InterUniversity
            //                             select c).ToListAsync();
            //}

            ////
            //if (listCompetitionStatus.Count > 0) listCompetition = listCompetition.Where(comp => listCompetitionStatus.Contains(comp.Status)).ToList();

            ////
            //if (request.Sponsor.HasValue) listCompetition = listCompetition.Where(comp => comp.IsSponsor == request.Sponsor.Value).ToList();

            ////
            //if (!string.IsNullOrEmpty(request.Name)) listCompetition = listCompetition.Where(comp => comp.Name.ToLower().Contains(request.Name.ToLower())).ToList();

            //int totalCount = listCompetition.Count();

            ////
            //listCompetition = listCompetition.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

            //List<ViewCompetition> competitions = new List<ViewCompetition>();

            //foreach (Competition compe in listCompetition)
            //{

            //    //lấy major ID
            //    List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

            //    var queryListCompetitionInMajor = compe.CompetitionInMajors;
            //    List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

            //    //lấy Club Owner
            //    List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
            //                                              where compe.Id == cic.CompetitionId
            //                                              select cic).ToListAsync();

            //    List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

            //    if (clubList.Count > 0)
            //    {
            //        foreach (var competitionInClub in clubList)
            //        {
            //            Club club = await (from c in context.Clubs
            //                               where c.Id == competitionInClub.ClubId
            //                               select c).FirstOrDefaultAsync();

            //            ViewClubInComp vcip = new ViewClubInComp()
            //            {
            //                Id = competitionInClub.Id,
            //                ClubId = club.Id,
            //                Name = club.Name,
            //                Image = club.Image,
            //                Fanpage = club.ClubFanpage,
            //                IsOwner = competitionInClub.IsOwner
            //            };

            //            listVcip.Add(vcip);
            //        }
            //    }

            //    //lấy competition type name
            //    CompetitionType competitionType = await (from c in context.Competitions
            //                                             where c.Id == compe.Id
            //                                             from ct in context.CompetitionTypes
            //                                             where ct.Id == c.CompetitionTypeId
            //                                             select ct).FirstOrDefaultAsync();

            //    string competitionTypeName = competitionType.TypeName;

            //    foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
            //    {
            //        Major major = await (from m in context.Majors
            //                             where m.Id == competitionInMajor.MajorId
            //                             select m).FirstOrDefaultAsync();
            //        if (major != null)
            //        {
            //            ViewMajorInComp vdic = new ViewMajorInComp()
            //            {
            //                Id = major.Id,
            //                Name = major.Name,
            //            };
            //            listViewMajorInComp.Add(vdic);
            //        }
            //    }

            //    //cb tạo View
            //    ViewCompetition vc = new ViewCompetition()
            //    {
            //        Id = compe.Id,
            //        Name = compe.Name,
            //        CompetitionTypeId = compe.CompetitionTypeId,
            //        CompetitionTypeName = competitionTypeName,
            //        Scope = compe.Scope,
            //        Status = compe.Status,
            //        View = compe.View,
            //        CreateTime = compe.CreateTime,
            //        StartTime = compe.StartTime,
            //        IsSponsor = compe.IsSponsor,
            //        MajorInCompetition = listViewMajorInComp,
            //        ClubInCompetition = listVcip,
            //        UniversityId = compe.UniversityId,
            //        IsEvent = (compe.NumberOfTeam == 0) ? true : false
            //    };
            //    competitions.Add(vc);
            //}//end each competition

            //return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewCompetition>> GetCompsOrEvesStudentJoin(GetStudentJoinCompOrEve request, int userId)
        {
            var query = from c in context.Competitions
                        join ct in context.CompetitionTypes on c.CompetitionTypeId equals ct.Id
                        join p in context.Participants on c.Id equals p.CompetitionId
                        where p.StudentId == userId
                              && c.CreateTime < new LocalTime().GetLocalTime().DateTime
                              && c.Status != CompetitionStatus.Draft
                              && c.Status != CompetitionStatus.Cancel
                              && c.Status != CompetitionStatus.PendingReview
                              && c.Status != CompetitionStatus.Approve
                        orderby c.CreateTime descending
                        select new { c, ct };
            //Scope
            if (request.Scope.HasValue) query = query.Where(selector => selector.c.Scope == request.Scope.Value);

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(selector => selector.c.Name.ToLower().Contains(request.Name.ToLower()));

            //Event
            if (request.Event.HasValue)
            {
                query = (request.Event.Value.Equals(true))
                    ? query.Where(selector => selector.c.NumberOfTeam.Equals(0))
                    : query.Where(selector => !selector.c.NumberOfTeam.Equals(0));
            }

            int totalCount = query.Count();

            List<ViewCompetition> competitions = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                    .Select(selector => new ViewCompetition()
                                                    {
                                                        Id = selector.c.Id,
                                                        Name = selector.c.Name,
                                                        CompetitionTypeId = selector.c.CompetitionTypeId,
                                                        CompetitionTypeName = selector.ct.TypeName,
                                                        Scope = selector.c.Scope,
                                                        Status = selector.c.Status,
                                                        View = selector.c.View,
                                                        CreateTime = selector.c.CreateTime,
                                                        StartTime = selector.c.StartTime,
                                                        IsSponsor = selector.c.IsSponsor,
                                                        UniversityId = selector.c.UniversityId,
                                                        IsEvent = (selector.c.NumberOfTeam == 0) ? true : false
                                                    }).ToListAsync();

            //
            //listCompetitionStudentJoin = listCompetitionStudentJoin.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

            //List<ViewCompetition> competitions = new List<ViewCompetition>();

            foreach (var competition in competitions)
            {
                competition.ClubInCompetition = await (from cic in context.CompetitionInClubs
                                                       join club in context.Clubs on cic.ClubId equals club.Id
                                                       where cic.CompetitionId.Equals(competition.Id)
                                                       select new ViewClubInComp()
                                                       {
                                                           Id = cic.Id,
                                                           ClubId = cic.ClubId,
                                                           Fanpage = club.ClubFanpage,
                                                           Image = club.Image,
                                                           IsOwner = cic.IsOwner,
                                                           Name = club.Name
                                                       }).ToListAsync();

                competition.MajorInCompetition = await (from cim in context.CompetitionInMajors
                                                        join m in context.Majors on cim.MajorId equals m.Id
                                                        where cim.CompetitionId.Equals(competition.Id)
                                                        select new ViewMajorInComp()
                                                        {
                                                            Id = cim.Id,
                                                            MajorId = cim.MajorId,
                                                            Name = m.Name
                                                        }).ToListAsync();
            }

            //foreach (ViewCompetition compe in competitions)
            //{

            //    //lấy major ID
            //    List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

            //    var queryListCompetitionInMajor = compe.CompetitionInMajors;
            //    List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

            //    //lấy Club Owner
            //    List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
            //                                              where compe.Id == cic.CompetitionId
            //                                              select cic).ToListAsync();

            //    List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

            //    if (clubList.Count > 0)
            //    {
            //        foreach (var competitionInClub in clubList)
            //        {
            //            Club club = await (from c in context.Clubs
            //                               where c.Id == competitionInClub.ClubId
            //                               select c).FirstOrDefaultAsync();

            //            ViewClubInComp vcip = new ViewClubInComp()
            //            {
            //                Id = competitionInClub.Id,
            //                ClubId = club.Id,
            //                Name = club.Name,
            //                Image = club.Image,
            //                Fanpage = club.ClubFanpage,
            //                IsOwner = competitionInClub.IsOwner
            //            };

            //            listVcip.Add(vcip);
            //        }
            //    }

            //    //lấy competition type name
            //    CompetitionType competitionType = await (from c in context.Competitions
            //                                             where c.Id == compe.Id
            //                                             from ct in context.CompetitionTypes
            //                                             where ct.Id == c.CompetitionTypeId
            //                                             select ct).FirstOrDefaultAsync();

            //    string competitionTypeName = competitionType.TypeName;

            //    foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
            //    {
            //        Major major = await (from m in context.Majors
            //                             where m.Id == competitionInMajor.MajorId
            //                             select m).FirstOrDefaultAsync();
            //        if (major != null)
            //        {
            //            ViewMajorInComp vdic = new ViewMajorInComp()
            //            {
            //                Id = major.Id,
            //                Name = major.Name,
            //            };
            //            listViewMajorInComp.Add(vdic);
            //        }
            //    }

            //    //cb tạo View
            //    ViewCompetition vc = new ViewCompetition()
            //    {
            //        Id = compe.Id,
            //        Name = compe.Name,
            //        CompetitionTypeId = compe.CompetitionTypeId,
            //        CompetitionTypeName = competitionTypeName,
            //        Scope = compe.Scope,
            //        Status = compe.Status,
            //        View = compe.View,
            //        CreateTime = compe.CreateTime,
            //        StartTime = compe.StartTime,
            //        IsSponsor = compe.IsSponsor,
            //        MajorInCompetition = listViewMajorInComp,
            //        ClubInCompetition = listVcip,
            //        UniversityId = compe.UniversityId,
            //        IsEvent = (compe.NumberOfTeam == 0) ? true : false

            //    };
            //    competitions.Add(vc);
            //}//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<ViewCompetition> GetCompOrEveStudentJoin(int competitionId, int userId)
        {
            Competition CompetitionStudentJoin = await (from p in context.Participants
                                                        where p.StudentId == userId
                                                        from c in context.Competitions
                                                        where c.Id == p.CompetitionId && c.Id == competitionId
                                                        select c).FirstOrDefaultAsync();
            if (CompetitionStudentJoin != null)
            {
                //lấy major ID
                List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

                var queryListCompetitionInMajor = CompetitionStudentJoin.CompetitionInMajors;
                List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

                //lấy Club Owner
                List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                          where CompetitionStudentJoin.Id == cic.CompetitionId
                                                          select cic).ToListAsync();

                List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

                if (clubList.Count > 0)
                {
                    foreach (var competitionInClub in clubList)
                    {
                        Club club = await (from c in context.Clubs
                                           where c.Id == competitionInClub.ClubId
                                           select c).FirstOrDefaultAsync();

                        ViewClubInComp vcip = new ViewClubInComp()
                        {
                            Id = competitionInClub.Id,
                            ClubId = club.Id,
                            Name = club.Name,
                            Image = club.Image,
                            Fanpage = club.ClubFanpage,
                            IsOwner = competitionInClub.IsOwner
                        };
                    }
                }

                //lấy competition type name
                CompetitionType competitionType = await (from c in context.Competitions
                                                         where c.Id == CompetitionStudentJoin.Id
                                                         from ct in context.CompetitionTypes
                                                         where ct.Id == c.CompetitionTypeId
                                                         select ct).FirstOrDefaultAsync();

                string competitionTypeName = competitionType.TypeName;

                foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
                {
                    Major major = await (from m in context.Majors
                                         where m.Id == competitionInMajor.MajorId
                                         select m).FirstOrDefaultAsync();
                    if (major != null)
                    {
                        ViewMajorInComp vdic = new ViewMajorInComp()
                        {
                            Id = competitionInMajor.Id, //fix
                            MajorId = competitionInMajor.MajorId,
                            Name = major.Name,
                        };
                        listViewMajorInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewCompetition vc = new ViewCompetition()
                {
                    Id = CompetitionStudentJoin.Id,
                    Name = CompetitionStudentJoin.Name,
                    CompetitionTypeId = CompetitionStudentJoin.CompetitionTypeId,
                    CompetitionTypeName = competitionTypeName,
                    Scope = CompetitionStudentJoin.Scope,
                    Status = CompetitionStudentJoin.Status,
                    View = CompetitionStudentJoin.View,
                    CreateTime = CompetitionStudentJoin.CreateTime,
                    StartTime = CompetitionStudentJoin.StartTime,
                    IsSponsor = CompetitionStudentJoin.IsSponsor,
                    MajorInCompetition = listViewMajorInComp,
                    ClubInCompetition = listVcip,
                    UniversityId = CompetitionStudentJoin.UniversityId,
                    IsEvent = (CompetitionStudentJoin.NumberOfTeam == 0) ? true : false
                };

                return vc;
            }
            else
            {
                return null;
            }
        }

        public async Task<int> GetNumberOfCompetitionOrEventInClubWithStatus(CompetitionStatus status, int clubId, bool isEvent)
        {
            List<Competition> listCompetitionOrEvent;
            if (isEvent)
            {
                //event
                listCompetitionOrEvent = await (from cic in context.CompetitionInClubs
                                                join c in context.Competitions on cic.CompetitionId equals c.Id
                                                where cic.IsOwner == true
                                                && cic.ClubId == clubId
                                                && c.Status == status
                                                && c.NumberOfTeam == 0 // điều kiện event
                                                select c).ToListAsync();

            }
            else
            {
                //competition
                listCompetitionOrEvent = await (from cic in context.CompetitionInClubs
                                                join c in context.Competitions on cic.CompetitionId equals c.Id
                                                where cic.IsOwner == true
                                                && cic.ClubId == clubId
                                                && c.Status == status
                                                && c.NumberOfTeam != 0 // điều kiện competition
                                                select c).ToListAsync();
            }

            return listCompetitionOrEvent.Count;
        }


        //có thêm club id để sàng lọc ra cuộc thi được tổ chức bởi CLB -> lấy được task của CLB
        public async Task<PagingResult<ViewCompetition>> GetCompOrEveStudentIsAssignedTask(PagingRequest request, int clubId, string searchName, bool? isEvent, int userId)
        {
            var query = from m in context.Members
                        where m.UserId == userId
                        from mta in context.MemberTakesActivities
                        where mta.MemberId == m.Id
                        from ca in context.CompetitionActivities
                        where ca.Id == mta.CompetitionActivityId
                              && ca.Status != CompetitionActivityStatus.Cancelling
                        from c in context.Competitions
                        where ca.CompetitionId == c.Id && c.Status != CompetitionStatus.Draft
                              && c.Status != CompetitionStatus.Cancel
                              && c.Status != CompetitionStatus.PendingReview
                              //&& c.Status != CompetitionStatus.Approve
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == clubId && c.Id == cic.CompetitionId //&& c.CreateTime < new LocalTime().GetLocalTime().DateTime
                        //orderby c.CreateTime descending
                        select c;

            //search name
            if (!string.IsNullOrEmpty(searchName)) query = query.Where(c => c.Name.ToLower().Contains(searchName.ToLower()));

            //isEvent
            if (isEvent.HasValue)
            {
                if (isEvent.Value == false)
                {
                    query = query.Where(c => c.NumberOfTeam != 0);
                }
                if (isEvent.Value)
                {
                    query = query.Where(c => c.NumberOfTeam == 0);
                }
            }

            List<Competition> listCompetitionStudentIsAssignedTask = await query.Distinct().ToListAsync();

            listCompetitionStudentIsAssignedTask = listCompetitionStudentIsAssignedTask.OrderByDescending(c => c.CreateTime).ToList();


            int totalCount = listCompetitionStudentIsAssignedTask.Count();

            //
            listCompetitionStudentIsAssignedTask = listCompetitionStudentIsAssignedTask.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            foreach (Competition compe in listCompetitionStudentIsAssignedTask)
            {

                //lấy major ID
                List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

                var queryListCompetitionInMajor = compe.CompetitionInMajors;
                List<CompetitionInMajor> listCompetitionInMajor = queryListCompetitionInMajor.ToList();

                //lấy Club Owner
                List<CompetitionInClub> clubList = await (from cic in context.CompetitionInClubs
                                                          where compe.Id == cic.CompetitionId
                                                          select cic).ToListAsync();

                List<ViewClubInComp> listVcip = new List<ViewClubInComp>();

                if (clubList.Count > 0)
                {
                    foreach (var competitionInClub in clubList)
                    {
                        Club club = await (from c in context.Clubs
                                           where c.Id == competitionInClub.ClubId
                                           select c).FirstOrDefaultAsync();

                        ViewClubInComp vcip = new ViewClubInComp()
                        {
                            Id = competitionInClub.Id,
                            ClubId = club.Id,
                            Name = club.Name,
                            Image = club.Image,
                            Fanpage = club.ClubFanpage,
                            IsOwner = competitionInClub.IsOwner
                        };

                        listVcip.Add(vcip);
                    }
                }

                //lấy competition type name
                CompetitionType competitionType = await (from c in context.Competitions
                                                         where c.Id == compe.Id
                                                         from ct in context.CompetitionTypes
                                                         where ct.Id == c.CompetitionTypeId
                                                         select ct).FirstOrDefaultAsync();

                string competitionTypeName = competitionType.TypeName;

                foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
                {
                    Major major = await (from m in context.Majors
                                         where m.Id == competitionInMajor.MajorId
                                         select m).FirstOrDefaultAsync();
                    if (major != null)
                    {
                        ViewMajorInComp vdic = new ViewMajorInComp()
                        {
                            Id = competitionInMajor.Id,//fix
                            MajorId = competitionInMajor.MajorId,
                            Name = major.Name,
                        };
                        listViewMajorInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewCompetition vc = new ViewCompetition()
                {
                    Id = compe.Id,
                    Name = compe.Name,
                    CompetitionTypeId = compe.CompetitionTypeId,
                    CompetitionTypeName = competitionTypeName,
                    Scope = compe.Scope,
                    Status = compe.Status,
                    View = compe.View,
                    CreateTime = compe.CreateTime,
                    StartTime = compe.StartTime,
                    IsSponsor = compe.IsSponsor,
                    MajorInCompetition = listViewMajorInComp,
                    ClubInCompetition = listVcip,
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false
                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<bool> CheckClubBelongToUniversity(int clubId, int universityId)
        {
            Club query = await (from c in context.Clubs
                                where c.UniversityId == universityId && c.Id == clubId
                                select c).FirstOrDefaultAsync();

            return (query != null) ? true : false;
        }



        // Nhat
        public async Task<CompetitionScopeStatus> GetScopeCompetition(int id)
        {
            var query = from c in context.Competitions
                        where c.Id.Equals(id)
                        select c.Scope;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExisteUniInCompetition(int universityId, int competitionId)
        {
            return await (from cic in context.CompetitionInClubs
                          join c in context.Clubs on cic.ClubId equals c.Id
                          where cic.CompetitionId.Equals(competitionId) && c.UniversityId.Equals(universityId)
                          select c.UniversityId).FirstOrDefaultAsync() > 0;
        }

        public async Task<bool> CheckExistedCompetition(int competitionId)
        {
            return await context.Competitions.FirstOrDefaultAsync(competition => competition.Id.Equals(competitionId)) != null;
        }

        public async Task<bool> CheckExistedCompetitionByStatus(int competitionId, CompetitionStatus status)
        {
            return await context.Competitions.FirstOrDefaultAsync(competition => competition.Id.Equals(competitionId)
                                                                                    && competition.Status.Equals(status)) != null;
        }
    }
}
