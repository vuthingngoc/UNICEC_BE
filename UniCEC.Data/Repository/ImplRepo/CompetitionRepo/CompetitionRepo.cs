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
            IQueryable<Competition> query;
            //LocalTime
            DateTimeOffset localTime = new LocalTime().GetLocalTime();


            //search có clubId 
            if (request.ClubId.HasValue) // có club là lấy trường mình
            {
                query = from c in context.Clubs
                        where c.Id == request.ClubId
                        from cic in context.CompetitionInClubs
                        where cic.ClubId == c.Id
                        from comp in context.Competitions
                        where cic.CompetitionId == comp.Id && comp.UniversityId == ((request.UniversityId.HasValue) ? request.UniversityId : universityId)
                        orderby comp.CreateTime descending
                        select comp;
            }
            //search kh club thì sẽ ra competition trong trường mình hoặc trường khác 
            else
            {
                query = from comp in context.Competitions
                        where comp.UniversityId == ((request.UniversityId.HasValue) ? request.UniversityId : universityId) // nếu truyền vào id Uni thì ra còn kh thì thôi lấy trường mình
                        orderby comp.CreateTime descending
                        select comp;
            }

            //status     
            if (request.Statuses != null) query = query.Where(comp => request.Statuses.Contains((CompetitionStatus)comp.Status));

            //Scope
            if (request.Scope.HasValue) query = query.Where(comp => comp.Scope == request.Scope);

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(comp => comp.Name.Contains(request.Name));

            //Serach Event
            if (request.Event.HasValue)
            {
                if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfTeam == 0);

                if (request.Event.Value == false) query = query.Where(comp => comp.NumberOfTeam != 0);
            }

            //View Most
            if (request.ViewMost.HasValue) query = query.OrderByDescending(comp => comp.View);


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
                            Id = competitionInClub.Id,
                            ClubId = club.Id,
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
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false
                };
                competitions.Add(vc);
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
                            Id = major.Id,
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
            List<Competition> list_Competition = await (from c in context.Competitions
                                                        where c.UniversityId == universityId && c.Status == CompetitionStatus.PendingReview
                                                        select c).ToListAsync();
            //Name
            if (!string.IsNullOrEmpty(request.Name)) list_Competition = list_Competition.Where(comp => comp.Name.Contains(request.Name)).ToList();

            int totalCount = list_Competition.Count();

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            list_Competition = list_Competition.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

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
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false
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
            //có vấn đề 
            if (request.MostView.HasValue && request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                         where c.Scope == CompetitionScopeStatus.InterUniversity
                                         orderby c.View descending, c.CreateTime descending
                                         select c).ToListAsync();
            }

            //lấy thời gian đăng ký gần hiện tại
            if (!request.MostView.HasValue && request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                         where c.Scope == CompetitionScopeStatus.InterUniversity
                                         orderby c.CreateTime descending
                                         select c).ToListAsync();
            }

            //lấy số lượng view nhiều 
            if (request.MostView.HasValue && !request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                         where c.Scope == CompetitionScopeStatus.InterUniversity
                                         orderby c.View descending
                                         select c).ToListAsync();
            }

            //Không sort theo cái nào cả
            if (!request.MostView.HasValue && !request.NearlyDate.HasValue)
            {
                listCompetition = await (from c in context.Competitions
                                         where c.Scope == CompetitionScopeStatus.InterUniversity
                                         select c).ToListAsync();
            }

            //
            if (listCompetitionStatus.Count > 0) listCompetition = listCompetition.Where(comp => listCompetitionStatus.Contains((CompetitionStatus)comp.Status)).ToList();

            //
            if (request.Sponsor.HasValue) listCompetition = listCompetition.Where(comp => comp.IsSponsor == request.Sponsor.Value).ToList();

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
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false
                };
                competitions.Add(vc);
            }//end each competition

            return (competitions.Count != 0) ? new PagingResult<ViewCompetition>(competitions, totalCount, request.CurrentPage, request.PageSize) : null;

        }

        public async Task<PagingResult<ViewCompetition>> GetCompsOrEvesStudentJoin(GetStudentJoinCompOrEve request, int userId)
        {
            var query = from p in context.Participants
                        where p.StudentId == userId
                        from c in context.Competitions
                        where c.Id == p.CompetitionId
                        select c;
            //Scope
            if (request.Scope.HasValue) query = query.Where(comp => comp.Scope == request.Scope.Value);

            //Name
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(comp => comp.Name.Contains(request.Name));

            //Event
            if (request.Event.HasValue)
            {
                if (request.Event.Value == true) query = query.Where(comp => comp.NumberOfTeam == 0);
                if (request.Event.Value == false) query = query.Where(comp => comp.NumberOfTeam != 0);
            }


            List<Competition> listCompetitionStudentJoin = await query.ToListAsync();

            int totalCount = listCompetitionStudentJoin.Count();

            //
            listCompetitionStudentJoin = listCompetitionStudentJoin.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).ToList();

            List<ViewCompetition> competitions = new List<ViewCompetition>();

            foreach (Competition compe in listCompetitionStudentJoin)
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
                    UniversityId = compe.UniversityId,
                    IsEvent = (compe.NumberOfTeam == 0) ? true : false

                };
                competitions.Add(vc);
            }//end each competition

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
                            Id = major.Id,
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
        public async Task<PagingResult<ViewCompetition>> GetCompOrEveStudentIsAssignedTask(PagingRequest request, int clubId, int userId)
        {
            List<Competition> listCompetitionStudentIsAssignedTask = await (from m in context.Members
                                                                            where m.UserId == userId
                                                                            from mta in context.MemberTakesActivities
                                                                            where mta.MemberId == m.Id
                                                                            from ca in context.CompetitionActivities
                                                                            where ca.Id == mta.CompetitionActivityId
                                                                                  && ca.Status != CompetitionActivityStatus.Cancelling
                                                                            from c in context.Competitions
                                                                            where ca.CompetitionId == c.Id
                                                                            from cic in context.CompetitionInClubs
                                                                            where cic.ClubId == clubId && c.Id == cic.CompetitionId
                                                                            select c).Distinct().ToListAsync();
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
            if(query == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
