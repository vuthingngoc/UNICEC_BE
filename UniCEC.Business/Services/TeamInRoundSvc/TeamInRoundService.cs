using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.MemberRepo;
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
        private IMemberRepo _memberRepo;
        private IClubRepo _clubRepo;

        private DecodeToken _decodeToken;

        public TeamInRoundService(ITeamInRoundRepo teamInRoundRepo, ICompetitionRepo competitionRepo, ITeamRepo teamRepo
                                    , ICompetitionRoundRepo competitionRoundRepo, IMemberInCompetitionRepo memberInCompetitionRepo
                                    , IClubRepo clubRepo, IMemberRepo memberRepo)
        {
            _teamInRoundRepo = teamInRoundRepo;
            _teamRepo = teamRepo;
            _competitionRepo = competitionRepo;
            _competitionRoundRepo = competitionRoundRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _clubRepo = clubRepo;
            _memberRepo = memberRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task CheckValidAuthorizedViewer(string token, int competitionId)
        {
            int universityId = _decodeToken.Decode(token, "UniversityId");
            int userId = _decodeToken.Decode(token, "Id");
            bool isValid = await _competitionRepo.CheckExisteUniInCompetition(universityId, competitionId);
            if (!isValid) throw new ArgumentException("You do not have permission to access this resource");
            CompetitionScopeStatus status = await _competitionRepo.GetScopeCompetition(competitionId);
            if (status.Equals(CompetitionScopeStatus.Club))
            {
                List<int> clubIds = await _clubRepo.GetByCompetition(competitionId);
                if(clubIds != null)
                {
                    foreach (int clubId in clubIds)
                    {
                        bool isExisted = await _memberRepo.CheckExistedMemberInClub(userId, clubId);
                        if(!isExisted) throw new ArgumentException("You do not have permission to access this resource");
                    }
                }
            }
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

        public async Task<List<ViewTeamInRound>> GetTopTeamsInCompetition(string token, int competitionId, int top) // not finish
        {
            // check valid
            if (competitionId == 0) throw new ArgumentException("Invalid competition");
            bool isExisted = await _competitionRepo.CheckExistedCompetition(competitionId);
            if (!isExisted) throw new ArgumentException("Not found this competition");

            await CheckValidAuthorizedViewer(token, competitionId);

            // Action
            List<ViewTeamInRound> teamsInRound = await _teamInRoundRepo.GetTopTeamsInCompetition(competitionId, top);
            if(teamsInRound == null) throw new NullReferenceException();
            return teamsInRound;
        }

        //public Task<ViewTeamInRound> GetById(string token, int id)
        //{
        //    ViewTeamInRound teamInRound = 
        //    throw new NotImplementedException();
        //}
        //}

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

                isExisted = await _teamRepo.CheckExistedTeam(model.TeamId);
                if (!isExisted) throw new ArgumentException("Not found this team");
            }

            models.OrderByDescending(tir => tir.Scores);
            int rank = 1;

            List<ViewTeamInRound> teamInRounds = new List<ViewTeamInRound>();
            foreach (var model in models)
            {
                TeamInRound teamInRound = new TeamInRound()
                {
                    Id = model.RoundId,
                    Rank = rank,
                    Scores = model.Scores,
                    RoundId = model.RoundId,
                    TeamId = model.TeamId,
                    Status = true // default status when insert                    
                };

                int teamInRoundId = await _teamInRoundRepo.Insert(teamInRound);
                ViewTeamInRound viewModel = await _teamInRoundRepo.GetById(teamInRoundId, teamInRound.Status);
                teamInRounds.Add(viewModel);
                rank++;
            }

            return teamInRounds;
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
            if (!isExisted) throw new ArgumentException("Not found this round");

            if (model.Scores.HasValue) teamInRound.Scores = model.Scores.Value;

            if (model.Status.HasValue && model.Status.Value.Equals(true)) teamInRound.Status = model.Status.Value;

            if (model.Rank.HasValue && model.Rank.Value > 0) teamInRound.Rank = model.Rank.Value;

            await _teamInRoundRepo.Update();
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
