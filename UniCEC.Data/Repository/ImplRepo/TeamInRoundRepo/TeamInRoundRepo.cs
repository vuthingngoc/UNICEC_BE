using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Participant;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo
{
    public class TeamInRoundRepo : Repository<TeamInRound>, ITeamInRoundRepo
    {
        public TeamInRoundRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<PagingResult<ViewTeamInRound>> GetByConditions(TeamInRoundRequestModel request)
        {
            var query = from tir in context.TeamInRounds
                        join t in context.Teams on tir.TeamId equals t.Id
                        where tir.RoundId.Equals(request.RoundId)
                        orderby tir.Rank ascending
                        select new { tir, t };

            if (request.Top.HasValue) query = query.Where(selector => selector.tir.Rank <= request.Top.Value);

            if (request.TeamId.HasValue) query = query.Where(selector => selector.tir.TeamId.Equals(request.TeamId.Value));

            if (request.Scores.HasValue) query = query.Where(selector => selector.tir.Scores.Equals(request.Scores.Value));

            if (request.Status.HasValue) query = query.Where(selector => selector.tir.Status.Equals(request.Status.Value));

            if (request.Rank.HasValue) query = query.Where(selector => selector.tir.Rank.Equals(request.Rank.Value));

            List<ViewTeamInRound> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(selector => new ViewTeamInRound()
                {
                    Id = selector.tir.Id,
                    TeamId = selector.tir.TeamId,
                    TeamName = selector.t.Name,
                    RoundId = selector.tir.RoundId,
                    Rank = selector.tir.Rank,
                    Scores = selector.tir.Scores,
                    Status = selector.tir.Status,
                }).ToListAsync();

            if (!query.Any()) return null;

            int totalCount = query.Count();

            foreach (var element in items)
            {
                element.MembersInTeam = await GetMembersInTeam(element.TeamId);
                element.NumberOfParticipatedMatches = GetNumberOfParticipatedMatches(element.TeamId, element.RoundId);
            }

            return new PagingResult<ViewTeamInRound>(items, totalCount, request.CurrentPage, request.PageSize);
        }

        public async Task<ViewTeamInRound> GetById(int id, bool? status)
        {
            var query = from tir in context.TeamInRounds
                        join t in context.Teams on tir.TeamId equals t.Id
                        where tir.Id.Equals(id)
                        select new { tir, t };

            if (status.HasValue) query = query.Where(selector => selector.tir.Status.Equals(status.Value));

            ViewTeamInRound teamInRound = await query.Select(selector => new ViewTeamInRound()
            {
                Id = selector.tir.Id,
                TeamId = selector.tir.TeamId,
                TeamName = selector.t.Name,
                RoundId = selector.tir.RoundId,
                Rank = selector.tir.Rank,
                Scores = selector.tir.Scores,
                Status = selector.tir.Status,
            }).FirstOrDefaultAsync();

            teamInRound.MembersInTeam = await GetMembersInTeam(teamInRound.TeamId);
            teamInRound.NumberOfParticipatedMatches = GetNumberOfParticipatedMatches(teamInRound.TeamId, teamInRound.RoundId);

            return teamInRound;
        }

        public async Task UpdateRankTeamsInRound(int roundId)
        {
            var query = from tir in context.TeamInRounds
                        where tir.RoundId.Equals(roundId) //&& tir.Status.Equals(true)
                        select tir;

            int rank = 1;
            List<TeamInRound> teams = query.OrderByDescending(team => team.Scores).ToList();
            for (int index = 0; index < teams.Count; index++)
            {
                if (index > 0 && !teams[index - 1].Scores.Equals(teams[index].Scores)) rank++;
                teams[index].Rank = rank;
            }

            await Update();
        }

        public async Task<List<ViewTeamInRound>> GetTopTeamsInCompetition(int competitionId, int top) // no use anymore
        {
            List<ViewTeamInRound> teamsInRound = await (from tir in context.TeamInRounds
                                                        join t in context.Teams on tir.TeamId equals t.Id
                                                        join cr in context.CompetitionRounds on tir.RoundId equals cr.Id
                                                        where cr.CompetitionId.Equals(competitionId) && tir.Rank <= top && tir.Status.Equals(true)
                                                        orderby (tir.Rank)
                                                        select new ViewTeamInRound()
                                                        {
                                                            Id = tir.Id,
                                                            RoundId = tir.RoundId,
                                                            Rank = tir.Rank,
                                                            Scores = tir.Scores,
                                                            TeamId = tir.TeamId,
                                                            TeamName = t.Name,
                                                            Status = tir.Status
                                                        }).Take(top).ToListAsync();

            foreach (var element in teamsInRound)
            {
                element.MembersInTeam = await GetMembersInTeam(element.TeamId);
            }

            return (teamsInRound.Count > 0) ? teamsInRound : null;
        }

        public async Task<List<ViewParticipantInTeam>> GetMembersInTeam(int teamId)
        {
            return await (from pit in context.ParticipantInTeams
                          join p in context.Participants on pit.ParticipantId equals p.Id
                          join u in context.Users on p.StudentId equals u.Id
                          where pit.TeamId.Equals(teamId)
                          select new ViewParticipantInTeam()
                          {
                              Id = pit.ParticipantId,
                              Name = u.Fullname,
                              Avatar = u.Avatar,
                              StudentCode = u.StudentCode
                          }).ToListAsync();
        }

        public async Task<List<ViewTeamInRound>> GetViewTeams(List<int> teamIds, int roundId)
        {
            var query = from tir in context.TeamInRounds
                        join t in context.Teams on tir.TeamId equals t.Id
                        where teamIds.Contains(tir.TeamId) && tir.RoundId.Equals(roundId)
                        select new { t, tir };

            List<ViewTeamInRound> items = await query.Select(selector => new ViewTeamInRound()
            {
                Id = selector.tir.Id,
                RoundId = selector.tir.RoundId,
                Rank = selector.tir.Rank,
                Scores = selector.tir.Scores,
                TeamId = selector.tir.TeamId,
                TeamName = selector.t.Name,
                Status = selector.tir.Status
            }).ToListAsync();

            foreach (var element in items)
            {
                element.MembersInTeam = await GetMembersInTeam(element.TeamId);
                element.NumberOfParticipatedMatches = GetNumberOfParticipatedMatches(element.TeamId, element.RoundId);
            }

            return items;
        }

        public async Task<int> GetTotalPointsTeam(int teamId, int competitionId)
        {
            return await (from tir in context.TeamInRounds
                          join cr in context.CompetitionRounds on tir.RoundId equals cr.Id
                          where tir.TeamId.Equals(teamId) && cr.CompetitionId.Equals(competitionId)
                          orderby tir.Id
                          select tir.Scores).LastOrDefaultAsync();
        }

        public async Task InsertMultiTeams(List<int> teamIds, int roundId)
        {
            if (teamIds.Count > 0)
            {
                List<TeamInRound> teams = new List<TeamInRound>();
                foreach (int teamId in teamIds)
                {
                    TeamInRound team = new TeamInRound()
                    {
                        RoundId = roundId,
                        Rank = 0,
                        Scores = 0,
                        TeamId = teamId,
                        Status = true // default status when insert
                    };

                    teams.Add(team);
                }

                await context.TeamInRounds.AddRangeAsync(teams);
                await context.SaveChangesAsync();
            }
        }

        public async Task InsertMultiTeams(List<TeamInRound> teams)
        {
            if (teams.Count > 0)
            {
                await context.TeamInRounds.AddRangeAsync(teams);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetTeamIdsByRound(int roundId, bool? status)
        {
            var query = from tir in context.TeamInRounds
                        where tir.RoundId.Equals(roundId)
                        select tir;

            if (status.HasValue) query = query.Where(tir => tir.Status.Equals(status.Value));

            return (query.Any()) ? await query.Select(tir => tir.TeamId).ToListAsync() : null;
        }

        public async Task<List<ViewTeamInRound>> GetTeamsByRound(int roundId, int? top) // for competition manager
        {
            List<ViewTeamInRound> teams = await (from tir in context.TeamInRounds
                                                 join t in context.Teams on tir.TeamId equals t.Id
                                                 where tir.RoundId.Equals(roundId)
                                                 orderby (tir.Rank)
                                                 select new ViewTeamInRound()
                                                 {
                                                     Id = tir.Id,
                                                     RoundId = tir.RoundId,
                                                     Rank = tir.Rank,
                                                     Scores = tir.Scores,
                                                     TeamId = tir.TeamId,
                                                     TeamName = t.Name,
                                                     Status = tir.Status
                                                 }).ToListAsync();

            if (teams.Any())
            {
                if (top.HasValue)
                {
                    // update status teams in round
                    var winTeams = teams.Take(top.Value).ToList();
                    var loseTeams = teams.Skip(top.Value).ToList();
                    List<int> winTeamIds = winTeams.Select(team => team.TeamId).ToList();
                    List<int> loseTeamIds = loseTeams.Select(team => team.TeamId).ToList();

                    context.TeamInRounds.Where(team => winTeamIds.Contains(team.TeamId)).ToList()
                                        .ForEach(team => team.Status = true); // status win

                    context.TeamInRounds.Where(team => loseTeamIds.Contains(team.TeamId)).ToList()
                                        .ForEach(team => team.Status = false); // status lose

                    await Update();
                    foreach (var team in winTeams)
                    {
                        team.Status = true;
                        team.MembersInTeam = await GetMembersInTeam(team.TeamId);
                    }

                    return winTeams;
                }

                foreach (var team in teams)
                {
                    team.MembersInTeam = await GetMembersInTeam(team.TeamId);
                    team.NumberOfParticipatedMatches = GetNumberOfParticipatedMatches(team.TeamId, team.RoundId);
                }
            }

            return teams;
        }

        public async Task<bool> CheckExistedTeamsInRound(int roundId)
        {
            var query = from tir in context.TeamInRounds
                        where tir.RoundId.Equals(roundId)
                        select tir;

            return await query.AnyAsync();
        }

        public async Task UpdateResultTeamsInRound(int roundId, int teamId, int? scores, bool? status)
        {
            var teamInRound = await (from tir in context.TeamInRounds
                                     where tir.TeamId.Equals(teamId) && tir.RoundId.Equals(roundId)
                                     select tir).FirstOrDefaultAsync();

            if (teamInRound != null)
            {
                if (scores.HasValue) teamInRound.Scores += scores.Value;

                if (status.HasValue) teamInRound.Status = status.Value;

                await Update();
            }
        }

        public int GetNumberOfParticipatedMatches(int teamId, int roundId)
        {
            return (from tim in context.TeamInMatches
                    join m in context.Matches on tim.MatchId equals m.Id
                    where m.RoundId.Equals(roundId) && tim.TeamId.Equals(teamId)
                            && !m.Status.Equals(MatchStatus.IsDeleted) && !m.Status.Equals(MatchStatus.Cancel)
                    select tim).ToList().Count;
        }

        public async Task<List<TeamInRound>> GetTeamsByRound(int roundId)
        {
            return await (from tir in context.TeamInRounds
                          where tir.RoundId.Equals(roundId) && tir.Status.Equals(true)
                          select tir).ToListAsync();
        }

        public async Task DeleteMultiTeams(List<TeamInRound> teams)
        {
            context.TeamInRounds.RemoveRange(teams);
            await Update();
        }

        public async Task DeleteMultiTeams(List<int> teamIds, int roundId, bool? status)
        {
            var query = from tir in context.TeamInRounds
                        where teamIds.Contains(tir.TeamId) && tir.RoundId.Equals(roundId)
                        select tir;

            if (status.HasValue) query = query.Where(team => team.Status.Equals(status));

            var teams = query.ToList();
            context.TeamInRounds.RemoveRange(teams);
            await Update();
        }
    }
}
