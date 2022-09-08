using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.MatchRepo
{
    public class MatchRepo : Repository<Match>, IMatchRepo
    {
        public MatchRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<bool> CheckDuplicatedMatch(string title, int roundId)
        {
            var query = from m in context.Matches
                        where m.Title.ToLower().Equals(title.ToLower()) && m.RoundId.Equals(roundId)
                        select m;

            return await query.AnyAsync();
        }

        public async Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request)
        {
            var query = from m in context.Matches
                        join mt in context.MatchTypes on m.MatchTypeId equals mt.Id
                        join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                        join c in context.Competitions on cr.CompetitionId equals c.Id
                        select new { m, mt, cr, c };

            if (request.CompetitionId.HasValue) query = query.Where(selector => selector.c.Id.Equals(request.CompetitionId.Value));

            if (request.RoundId.HasValue) query = query.Where(selector => selector.cr.Id.Equals(request.RoundId.Value));

            if (request.MatchTypeId.HasValue) query = query.Where(selector => selector.m.MatchTypeId.Equals(request.MatchTypeId.Value));

            if (request.StartTime.HasValue) query = query.Where(selector => selector.m.StartTime.Year.Equals(request.StartTime.Value.Year)
                    && selector.m.StartTime.Month.Equals(request.StartTime.Value.Month) && selector.m.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.StartTime.HasValue && request.StartTime.Value.Hour > 0)
                query = query.Where(selector => selector.m.StartTime.Hour.Equals(request.StartTime.Value.Hour));

            if (request.EndTime.HasValue) query = query.Where(selector => selector.m.EndTime.Year.Equals(request.EndTime.Value.Year)
                    && selector.m.EndTime.Month.Equals(request.EndTime.Value.Month) && selector.m.EndTime.Day.Equals(request.EndTime.Value.Day));

            if (request.EndTime.HasValue && request.EndTime.Value.Hour > 0)
                query = query.Where(selector => selector.m.EndTime.Hour.Equals(request.EndTime.Value.Hour));

            if (request.Status.HasValue) query = query.Where(selector => selector.m.Status.Equals(request.Status.Value));

            var matches = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                .Select(selector => new ViewMatch()
                                {
                                    Id = selector.m.Id,
                                    Address = selector.m.Address,
                                    CreateTime = selector.m.CreateTime,
                                    Description = selector.m.Description,
                                    EndTime = selector.m.EndTime,
                                    MatchTypeId = selector.m.MatchTypeId,
                                    MatchTypeName = selector.mt.Name,
                                    NumberOfTeam = selector.m.NumberOfTeam,
                                    RoundId = selector.m.RoundId,
                                    RoundName = selector.cr.Title,
                                    StartTime = selector.m.StartTime,
                                    Title = selector.m.Title,
                                    Status = selector.m.Status
                                }).ToListAsync();

            int totalCount = query.Count();

            return (matches.Count > 0)
                    ? new PagingResult<ViewMatch>(matches, totalCount, request.CurrentPage, request.PageSize)
                    : null;
        }

        public async Task<ViewMatch> GetById(int id)
        {
            var query = from m in context.Matches
                        join mt in context.MatchTypes on m.MatchTypeId equals mt.Id
                        join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                        where m.Id.Equals(id)
                        select new { m, mt, cr };

            if (!query.Any()) return null;

            return await query.Select(selector => new ViewMatch()
            {
                Id = id,
                Address = selector.m.Address,
                CreateTime = selector.m.CreateTime,
                Description = selector.m.Description,
                EndTime = selector.m.EndTime,
                MatchTypeId = selector.m.MatchTypeId,
                MatchTypeName = selector.mt.Name,
                NumberOfTeam = selector.m.NumberOfTeam,
                RoundId = selector.m.RoundId,
                RoundName = selector.cr.Title,
                StartTime = selector.m.StartTime,
                Title = selector.m.Title,
                Status = selector.m.Status
            }).FirstOrDefaultAsync();
        }
    }
}
