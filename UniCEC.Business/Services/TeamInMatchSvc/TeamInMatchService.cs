using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.Business.Services.TeamInMatchSvc
{
    public class TeamInMatchService : ITeamInMatchService
    {
        private ITeamInMatchRepo _teamInMatchRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private ICompetitionRoundRepo _competitionRoundRepo;
        private ITeamInRoundRepo _teamInRoundRepo;
        private IMatchRepo _matchRepo;
        private ITeamRepo _teamRepo;
        private DecodeToken _decodeToken;

        public TeamInMatchService(ITeamInMatchRepo teamInMatchRepo, IMemberInCompetitionRepo memberInCompetitionRepo,
                                    ICompetitionRoundRepo competitionRoundRepo, IMatchRepo matchRepo, ITeamRepo teamRepo,
                                    ITeamInRoundRepo teamInRoundRepo)
        {
            _teamInMatchRepo = teamInMatchRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionRoundRepo = competitionRoundRepo;
            _teamInRoundRepo = teamInRoundRepo;
            _matchRepo = matchRepo;
            _teamRepo = teamRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task<bool> IsValidCompetitionManager(int matchId, string token)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int competitionId = await _matchRepo.GetCompetitionIdByMatch(matchId);
            return _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId, null);
        }

        public async Task Delete(int id, string token)
        {
            TeamInMatch teamInMatch = await _teamInMatchRepo.Get(id);
            if (teamInMatch == null) throw new NullReferenceException("Not found this team in match");

            bool isManager = await IsValidCompetitionManager(teamInMatch.MatchId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            //teamInMatch.Status = TeamInMatchStatus.IsDeleted; // delete status
            await _teamInMatchRepo.Delete(teamInMatch);
        }

        public async Task<PagingResult<ViewTeamInMatch>> GetByConditions(TeamInMatchRequestModel request, string token)
        {
            PagingResult<ViewTeamInMatch> teamsInMatch = await _teamInMatchRepo.GetByConditions(request);
            if (teamsInMatch == null) throw new NullReferenceException("Not found any result of teams in match");

            //if (!string.IsNullOrEmpty(token))
            //{
            //    bool isManager = await IsValidCompetitionManager(teamsInMatch.Items[0].MatchId, token);
            //    if (!isManager)
            //    {
            //        teamsInMatch.Items = teamsInMatch.Items.Where(team => team.Status.)
            //    }                 
            //}
            // Now, token is no use any more, it use for future if any

            return teamsInMatch;
        }

        public async Task<ViewTeamInMatch> GetById(int id, string token)
        {
            ViewTeamInMatch teamInMatch = await _teamInMatchRepo.GetById(id);
            if (teamInMatch == null) throw new NullReferenceException("Not found this result of team in match");
            return teamInMatch;
        }

        public async Task<List<ViewTeamInMatch>> Insert(List<TeamInMatchInsertModel> models, string token)
        {
            if (models.Count.Equals(0)) throw new ArgumentException("Data Input Null");

            int matchId = models[0].MatchId;
            bool isDuplicated = models.GroupBy(x => x.TeamId)
                                      .Where(g => g.Count() > 1)
                                      .Select(y => y.Key)
                                      .ToList().Count > 0;
            if (isDuplicated) throw new ArgumentException("Duplicated teams in round");

            foreach (var model in models)
            {
                if (model.MatchId.Equals(0) || model.Scores < 0)
                    throw new ArgumentException("MatchId Null || Scores < 0");

                bool isExisted = await _matchRepo.CheckAvailableMatchId(model.MatchId);
                if (!isExisted) throw new ArgumentException("Not found this match");

                isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
                if (!isExisted) throw new ArgumentException("Not found this team");

                isDuplicated = _teamInMatchRepo.CheckDuplicatedTeamInMatch(model.MatchId, model.TeamId, null);
                if (isDuplicated) throw new ArgumentException("Duplicated team in match");

                bool isManager = await IsValidCompetitionManager(model.MatchId, token);
                if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

                if (!model.MatchId.Equals(matchId)) throw new ArgumentException("Can not insert multiple matches in one time");
            }

            // insert
            List<ViewTeamInMatch> teams = new List<ViewTeamInMatch>();
            foreach (var model in models)
            {
                TeamInMatch teamInMatch = new TeamInMatch()
                {
                    Description = model.Description ?? "",
                    MatchId = model.MatchId,
                    TeamId = model.TeamId,
                    Scores = model.Scores,
                    Status = model.Status
                };

                int id = await _teamInMatchRepo.Insert(teamInMatch);
                ViewTeamInMatch team = await _teamInMatchRepo.GetById(id);
                teams.Add(team);
            }

            return teams;
        }

        public async Task Update(List<TeamInMatchUpdateModel> models, string token)
        {
            if (models.Count.Equals(0)) throw new ArgumentException("Input Data Null");

            int matchId = models[0].MatchId;
            bool isInValid = models.Where(model => model.MatchId != matchId).ToList().Count > 0;
            if (isInValid) throw new ArgumentException("Can not update 2 match in one time");

            foreach (var model in models)
            {
                TeamInMatch teamInMatch = await _teamInMatchRepo.Get(model.Id);
                if (teamInMatch == null) throw new NullReferenceException("Not found result of the team in match");

                // validation data
                if (model.MatchId.Equals(0) || model.TeamId.Equals(0)) throw new ArgumentException("MatchId Null || TeamId Null");

                bool isDuplicated = _teamInMatchRepo.CheckDuplicatedTeamInMatch(model.MatchId, model.TeamId, model.Id);
                if (isDuplicated) throw new ArgumentException("Duplicated team in match");

                bool isExisted = await _matchRepo.CheckAvailableMatchId(model.MatchId);
                if (!isExisted) throw new ArgumentException("Not found this match");

                isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
                if (!isExisted) throw new ArgumentException("Not found this team");

                bool isManager = await IsValidCompetitionManager(teamInMatch.MatchId, token);
                if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

                if (model.Scores.HasValue && model.Scores.Value < 0) throw new ArgumentException("The scores must greater than 0");

                if (model.Status.HasValue && !Enum.IsDefined(typeof(TeamInMatchStatus), model.Status.Value))
                    throw new ArgumentException("Invalid team in match status");

                // update
                int roundTypeId = await _competitionRoundRepo.GetRoundTypeByMatch(model.MatchId);

                teamInMatch.MatchId = model.MatchId;

                teamInMatch.TeamId = model.TeamId;

                if (model.Scores.HasValue) teamInMatch.Scores = model.Scores.Value;

                if (model.Status.HasValue)
                {
                    teamInMatch.Status = (roundTypeId.Equals(2)) // Round robin type
                                            ? TeamInMatchStatus.Win
                                            : model.Status.Value;
                }

                if (!string.IsNullOrEmpty(model.Description)) teamInMatch.Description = model.Description;

                await _teamInMatchRepo.Update();

                // update to team in round                
                int roundId = await _teamInMatchRepo.GetRoundIdByMatch(model.MatchId);

                if (roundTypeId.Equals(1)) // elimination type
                {
                    bool status = true;
                    if (model.Status.HasValue && model.Status.Equals(TeamInMatchStatus.Lose)) status = false;

                    await _teamInRoundRepo.UpdateResultTeamsInRound(roundId, model.TeamId, null, status);
                    await _teamInRoundRepo.UpdateRankTeamsInRound(roundId);
                }
                else if (roundTypeId.Equals(2)) // round robin type
                {
                    if (model.Scores.HasValue)
                    {
                        await _teamInRoundRepo.UpdateResultTeamsInRound(roundId, model.TeamId, model.Scores.Value, null);
                        await _teamInRoundRepo.UpdateRankTeamsInRound(roundId);
                    }
                }
                else if (roundTypeId.Equals(3)) // combination type
                {
                    bool isLoseMatch = await _teamInMatchRepo.CheckIsLoseMatch(model.MatchId);
                    if (isLoseMatch)
                    {
                        if (model.Status.HasValue && model.Status.Equals(TeamInMatchStatus.Lose)) // eliminate lose team
                        {
                            await _teamInRoundRepo.UpdateResultTeamsInRound(roundId, model.TeamId, null, false);
                        }
                        else if (model.Status.HasValue && model.Status.Equals(TeamInMatchStatus.Win)) // update win team to result of round
                        {
                            await _teamInRoundRepo.UpdateResultTeamsInRound(roundId, model.TeamId, null, true);
                        }
                    }
                    else
                    {
                        bool status = true;
                        if (model.Status.HasValue && model.Status.Equals(TeamInMatchStatus.Lose)) status = false;

                        await _teamInRoundRepo.UpdateResultTeamsInRound(roundId, model.TeamId, null, status);
                    }
                }
            }
        }
    }
}
