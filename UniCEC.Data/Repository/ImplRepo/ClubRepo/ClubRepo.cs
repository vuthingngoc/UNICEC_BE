using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public class ClubRepo : Repository<Club>, IClubRepo
    {
        public ClubRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<ViewClub> GetById(int id, int roleId)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.Id.Equals(id)
                        select new { c, u };

            // student and sponsor role
            if (roleId != 1 && roleId != 4) query = query.Where(x => x.c.Status.Equals(true));

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

            return (query.Count() > 0) ? club : null;
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
                                                  Status = x.c.Status,
                                                  Image = x.c.Image
                                              }).ToListAsync();

            return (clubs.Count > 0) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewClub>> GetByName(int universityId, int roleId, string name, PagingRequest request)
        {
            var query = from c in context.Clubs
                        join u in context.Universities on c.UniversityId equals u.Id
                        where c.Name.Contains(name) && c.UniversityId.Equals(universityId)
                        select new { c, u };

            // student and sponsor role
            if (roleId != 1 && roleId != 4) query = query.Where(x => x.c.Status.Equals(true));

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
                    Status = x.c.Status,
                    Image = x.c.Image
                }
            ).ToListAsync();

            return (clubs.Count > 0) ? new PagingResult<ViewClub>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
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
                Status = x.c.Status,
                Image = x.c.Image
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
                                                  Status = x.c.Status,
                                                  Image = x.c.Image
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
            return await (from cic in context.CompetitionInClubs
                         where cic.CompetitionId.Equals(competitionId)
                         select cic.ClubId).ToListAsync();
        }

        public async Task<int> GetUniversityByClub(int clubId)
        {
            return await(from c in context.Clubs
                         where c.Id.Equals(clubId)
                         select c.UniversityId).FirstOrDefaultAsync();
        }
    }
}

