using System;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInMatchRepo;
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
        private IMatchRepo _matchRepo;
        private ITeamRepo _teamRepo;
        private DecodeToken _decodeToken;

        public TeamInMatchService(ITeamInMatchRepo teamInMatchRepo, IMemberInCompetitionRepo memberInCompetitionRepo
                                    , IMatchRepo matchRepo, ITeamRepo teamRepo)
        {
            _teamInMatchRepo = teamInMatchRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
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

        public async Task<ViewTeamInMatch> Insert(TeamInMatchInsertModel model, string token)
        {
            if (model.MatchId.Equals(0) || model.TeamId.Equals(0) || model.Scores < 0)
                throw new ArgumentException("MatchId Null || TeamId Null || Scores < 0");

            bool isExisted = await _matchRepo.CheckAvailableMatchId(model.MatchId);
            if (!isExisted) throw new ArgumentException("Not found this match");

            isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
            if (!isExisted) throw new ArgumentException("Not found this team");

            bool isManager = await IsValidCompetitionManager(model.MatchId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            bool isDuplicated = _teamInMatchRepo.CheckDuplicatedTeamInMatch(model.MatchId, model.TeamId, null);
            if (isDuplicated) throw new ArgumentException("Duplicated team in match");

            TeamInMatch teamInMatch = new TeamInMatch()
            {
                Description = model.Description ?? "",
                MatchId = model.MatchId,
                TeamId = model.TeamId,
                Scores = model.Scores,
                Status = model.Status
            };
            
            int id = await _teamInMatchRepo.Insert(teamInMatch);
            return await _teamInMatchRepo.GetById(id);
        }

        public async Task Update(TeamInMatchUpdateModel model, string token)
        {
            TeamInMatch teamInMatch = await _teamInMatchRepo.Get(model.Id);
            if (teamInMatch == null) throw new NullReferenceException("Not found result of the team in match");

            // validation data
            if(model.MatchId.HasValue || model.TeamId.HasValue)
            {
                int matchId = model.MatchId ?? teamInMatch.MatchId;
                int teamId = model.TeamId ?? teamInMatch.TeamId;
                bool isDuplicated = _teamInMatchRepo.CheckDuplicatedTeamInMatch(matchId, teamId, model.Id);
                if (isDuplicated) throw new ArgumentException("Duplicated team in match");

                bool isExisted = await _matchRepo.CheckAvailableMatchId(matchId);
                if (!isExisted) throw new ArgumentException("Not found this match");

                isExisted = await _teamRepo.CheckExistedTeam(teamId);
                if (!isExisted) throw new ArgumentException("Not found this team");
            }

            bool isManager = await IsValidCompetitionManager(teamInMatch.MatchId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            if (model.Scores.HasValue && model.Scores.Value < 0) throw new ArgumentException("The scores must greater than 0");

            if (model.Status.HasValue && !Enum.IsDefined(typeof(TeamInMatchStatus), model.Status.Value))
                throw new ArgumentException("Invalid team in match status");

            // update
            if (model.MatchId.HasValue) teamInMatch.MatchId = model.MatchId.Value;

            if(model.TeamId.HasValue) teamInMatch.TeamId = model.TeamId.Value;

            if(model.Scores.HasValue) teamInMatch.Scores = model.Scores.Value;

            if (model.Status.HasValue) teamInMatch.Status = model.Status.Value;

            if(!string.IsNullOrEmpty(model.Description)) teamInMatch.Description = model.Description;

            await _teamInMatchRepo.Update();
        }
    }
}
