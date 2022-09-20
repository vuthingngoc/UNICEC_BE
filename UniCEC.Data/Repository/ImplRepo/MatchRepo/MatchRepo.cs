using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.MatchRepo
{
    public class MatchRepo : Repository<Match>, IMatchRepo
    {
        public MatchRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<bool> CheckAvailableMatchId(int matchId)
        {
            return await context.Matches.FirstOrDefaultAsync(match => match.Id.Equals(matchId) 
                                                                        && !match.Status.Equals(MatchStatus.IsDeleted)) != null;
        }

        public async Task<bool> CheckDuplicatedMatch(string title, int roundId)
        {
            var query = from m in context.Matches
                        where m.Title.ToLower().Equals(title.ToLower()) && m.RoundId.Equals(roundId) 
                                && !m.Status.Equals(MatchStatus.IsDeleted)
                        select m;

            return await query.AnyAsync();
        }

        public async Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request)
        {
            var query = from m in context.Matches
                        join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                        join crt in context.CompetitionRoundTypes on cr.CompetitionRoundTypeId equals crt.Id
                        join c in context.Competitions on cr.CompetitionId equals c.Id
                        select new { m, cr, crt, c };

            if (request.CompetitionId.HasValue) query = query.Where(selector => selector.c.Id.Equals(request.CompetitionId.Value));

            if (request.RoundId.HasValue) query = query.Where(selector => selector.cr.Id.Equals(request.RoundId.Value));

            if (request.IsLoseMatch.HasValue) query = query.Where(selector => selector.m.IsLoseMatch.Equals(request.IsLoseMatch.Value));

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
                                    IsLoseMatch = selector.m.IsLoseMatch,
                                    NumberOfTeam = selector.m.NumberOfTeam,
                                    RoundId = selector.m.RoundId,
                                    RoundName = selector.cr.Title,
                                    RoundTypeId = selector.crt.Id,
                                    RoundTypeName = selector.crt.Name,
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
                        join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                        join crt in context.CompetitionRoundTypes on cr.CompetitionRoundTypeId equals crt.Id
                        where m.Id.Equals(id)
                        select new { m, cr, crt };

            if (!query.Any()) return null;

            return await query.Select(selector => new ViewMatch()
            {
                Id = id,
                Address = selector.m.Address,
                CreateTime = selector.m.CreateTime,
                Description = selector.m.Description,
                EndTime = selector.m.EndTime,
                IsLoseMatch = selector.m.IsLoseMatch,
                NumberOfTeam = selector.m.NumberOfTeam,
                RoundId = selector.m.RoundId,
                RoundName = selector.cr.Title,
                RoundTypeId = selector.crt.Id,
                RoundTypeName = selector.crt.Name,
                StartTime = selector.m.StartTime,
                Title = selector.m.Title,
                Status = selector.m.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<int> GetCompetitionIdByMatch(int matchId)
        {
            return await (from m in context.Matches
                         join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                         where m.Id.Equals(matchId)
                         select cr.CompetitionId).FirstOrDefaultAsync();
        }

        public async Task UpdateStatusMatchesByComp(int competitionId, MatchStatus status)
        {
            (from m in context.Matches
             join cr in context.CompetitionRounds on m.RoundId equals cr.Id
             where cr.CompetitionId.Equals(competitionId) && !m.Status.Equals(MatchStatus.IsDeleted)
             select m).ToList().ForEach(m => m.Status = status);

            await Update();
        }

        public async Task UpdateStatusMatchesByRound(int roundId, MatchStatus status)
        {
            (from m in context.Matches
             where m.RoundId.Equals(roundId) && !m.Status.Equals(MatchStatus.IsDeleted)
             select m).ToList().ForEach(m => m.Status = status);

            await Update();
        }
    }
}
