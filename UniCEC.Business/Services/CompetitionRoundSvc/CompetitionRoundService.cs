using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Common;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public class CompetitionRoundService : ICompetitionRoundService
    {
        private ICompetitionRoundRepo _competitionRoundRepo;
        
        private ICompetitionRepo _competitionRepo;

        private IMemberInCompetitionRepo _memberInCompetitionRepo;

        private DecodeToken _decodeToken;

        public CompetitionRoundService(ICompetitionRoundRepo competitionRoundRepo,  ICompetitionRepo competitionRepo, IMemberInCompetitionRepo memberInCompetitionRepo)
        {
            _competitionRoundRepo = competitionRoundRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionRepo = competitionRepo;
            _decodeToken = new DecodeToken();
        }

        private void CheckValidAuthorized(string token, int competitionId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isValidUser = _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, userId);
            if (!isValidUser) throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<PagingResult<ViewCompetitionRound>> GetByConditions(string token, CompetitionRoundRequestModel request)
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
            bool isValidUser = _memberInCompetitionRepo.CheckValidManagerByUser(competitionRound.Id, userId);
            if (!isValidUser && competitionRound.Status.Equals(false)) throw new NullReferenceException();

            return competitionRound;
        }

        public async Task<List<ViewCompetitionRound>> Insert(string token, List<CompetitionRoundInsertModel> models)
        {
            if (models.Count == 0) throw new ArgumentException();
            int competitionId = models[0].CompetitionId;
            CheckValidAuthorized(token, competitionId);

            Competition competition = await _competitionRepo.Get(competitionId);
            if (competition == null) throw new ArgumentException("Not found this competition");
            if (competition.Status.Equals(4) || competition.Status.Equals(5)) throw new ArgumentException("Can not access the cancel or ending competition");

            List<ViewCompetitionRound> viewCompetitionRounds = new List<ViewCompetitionRound>();
            DateTime timePreviousRound = new LocalTime().GetLocalTime().DateTime;
            List<string> titleRounds = new List<string>();


            foreach (var model in models)
            {
                // check in list
                if (model.CompetitionId.Equals(0) || string.IsNullOrEmpty(model.Title)
                    || model.StartTime.Equals(DateTime.MinValue) || model.EndTime.Equals(DateTime.MinValue)
                    || string.IsNullOrEmpty(model.Description) || model.SeedsPoint.Equals(0))
                    throw new ArgumentNullException("CompetitionId Null || Title Null || Description Null || StartTime Null " +
                        "|| EndTime Null || SeedsPoint Null");

                if (!model.CompetitionId.Equals(competitionId))
                    throw new ArgumentException("You do not have permission to insert rounds for more than 1 competitions in the same time");

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
                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionId, model.Title, model.StartTime, model.EndTime);
                if (roundId > 0) throw new ArgumentException("Duplicated round in this competition");
            }

            foreach (var model in models)
            {
                CompetitionRound competitionRound = new CompetitionRound()
                {
                    CompetitionId = competitionId,
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    NumberOfTeam = 0, // default number when insert round
                    SeedsPoint = model.SeedsPoint,
                    Status = CompetitionRoundStatus.Active // default status insert
                };
                int id = await _competitionRoundRepo.Insert(competitionRound);
                ViewCompetitionRound viewCompetitionRound = new ViewCompetitionRound()
                {
                    Id = id,
                    CompetitionId = competitionId,
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    NumberOfTeam = competitionRound.NumberOfTeam,
                    SeedsPoint = model.SeedsPoint,
                    Status = competitionRound.Status
                };
                viewCompetitionRounds.Add(viewCompetitionRound);
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
            if (competition.Status.Equals(4) || competition.Status.Equals(5)) throw new ArgumentException("Can not access the cancel or ending competition");
            
            if (!string.IsNullOrEmpty(model.Title))
            {
                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, model.Title, null, null);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated title competition round");
                competitionRound.Title = model.Title;
            }

            if (!string.IsNullOrEmpty(model.Description)) competitionRound.Description = model.Description;

            if (model.StartTime.HasValue) 
            {
                if (model.StartTime.Value < competition.StartTime || model.StartTime.Value > competition.EndTime)
                    throw new ArgumentException("Invalid startTime");

                if (model.EndTime.HasValue && model.StartTime.Value >= model.EndTime.Value)
                    throw new ArgumentException("Invalid time");
                else if(!model.EndTime.HasValue && model.StartTime.Value >= competitionRound.EndTime)
                    throw new ArgumentException("Invalid startTime");

                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, null, model.StartTime.Value, null);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated time of another competition round");
                competitionRound.StartTime = model.StartTime.Value;
            }

            if (model.EndTime.HasValue) 
            {
                if(model.EndTime.Value > competition.EndTime) throw new ArgumentException("Invalid endTime");

                if(!model.StartTime.HasValue && model.EndTime.Value <= competitionRound.StartTime)
                    throw new ArgumentException("Invalid endTime");

                int roundId = await _competitionRoundRepo.CheckInvalidRound(competitionRound.CompetitionId, null, null, model.EndTime.Value);
                if (roundId > 0 && !roundId.Equals(model.Id)) throw new ArgumentException("Duplicated time of another competition round");
                competitionRound.EndTime = model.EndTime.Value;
            }

            if (model.NumberOfTeam.HasValue)
            {
                if (model.NumberOfTeam.Value <= 0) throw new ArgumentException("Number of team must greater than 0");
                competitionRound.NumberOfTeam = model.NumberOfTeam.Value;
            }

            if (model.SeedsPoint.HasValue)
            {
                if(model.SeedsPoint.Value <= 0) throw new ArgumentException("SeedsPoint must greater than 0");
                competitionRound.SeedsPoint = model.SeedsPoint.Value;
            }

            await _competitionRoundRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            CheckValidAuthorized(token, id);

            CompetitionRound competitionRound = await _competitionRoundRepo.Get(id);
            if (competitionRound == null || (competitionRound != null && !competitionRound.CompetitionId.Equals(id)))
                throw new NullReferenceException("Not found this competition round");

            competitionRound.Status = CompetitionRoundStatus.Cancel;
            await _competitionRoundRepo.Update();
        }
    }
}
