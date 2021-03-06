using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.Club;
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public class ClubRepo : Repository<Club>, IClubRepo
    {
        public ClubRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<ViewClub> GetById(int id, bool? status)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.Id.Equals(id)
                        select new { c, u };

            if (status.HasValue) query = query.Where(selector => selector.c.Status.Equals(status.Value));

            ViewClub club = await query.Select(x => new ViewClub()
            {
                Id = x.c.Id,
                Description = x.c.Description,
                Founding = x.c.Founding,
                Name = x.c.Name,
                TotalMember = x.c.TotalMember,
                UniversityId = x.c.UniversityId,
                UniversityName = x.u.Name,
                Image = x.c.Image,
                ClubContact = x.c.ClubContact,
                ClubFanpage = x.c.ClubFanpage,
                Status = x.c.Status,
            }).FirstOrDefaultAsync();

            return (query.Any()) ? club : null;
        }

        public async Task<PagingResult<ViewClub>> GetByCompetition(int competitionId, PagingRequest request)
        {
            var query = from cil in context.CompetitionInClubs
                        join c in context.Clubs on cil.ClubId equals c.Id
                        join u in context.Universities on c.UniversityId equals u.Id
                        where cil.CompetitionId == competitionId
                        select new { c, u };
            int totalCount = query.Count();

            List<ViewClub> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                              .Select(x => new ViewClub()
                                              {
                                                  Id = x.c.Id,
                                                  Description = x.c.Description,
                                                  Founding = x.c.Founding,
                                                  Name = x.c.Name,
                                                  TotalMember = x.c.TotalMember,
                                                  UniversityId = x.c.UniversityId,
                                                  UniversityName = x.u.Name,
                                                  Image = x.c.Image,
                                                  ClubContact = x.c.ClubContact,
                                                  ClubFanpage = x.c.ClubFanpage,
                                                  Status = x.c.Status,
                                              }).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewClub>> GetByConditions(ClubRequestModel request)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.UniversityId.Equals(request.UniversityId)
                        select new { c, u };

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.c.Name.Contains(request.Name));

            // student and sponsor role
            if (request.Status.HasValue) query = query.Where(x => x.c.Status.Equals(request.Status.Value));

            int totalCount = query.Count();

            List<ViewClub> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x =>
                new ViewClub()
                {
                    Id = x.c.Id,
                    Description = x.c.Description,
                    Founding = x.c.Founding,
                    Name = x.c.Name,
                    TotalMember = x.c.TotalMember,
                    UniversityId = x.c.UniversityId,
                    UniversityName = x.u.Name,
                    Image = x.c.Image,
                    ClubContact = x.c.ClubContact,
                    ClubFanpage = x.c.ClubFanpage,
                    Status = x.c.Status,
                }
            ).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewClub>> GetByManager(ClubRequestByManagerModel request)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.Status.Equals(true)
                        select new { c, u };

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.c.Name.Contains(request.Name));

            if (request.UniversityIds != null) query = query.Where(selector => request.UniversityIds.Contains(selector.c.UniversityId));

            int totalCount = query.Count();

            List<ViewClub> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x =>
                new ViewClub()
                {
                    Id = x.c.Id,
                    Description = x.c.Description,
                    Founding = x.c.Founding,
                    Name = x.c.Name,
                    TotalMember = x.c.TotalMember,
                    UniversityId = x.c.UniversityId,
                    UniversityName = x.u.Name,
                    Image = x.c.Image,
                    ClubContact = x.c.ClubContact,
                    ClubFanpage = x.c.ClubFanpage,
                    Status = x.c.Status,
                }
            ).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<List<ViewClub>> GetByUser(int userId)
        {
            var query = from m in context.Members
                        join c in context.Clubs on m.ClubId equals c.Id
                        join u in context.Universities on c.UniversityId equals u.Id
                        where m.UserId == userId && m.Status == MemberStatus.Active
                        select new { c, u };

            int totalCount = query.Count();
            List<ViewClub> clubs = await query.Select(x => new ViewClub()
            {
                Id = x.c.Id,
                Description = x.c.Description,
                Founding = x.c.Founding,
                Name = x.c.Name,
                TotalMember = x.c.TotalMember,
                UniversityId = x.c.UniversityId,
                UniversityName = x.u.Name,
                Image = x.c.Image,
                ClubContact = x.c.ClubContact,
                ClubFanpage = x.c.ClubFanpage,
                Status = x.c.Status,
            }).ToListAsync();

            return (clubs.Count > 0) ? clubs : null;
        }

        public async Task<PagingResult<ViewClub>> GetByUniversity(int universityId, PagingRequest request)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.UniversityId.Equals(universityId)
                        select new { c, u };

            int totalCount = query.Count();
            List<ViewClub> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                              .Select(x => new ViewClub()
                                              {
                                                  Id = x.c.Id,
                                                  Description = x.c.Description,
                                                  Founding = x.c.Founding,
                                                  Name = x.c.Name,
                                                  TotalMember = x.c.TotalMember,
                                                  UniversityId = x.c.UniversityId,
                                                  UniversityName = x.u.Name,
                                                  Image = x.c.Image,
                                                  ClubContact = x.c.ClubContact,
                                                  ClubFanpage = x.c.ClubFanpage,
                                                  Status = x.c.Status,
                                              }).ToListAsync();

            return (clubs.Count > 0) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<int> CheckExistedClubName(int universityId, string name)
        {
            Club club = await context.Clubs.FirstOrDefaultAsync(c => c.Name == name && c.UniversityId == universityId);
            return (club != null) ? club.Id : 0;
        }

        public async Task<List<int>> GetByCompetition(int competitionId)
        {
            List<int> clubIds = await (from cic in context.CompetitionInClubs
                                       where cic.CompetitionId.Equals(competitionId)
                                       select cic.ClubId).ToListAsync();

            return (clubIds.Count > 0) ? clubIds : null;
        }

        public async Task<int> GetUniversityByClub(int clubId)
        {
            return await (from c in context.Clubs
                          where c.Id.Equals(clubId)
                          select c.UniversityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExistedClubInUniversity(int universityId, int clubId)
        {
            var query = from c in context.Clubs
                        where c.Id.Equals(clubId) && c.UniversityId.Equals(universityId)
                        select c;

            return await query.AnyAsync();
        }
    }
}

