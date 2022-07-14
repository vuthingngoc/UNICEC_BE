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
        public async Task<PagingResult<ViewCompetition>> GetCompOrEve(CompetitionRequestModel request)
        {
            //
            IQueryable<Competition> query;
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();

            if (request.ClubId.HasValue)
            {
                query = from cic in context.CompetitionInClubs
                        where cic.ClubId == request.ClubId
                        from comp in context.Competitions
                        where cic.CompetitionId == comp.Id
                        select comp;
            }
            //Không có club chỉ status publish
            // cái này đang dư cần chỉnh lại
            else
            {
                query = from comp in context.Competitions
                        where comp.StartTime >= localTime.DateTime && comp.Status != CompetitionStatus.Cancel && comp.Status == CompetitionStatus.Publish
                        orderby comp.StartTime
                        select comp;
            }

            //có club thì mới có nhiều status
            if (request.ClubId.HasValue)
            {
                //status     
                if (request.Statuses.Count > 0) query = query.Where(comp => request.Statuses.Count == 0 || request.Statuses.Contains((CompetitionStatus)comp.Status));
            }

            //Public
            if (request.Scope.HasValue) query = query.Where(comp => comp.Scope == request.Scope);
            //Serach Event
            if (request.Event.HasValue)
            {
                if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfTeam == 0);
            }

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(comp => comp.Name.Contains(request.Name));

            int totalCount = query.Count();
            //
            List<ViewCompetition> competitions = new List<ViewCompetition>();

            List<Competition> listCompetition = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            foreach (Competition compe in listCompetition)
            {
                //lấy department ID
                List<ViewMajorInComp> listViewMajorInComp = new List<ViewMajorInComp>();

                var query_List_CompetitionInDepartment = compe.CompetitionInMajors;
                List<CompetitionInMajor> listCompetitionInMajor = query_List_CompetitionInDepartment.ToList();

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
                            Image = club.Image,
                            Fanpage = club.ClubFanpage,
                            IsOwner = competitionInClub.IsOwner
                        };

                        List_vcip.Add(vcip);
                    }
                }

                foreach (CompetitionInMajor competitionInMajor in listCompetitionInMajor)
                {
                    Major major = await (from m in context.Majors
                                         where m.Id == competitionInMajor.MajorId
                                         select m).FirstOrDefaultAsync();
                    if (major != null)
                    {
                        ViewMajorInComp vdic = new ViewMajorInComp()
                        {
                            Id = major.Id,
                            Name = major.Name,
                        };
                        listViewMajorInComp.Add(vdic);
                    }
                }

                //cb tạo View
                ViewCompetition vc = new ViewCompetition()
                {
                    Id = compe.Id,
                    CompetitionTypeName = compe.CompetitionType.TypeName,
                    Name = compe.Name,
                    CompetitionTypeId = compe.CompetitionTypeId,
                    Scope = compe.Scope,
                    Status = compe.Status,
                    View = compe.View,
                    CreateTime = compe.CreateTime,
                    StartTime = compe.StartTime,
                    IsSponsor = compe.IsSponsor,
                    MajorInCompetition = listViewMajorInComp,
                    ClubInCompetition = List_vcip,

                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //Get top EVENT or COMPETITION by Status
        //gần ngày hiện tại
        //Thuộc Club
        //lấy theo publish
        public async Task<List<ViewCompetition>> GetTopCompOrEve(int ClubId, bool? Event/*, CompetitionStatus? Status*/, CompetitionScopeStatus? Scope, int Top)
        {
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();
            //
            IQueryable<Competition> query;


            //if (ClubId.HasValue)
            //{
            query = from cic in context.CompetitionInClubs
                    where cic.ClubId == ClubId
                    join comp in context.Competitions on cic.CompetitionId equals comp.Id
                    where comp.StartTime >= localTime.DateTime && comp.Status != CompetitionStatus.Cancel && comp.Status == CompetitionStatus.Publish
                    orderby comp.StartTime
                    select comp;
            //}
            //else
            //{
            //    query = from comp in context.Competitions
            //            where comp.StartTime >= localTime.DateTime
            //            orderby comp.StartTime
            //            select comp;
            //}

            //Serach Event
            if (Event.HasValue)
            {
                if (Event.Value == true) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.NumberOfTeam == 0);
            }
            //Scope
            if (Scope.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Scope == Scope);
            //Status
            //if (Status.HasValue) query = (IOrderedQueryable<Competition>)query.Where(comp => comp.Status == Status);

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            List<Competition> list_Competition = await query.Take(Top).ToListAsync();

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
                            Id = club.Id,
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
                            Id = major.Id,
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

                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count > 0) ? competitions : null;
        }



        public async Task<PagingResult<ViewCompetition>> GetCompOrEveByAdminUni(AdminUniGetCompetitionRequestModel request, int universityId)
        {
            List<Competition> list_Competition = await (from c in context.Competitions
                                                        where c.UniversityId == universityId && c.Status == CompetitionStatus.PendingReview
                                                        select c).ToListAsync();
            //Name
            if (!string.IsNullOrEmpty(request.Name)) list_Competition = list_Competition.Where(comp => comp.Name.Contains(request.Name)).ToList();

            int totalCount = list_Competition.Count();

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            list_Competition =  list_Competition.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

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
                            Id = club.Id,
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
                            Id = major.Id,
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

                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<Competition> GetCompetitionBySeedsCode(string seedsCode)
        {
            return await (from c in context.Competitions
                          where c.SeedsCode == seedsCode
                          select c).FirstOrDefaultAsync();
        }

        public async Task<PagingResult<ViewCompetition>> GetCompOrEveUnAuthorize(CompetitionUnAuthorizeRequestModel request, List<CompetitionStatus> listCompetitionStatus)
        {
            List<Competition> listCompetition = new List<Competition>();

            //lấy cả 2 
            if (request.MostView.HasValue && request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                          where c.Status != CompetitionStatus.Cancel
                                             && c.Scope == CompetitionScopeStatus.InterUniversity
                                             //&& c.StartTimeRegister >= new LocalTime().GetLocalTime().DateTime
                                          orderby c.View descending
                                          select c).ToListAsync();

                listCompetition = listCompetition.OrderByDescending(c => c.CreateTime).ToList();
            }

            //lấy thời gian đăng ký gần hiện tại
            if (!request.MostView.HasValue && request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                          where c.Status != CompetitionStatus.Cancel
                                             && c.Scope == CompetitionScopeStatus.InterUniversity
                                             //&& c.StartTimeRegister >= new LocalTime().GetLocalTime().DateTime
                                          orderby c.CreateTime descending
                                          select c).ToListAsync();
            }

            //lấy số lượng view nhiều 
            if (request.MostView.HasValue && !request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                          where c.Status != CompetitionStatus.Cancel
                                             && c.Scope == CompetitionScopeStatus.InterUniversity
                                          orderby c.View descending
                                          select c).ToListAsync();
            }

            //Không sort theo cái nào cả
            if (!request.MostView.HasValue && !request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                          where c.Status != CompetitionStatus.Cancel && c.Scope == CompetitionScopeStatus.InterUniversity
                                          select c).ToListAsync();
            }

            //
            if (listCompetitionStatus.Count > 0) listCompetition = listCompetition.Where(comp => listCompetitionStatus.Contains((CompetitionStatus)comp.Status)).ToList();

            //
            if (request.Sponsor.HasValue) listCompetition = listCompetition.Where(comp => comp.IsSponsor == true).ToList();

            //
            if (!string.IsNullOrEmpty(request.Name)) listCompetition = listCompetition.Where(comp => comp.Name.Contains(request.Name)).ToList();

            int totalCount = listCompetition.Count();

            //
            listCompetition = listCompetition.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            foreach (Competition compe in listCompetition)
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
                            Id = club.Id,
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
                            Id = major.Id,
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

                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;


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


    }
}
