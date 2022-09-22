using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundTypeRepo;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;
using System.Linq;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public class CompetitionRoundService : ICompetitionRoundService
    {
        private ICompetitionRoundRepo _competitionRoundRepo;

        private ICompetitionRoundTypeRepo _competitionRoundTypeRepo;

        private ICompetitionRepo _competitionRepo;

        private IMemberInCompetitionRepo _memberInCompetitionRepo;

        private ITeamRepo _teamRepo;

        private ITeamInRoundRepo _teamInRoundRepo;

        private IMatchRepo _matchRepo;

        private DecodeToken _decodeToken;

        public CompetitionRoundService(
            ICompetitionRoundRepo competitionRoundRepo,
            ICompetitionRoundTypeRepo competitionRoundTypeRepo,
            ICompetitionRepo competitionRepo,
            IMemberInCompetitionRepo memberInCompetitionRepo,
            ITeamRepo teamRepo,
            ITeamInRoundRepo teamInRoundRepo,
            IMatchRepo matchRepo)
        {
            _competitionRoundRepo = competitionRoundRepo;
            _competitionRoundTypeRepo = competitionRoundTypeRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionRepo = competitionRepo;
            _teamRepo = teamRepo;
            _teamInRoundRepo = teamInRoundRepo;
            _matchRepo = matchRepo;
            _decodeToken = new DecodeToken();
        }

        private void CheckValidAuthorized(string token, int competitionId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isValidUser = _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId, null);
            if (!isValidUser) throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel request)
        {
            PagingResult<ViewCompetitionRound> competitionRounds = await _competitionRoundRepo.GetByConditions(request);
            if (competitionRounds == null) throw new NullReferenceException();
            return competitionRounds;
        }

        public async Task<ViewCompetitionRound> GetById(string token, int id)
        {
            ViewCompetitionRound competitionRound = await _competitionRoundRepo.GetById(id, null);
            if (competitionRound == null) throw new NullReferenceException();

            int userId = _decodeToken.Decode(token, "Id");
            bool isManager = _memberInCompetitionRepo.CheckValidManagerByUser(competitionRound.CompetitionId, userId, null);
            if (!isManager && competitionRound.Status.Equals(CompetitionRoundStatus.IsDeleted)) throw new NullReferenceException();

            // trigger add teams in round
            if (isManager)
            {
                //bool isExisted = await _teamInRoundRepo.CheckExistedTeamsInRound(competitionRound.Id);
                //if (isExisted) return competitionRound;

                if (competitionRound.Order.Equals(1)) // first round
                {
                    bool isExisted = await _teamInRoundRepo.CheckExistedTeamsInRound(competitionRound.Id);
                    if (isExisted) return competitionRound;
                    // 
                    List<int> teamIds = await _teamRepo.GetAllTeamIdsInComp(competitionRound.CompetitionId);
                    await _teamInRoundRepo.InsertMultiTeams(teamIds, id);
                }
                else // the nth round
                {
                    int previousRoundOrder = competitionRound.Order - 1;
                    CompetitionRound previousRound = await _competitionRoundRepo.GetRoundAtOrder(competitionRound.CompetitionId, previousRoundOrder);
                    if (previousRound.Status.Equals(CompetitionRoundStatus.Finished))
                    {
                        List<TeamInRound> teams = await _teamInRoundRepo.GetTeamsByRound(previousRound.Id);
                        // check existed rows in next round
                        bool isExisted = await _teamInRoundRepo.CheckExistedTeamsInRound(id);
                        if (!isExisted)
                        {
                            teams = teams.Select(team => new TeamInRound()
                            {
                                Rank = team.Rank,
                                RoundId = id,
                                Scores = team.Scores,
                                Status = team.Status,
                                TeamId = team.TeamId
                            }).ToList();
                            await _teamInRoundRepo.InsertMultiTeams(teams);
                        }
                        else // update if any change
                        {
                            List<int> selectedRoundTeamIds = await _teamInRoundRepo.GetTeamIdsByRound(id, null);
                            List<TeamInRound> previousRoundTeams = await _teamInRoundRepo.GetTeamsByRound(previousRound.Id);

                            List<TeamInRound> differentPartTeams = 
                                previousRoundTeams.Where(team => !selectedRoundTeamIds.Contains(team.TeamId)).ToList();

                            if (differentPartTeams.Any())
                            {
                                differentPartTeams = differentPartTeams.Select(team => new TeamInRound()
                                {
                                    Rank = team.Rank,
                                    RoundId = id,
                                    Scores = team.Scores,
                                    Status = team.Status,
                                    TeamId = team.TeamId
                                }).ToList();
                                await _teamInRoundRepo.InsertMultiTeams(differentPartTeams);
                            }
                            else // empty list 
                            {
                                List<int> previousRoundTeamIds = previousRoundTeams.Select(team => team.TeamId).ToList();
                                List<int> differentPartTeamIds = 
                                    selectedRoundTeamIds.Where(teamId => !previousRoundTeamIds.Contains(teamId)).ToList();
                                // just delete teams with status true
                                await _teamInRoundRepo.DeleteMultiTeams(differentPartTeamIds, id, true); 
                            }
                        }

                        await _teamInRoundRepo.UpdateRankTeamsInRound(id);
                    }
                }
            }

            return competitionRound;
        }

        public async Task<List<ViewCompetitionRound>> Insert(string token, List<CompetitionRoundInsertModel> models)
        {
            if (models.Count == 0) throw new ArgumentException();
            int competitionId = models[0].CompetitionId;
            CheckValidAuthorized(token, competitionId);

            Competition competition = await _competitionRepo.Get(competitionId);
            if (competition == null) throw new ArgumentException("Not found this competition");
            if (competition.Status.Equals(CompetitionStatus.End) || competition.Status.Equals(CompetitionStatus.Complete)
                || competition.Status.Equals(CompetitionStatus.Cancel))
                throw new ArgumentException("Can not access the cancel or ending competition");

            List<ViewCompetitionRound> viewCompetitionRounds = new List<ViewCompetitionRound>();
            DateTime timePreviousRound = new LocalTime().GetLocalTime().DateTime;
            List<string> titleRounds = new List<string>();

            models.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            foreach (var model in models)
            {
                // check in list
                if (model.CompetitionId.Equals(0) || string.IsNullOrEmpty(model.Title) || model.RoundTypeId.Equals(0)
                    || model.StartTime.Equals(DateTime.MinValue) || model.EndTime.Equals(DateTime.MinValue)
                    || string.IsNullOrEmpty(model.Description) || model.SeedsPoint < 0)
                    throw new ArgumentNullException("CompetitionId Null || Title Null || RoundTypeId Null " +
                        "|| Description Null || StartTime Null || EndTime Null || SeedsPoint can not be negative number");

                if (!model.CompetitionId.Equals(competitionId))
                    throw new ArgumentException("You do not have permission to insert rounds for more than 1 competitions in the same time");

                bool isExisted = await _competitionRoundTypeRepo.CheckExistedType(model.RoundTypeId);
                if (!isExisted) throw new ArgumentException("Not found this round type");

                // start comparing in 2nd element
                if (titleRounds.Count > 0)
                {
                    foreach (var title in titleRounds)
                    {
                        if (title.ToLower().Equals(model.Title.ToLower())) throw new ArgumentException("Duplicated title round in this competition");
                    }
                }

                titleRounds.Add(model.Title);

                if (model.SeedsPoint < 0) throw new ArgumentException("SeedsPoint must be greater than 0");

                if (model.StartTime < timePreviousRound || model.EndTime <= model.StartTime
                    || model.StartTime < competition.StartTime || model.StartTime > competition.EndTime
                    || model.EndTime > competition.EndTime)
                    throw new ArgumentException("StartTime < EndTime and time in each round is in order follow by competition");

                timePreviousRound = model.EndTime;

                // check in db
                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionId, model.Title, model.StartTime, null, null);
                if (roundId > 0) throw new ArgumentException("Duplicated round in this competition");
            }

            // insert round            
            int count = await _competitionRoundRepo.GetNumberOfRoundsByCompetition(competitionId) + 1;

            foreach (var model in models)
            {
                CompetitionRound competitionRound = new CompetitionRound()
                {
                    CompetitionId = competitionId,
                    CompetitionRoundTypeId = model.RoundTypeId,
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    NumberOfTeam = 0, // default number when insert round
                    SeedsPoint = model.SeedsPoint,
                    Status = CompetitionRoundStatus.Active, // default status insert
                    Order = count
                };
                int id = await _competitionRoundRepo.Insert(competitionRound);
                ViewCompetitionRound viewCompetitionRound = await _competitionRoundRepo.GetById(id, CompetitionRoundStatus.Active);
                viewCompetitionRounds.Add(viewCompetitionRound);

                ++count;
            }

            return viewCompetitionRounds;
        }

        public async Task Update(string token, CompetitionRoundUpdateModel model)
        {
            CompetitionRound competitionRound = await _competitionRoundRepo.Get(model.Id);
            if (competitionRound == null) throw new NullReferenceException("Not found this competition round");

            CheckValidAuthorized(token, competitionRound.CompetitionId);

            Competition competition = await _competitionRepo.Get(competitionRound.CompetitionId);
            if (competition == null) throw new ArgumentException("Not found this competition");

            if (competition.Status.Equals(CompetitionStatus.End) || competition.Status.Equals(CompetitionStatus.Complete)
                || competition.Status.Equals(CompetitionStatus.Cancel))
                throw new ArgumentException("Can not access the cancel or ending competition");

            DateTime currentTime = new LocalTime().GetLocalTime().DateTime;
            TimeSpan timeSpan = competitionRound.StartTime - currentTime;

            if ((!string.IsNullOrEmpty(model.Title) || model.RoundTypeId.HasValue || !string.IsNullOrEmpty(model.Description)
                || model.StartTime.HasValue || model.EndTime.HasValue || model.NumberOfTeam.HasValue || model.SeedsPoint.HasValue)
                && timeSpan.TotalMinutes < 10)
                throw new ArgumentException("Can not update round < 10 mins before round start");

            if (!string.IsNullOrEmpty(model.Title))
            {
                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, model.Title, null, null, null);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated title competition round");
                competitionRound.Title = model.Title;
            }

            if (model.RoundTypeId.HasValue) competitionRound.CompetitionRoundTypeId = model.RoundTypeId.Value;

            if (!string.IsNullOrEmpty(model.Description)) competitionRound.Description = model.Description;

            if (model.StartTime.HasValue)
            {
                if (model.StartTime.Value < competition.StartTime || model.StartTime.Value > competition.EndTime)
                    throw new ArgumentException("Invalid startTime");

                if (model.EndTime.HasValue && model.StartTime.Value >= model.EndTime.Value)
                    throw new ArgumentException("Invalid time");
                else if (!model.EndTime.HasValue && model.StartTime.Value >= competitionRound.EndTime)
                    throw new ArgumentException("Invalid startTime");

                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, null, model.StartTime.Value, null, null);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated time of another competition round");
                competitionRound.StartTime = model.StartTime.Value;
            }

            if (model.EndTime.HasValue)
            {
                if (model.EndTime.Value > competition.EndTime) throw new ArgumentException("Invalid endTime");

                if (!model.StartTime.HasValue && model.EndTime.Value <= competitionRound.StartTime)
                    throw new ArgumentException("Invalid endTime");

                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, null, null, model.EndTime.Value, competitionRound.Order);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated time of another competition round");
                competitionRound.EndTime = model.EndTime.Value;
            }

            if (model.NumberOfTeam.HasValue)
            {
                if (model.NumberOfTeam.Value < 0) throw new ArgumentException("Number of team must greater than 0");
                competitionRound.NumberOfTeam = model.NumberOfTeam.Value;
            }

            if (model.SeedsPoint.HasValue)
            {
                if (model.SeedsPoint.Value < 0) throw new ArgumentException("SeedsPoint must greater than 0");
                competitionRound.SeedsPoint = model.SeedsPoint.Value;
            }

            if (model.Status.HasValue)
            {
                // check valid status
                if (!Enum.IsDefined(typeof(CompetitionRoundStatus), model.Status.Value))
                    throw new ArgumentException("Invalid competition round status");

                competitionRound.Status = model.Status.Value;
                MatchStatus matchStatus = MatchStatus.Ready; // default match status
                if (model.Status.Equals(CompetitionRoundStatus.Cancel))
                {
                    matchStatus = MatchStatus.Cancel;
                }
                else if (model.Status.Equals(CompetitionRoundStatus.Finished))
                {
                    matchStatus = MatchStatus.Finish;
                }
                else if (model.Status.Equals(CompetitionRoundStatus.IsDeleted))
                {
                    matchStatus = MatchStatus.IsDeleted;
                }

                // update status matches in round
                await _matchRepo.UpdateStatusMatchesByRound(competitionRound.Id, matchStatus);
            }

            await _competitionRoundRepo.Update();

            if (model.StartTime.HasValue || model.EndTime.HasValue || model.Status.HasValue)
                await _competitionRoundRepo.UpdateOrderRoundsByCompe(competitionRound.CompetitionId);
        }

        public async Task Delete(string token, int id)
        {
            CompetitionRound competitionRound = await _competitionRoundRepo.Get(id);
            if (competitionRound == null) throw new NullReferenceException("Not found this competition round");

            CheckValidAuthorized(token, competitionRound.CompetitionId);

            if (competitionRound.Status.Equals(CompetitionRoundStatus.Happening)
                || competitionRound.Status.Equals(CompetitionRoundStatus.Finished))
                throw new ArgumentException("Can not delete round when It's happening or finished");

            competitionRound.Status = CompetitionRoundStatus.IsDeleted;
            competitionRound.Order = 0;
            await _competitionRoundRepo.Update();
            await _competitionRoundRepo.UpdateOrderRoundsByCompe(competitionRound.CompetitionId);

            // update status matches in round
            await _matchRepo.UpdateStatusMatchesByRound(competitionRound.Id, MatchStatus.Cancel);
        }
    }
}
