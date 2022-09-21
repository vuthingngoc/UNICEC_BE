using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;
using UniCEC.Data.RequestModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.Enum;

namespace UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo
{
    public class TeamInMatchRepo : Repository<TeamInMatch>, ITeamInMatchRepo
    {
        public TeamInMatchRepo(UniCECContext context) : base(context)
        {
        }

        public bool CheckDuplicatedTeamInMatch(int matchId, int teamId, int? teamInMatchId)
        {
            var query = from tim in context.TeamInMatches
                        where tim.MatchId.Equals(matchId) && tim.TeamId.Equals(teamId)
                        select tim;

            if (teamInMatchId.HasValue) query = query.Where(tim => !tim.Id.Equals(teamInMatchId));

            return query.Any();
        }

        public async Task<bool> CheckIsLoseMatch(int matchId)
        {
            return await (from m in context.Matches
                          where m.Id.Equals(matchId) && m.IsLoseMatch.Equals(true)
                          select m.IsLoseMatch).FirstOrDefaultAsync() != null;
        }

        public async Task Delete(TeamInMatch teamInMatch)
        {
            context.TeamInMatches.Remove(teamInMatch);
            await Update();
        }

        public async Task<PagingResult<ViewTeamInMatch>> GetByConditions(TeamInMatchRequestModel request)
        {
            var query = from tim in context.TeamInMatches
                        join m in context.Matches on tim.MatchId equals m.Id
                        join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                        join t in context.Teams on tim.TeamId equals t.Id
                        select new { tim, m, cr, t };

            if (request.Status.HasValue) query = query.Where(selector => selector.tim.Status.Equals(request.Status.Value));

            if (request.Scores.HasValue) query = query.Where(selector => selector.tim.Scores.Equals(request.Scores.Value));

            if (request.TeamId.HasValue) query = query.Where(selector => selector.tim.TeamId.Equals(request.TeamId.Value));

            if (request.MatchId.HasValue) query = query.Where(selector => selector.tim.MatchId.Equals(request.MatchId.Value));

            if (request.RoundId.HasValue) query = query.Where(selector => selector.m.RoundId.Equals(request.RoundId.Value));

            int totalCount = query.Count();

            List<ViewTeamInMatch> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                .Select(selector => new ViewTeamInMatch()
                                                {
                                                    Id = selector.tim.Id,
                                                    MatchId = selector.tim.MatchId,
                                                    MatchTitle = selector.m.Title,
                                                    RoundId = selector.cr.Id,
                                                    RoundName = selector.cr.Title,
                                                    Scores = selector.tim.Scores,
                                                    TeamId = selector.tim.TeamId,
                                                    TeamName = selector.t.Name,
                                                    Status = selector.tim.Status,
                                                    Description = selector.tim.Description,
                                                }).ToListAsync();

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    int numberOfMembers = (from pit in context.ParticipantInTeams
                                           where pit.TeamId.Equals(item.TeamId) && pit.Status.Equals(ParticipantInTeamStatus.InTeam)
                                           select pit).Count();
                    item.NumberOfMembers = numberOfMembers;
                }

                return new PagingResult<ViewTeamInMatch>(items, totalCount, request.CurrentPage, request.PageSize);
            }

            return null;
        }

        public async Task<ViewTeamInMatch> GetById(int id)
        {
            ViewTeamInMatch team = await (from tim in context.TeamInMatches
                                          join m in context.Matches on tim.MatchId equals m.Id
                                          join cr in context.CompetitionRounds on m.RoundId equals cr.Id
                                          join t in context.Teams on tim.TeamId equals t.Id
                                          where tim.Id.Equals(id)
                                          select new ViewTeamInMatch()
                                          {
                                              Id = tim.Id,
                                              MatchId = tim.MatchId,
                                              MatchTitle = m.Title,
                                              RoundId = cr.Id,
                                              RoundName = cr.Title,
                                              Scores = tim.Scores,
                                              TeamId = tim.TeamId,
                                              TeamName = t.Name,
                                              Status = tim.Status,
                                              Description = tim.Description,
                                          }).FirstOrDefaultAsync();

            if (team != null)
            {
                int numberOfMembers = (from pit in context.ParticipantInTeams
                                       where pit.TeamId.Equals(team.TeamId) && pit.Status.Equals(ParticipantInTeamStatus.InTeam)
                                       select pit).Count();
                team.NumberOfMembers = numberOfMembers;
            }

            return team;
        }

        public async Task<int> GetRoundIdByMatch(int matchId)
        {
            return await (from tim in context.TeamInMatches
                          join m in context.Matches on tim.MatchId equals m.Id
                          where tim.MatchId.Equals(matchId)
                          select m.RoundId).FirstOrDefaultAsync();
        }

        public async Task InsertMultiTeams(List<TeamInMatch> teams)
        {
            await context.TeamInMatches.AddRangeAsync(teams);
            await context.SaveChangesAsync();
        }

        public void UpdateStatusTeams(int matchId, TeamInMatchStatus status) // when cancel match 
        {
            (from tim in context.TeamInMatches
             where tim.MatchId.Equals(matchId)
             select tim).ToList().ForEach(team => team.Status = status);
        }
    }
}
