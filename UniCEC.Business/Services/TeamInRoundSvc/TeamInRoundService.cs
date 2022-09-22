using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Business.Services.TeamInRoundSvc
{
    public class TeamInRoundService : ITeamInRoundService
    {
        private ITeamInRoundRepo _teamInRoundRepo;
        private ITeamRepo _teamRepo;
        private ICompetitionRepo _competitionRepo;
        private ICompetitionRoundRepo _competitionRoundRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;

        private DecodeToken _decodeToken;

        public TeamInRoundService(ITeamInRoundRepo teamInRoundRepo, ICompetitionRepo competitionRepo, ITeamRepo teamRepo
                                    , ICompetitionRoundRepo competitionRoundRepo, IMemberInCompetitionRepo memberInCompetitionRepo)
        {
            _teamInRoundRepo = teamInRoundRepo;
            _teamRepo = teamRepo;
            _competitionRepo = competitionRepo;
            _competitionRoundRepo = competitionRoundRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task CheckValidAuthorizedViewer(string token, int competitionId)
        {
            int universityId = _decodeToken.Decode(token, "UniversityId");            
            bool isValid = await _competitionRepo.CheckExisteUniInCompetition(universityId, competitionId);
            if (!isValid) throw new ArgumentException("You do not have permission to access this resource");
            //CompetitionScopeStatus status = await _competitionRepo.GetScopeCompetition(competitionId);
            //if (status.Equals(CompetitionScopeStatus.Club))
            //{
            //    int userId = _decodeToken.Decode(token, "Id");
            //    List<int> clubIds = await _clubRepo.GetByCompetition(competitionId);
            //    if(clubIds != null)
            //    {
            //        foreach (int clubId in clubIds)
            //        {
            //            bool isExisted = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
            //            if(!isExisted) throw new ArgumentException("You do not have permission to access this resource");
            //        }
            //    }
            //}
        }

        private void CheckValidAuthorized(string token, int competitionId)
        {
            int userId = _decodeToken.Decode(token, "Id");            
            bool isValid = _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId, null);
            if (!isValid) throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<PagingResult<ViewTeamInRound>> GetByConditions(string token, TeamInRoundRequestModel request)
        {
            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(request.RoundId);
            if (competitionId == 0) throw new ArgumentException("Not found this competition based on roundId");
            await CheckValidAuthorizedViewer(token, competitionId);            
            
            int userId = _decodeToken.Decode(token, "Id");
            bool isManager = _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId, null);
            if (!isManager) request.Status = true;

            PagingResult<ViewTeamInRound> teamInRounds = await _teamInRoundRepo.GetByConditions(request);
            if (teamInRounds == null) throw new NullReferenceException();
            return teamInRounds;
        }

        public async Task<List<ViewTeamInRound>> InsertTopTeamsToNextRound(string token, int roundId, int top)
        {
            // check valid
            CompetitionRound round = await _competitionRoundRepo.Get(roundId);
            if (round == null || (round != null && (round.Status.Equals(CompetitionRoundStatus.IsDeleted)
                                                    || round.Status.Equals(CompetitionRoundStatus.Cancel))))
                throw new ArgumentException("Not found this round");

            int userId = _decodeToken.Decode(token, "Id");
            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(roundId);
            
            bool isManager = _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId, null);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int nextRoundOrder = round.Order + 1;
            CompetitionRound nextRound = await _competitionRoundRepo.GetRoundAtOrder(competitionId, nextRoundOrder);
            if (nextRound == null) throw new ArgumentException("This is the last round");

            // Action
            List<ViewTeamInRound> teams = await _teamInRoundRepo.GetTeamsByRound(roundId, top);
            if (teams == null) throw new NullReferenceException("Not found any teams in this round");
            List<TeamInRound> teamsInRound = teams.Select(team => new TeamInRound()
            {
                Rank = team.Rank,
                RoundId = nextRound.Id,
                Scores = team.Scores,
                Status = team.Status,
                TeamId = team.TeamId
            }).OrderByDescending(team => team.Scores).ToList();

            for (int index = 0; index < teamsInRound.Count; index++)
            {
                teamsInRound[index].Rank = index + 1;
            }

            await _teamInRoundRepo.InsertMultiTeams(teamsInRound);

            return await _teamInRoundRepo.GetTeamsByRound(nextRound.Id, null);
        }

        public async Task<List<ViewTeamInRound>> Insert(string token, List<TeamInRoundInsertModel> models)
        {
            if (models.Count() == 0) throw new ArgumentException("Null data");

            int competitionRoundId = models[0].RoundId;

            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(competitionRoundId);
            if (competitionId == 0) throw new ArgumentException("Not found this competition based on roundId");

            CheckValidAuthorized(token, competitionId);

            foreach(var model in models)
            {
                if (model.TeamId <= 0 || model.RoundId <= 0 || model.Scores < 0)
                    throw new ArgumentException("Invalid TeamId || Invalid RoundId || Result Null");

                if (model.RoundId != competitionRoundId) throw new ArgumentException("You can not insert teams to more than 1 round in the same time");

                bool isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
                if (!isExisted) throw new ArgumentException("Not found this team");

                isExisted = await _competitionRoundRepo.CheckExistedRound(model.RoundId);
                if (!isExisted) throw new ArgumentException("Not found this round or this round is cancel");
            }

            models.OrderByDescending(tir => tir.Scores);

            List<int> teamIds = new List<int>();
            for(int index = 0; index < models.Count; index++)
            {
                teamIds.Add(models[index].TeamId);
                TeamInRound teamInRound = new TeamInRound()
                {
                    Rank = 0,// rank,
                    Scores = models[index].Scores,
                    RoundId = competitionRoundId,
                    TeamId = models[index].TeamId,
                    Status = true // default status when insert                    
                };

                int teamInRoundId = await _teamInRoundRepo.Insert(teamInRound);
                ViewTeamInRound viewModel = await _teamInRoundRepo.GetById(teamInRoundId, teamInRound.Status);
            }

            await _teamInRoundRepo.UpdateRankTeamsInRound(competitionRoundId);
            return await _teamInRoundRepo.GetViewTeams(teamIds, competitionRoundId);
        }

        public async Task Update(string token, TeamInRoundUpdateModel model)
        {
            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(model.RoundId);
            if (competitionId == 0) throw new ArgumentException("Not found this competition based on roundId");

            CheckValidAuthorized(token, competitionId);

            TeamInRound teamInRound = await _teamInRoundRepo.Get(model.Id);
            if (teamInRound == null) throw new NullReferenceException("Not found this record");

            bool isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
            if (!isExisted) throw new ArgumentException("Not found this team");

            isExisted = await _competitionRoundRepo.CheckExistedRound(model.RoundId);
            if (!isExisted) throw new ArgumentException("Not found this round or this round is cancel");

            if ((model.Scores.HasValue && model.Scores < 0)
                || (model.Rank.HasValue && model.Rank < 0))
                throw new ArgumentException("Scores || Rank must be greater than 0");

            if (model.Scores.HasValue) teamInRound.Scores = model.Scores.Value;

            if (model.Status.HasValue) teamInRound.Status = model.Status.Value;

            if (model.Rank.HasValue) teamInRound.Rank = model.Rank.Value;

            await _teamInRoundRepo.Update();

            if (model.Scores.HasValue || model.Status.HasValue) 
                await _teamInRoundRepo.UpdateRankTeamsInRound(model.RoundId);
        }

        public async Task Delete(string token, int id)
        {
            TeamInRound teamInRound = await _teamInRoundRepo.Get(id);
            if (teamInRound == null) throw new NullReferenceException("Not found this record");

            if (teamInRound.Status.Equals(false)) return;

            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(teamInRound.RoundId);
            if (competitionId == 0) throw new ArgumentException("Not found this competition based on roundId");

            CheckValidAuthorized(token, competitionId);

            teamInRound.Status = false; // deleted status
            await _teamInRoundRepo.Update();            
        }
    }
}
