using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public class ClubRepo : Repository<Club>, IClubRepo
    {
        public ClubRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<Club>> GetByCompetition(int competitionId, PagingRequest request)
        {
            var query = from cil in context.CompetitionInClubs
                        join c in context.Clubs on cil.ClubId equals c.Id
                        where cil.CompetitionId == competitionId
                        select new { c };
            int totalCount = query.Count();

            List<Club> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x =>
                new Club()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Description = x.c.Description,
                    Founding = x.c.Founding,
                    Status = x.c.Status,
                    TotalMember = x.c.TotalMember,
                    UniversityId = x.c.UniversityId
                }).ToListAsync();

            return (clubs.Count > 0) ? new PagingResult<Club>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<Club>> GetByName(string name, PagingRequest request)
        {
            var query = from c in context.Clubs
                        where c.Name.Contains(name)
                        select new { c };
            int totalCount = query.Count();

            List<Club> clubs = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x =>
                new Club()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    Description = x.c.Description,
                    Founding = x.c.Founding,
                    Status = x.c.Status,
                    TotalMember = x.c.TotalMember,
                    UniversityId = x.c.UniversityId
                }
            ).ToListAsync();

            return (clubs.Count > 0) ? new PagingResult<Club>(clubs, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<List<Club>> GetByUser(int userId)
        {
            var query = from m in context.Members
                        join cp in context.ClubPrevious on m.Id equals cp.MemberId
                        join c in context.Clubs on cp.ClubId equals c.Id
                        where m.StudentId == userId && cp.Status == ClubHistoryStatus.Active
                        select new { c };
            int totalCount = query.Count();
            List<Club> clubs = await query.Select(x => new Club()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Description = x.c.Description,
                Founding = x.c.Founding,
                Status = x.c.Status,
                TotalMember = x.c.TotalMember,
                UniversityId = x.c.UniversityId
            }).ToListAsync();

            return (clubs.Count > 0) ? clubs : null; 
        }

        public async Task<int> CheckExistedClubName(int universityId, string name)
        {
            Club club = await context.Clubs.FirstOrDefaultAsync(c => c.Name == name && c.UniversityId == universityId);
            return (club != null) ? club.Id : 0;
        }
    }
}
