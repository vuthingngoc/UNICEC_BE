using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.ICompetitionManagerRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Business.Services.CompetitionRoundSvc
{
    public class CompetitionRoundService : ICompetitionRoundService
    {
        private ICompetitionRoundRepo _competitionRoundRepo;
        private ICompetitionManagerRepo _competitionManagerRepo;

        private DecodeToken _decodeToken;

        public CompetitionRoundService(ICompetitionRoundRepo competitionRoundRepo, ICompetitionManagerRepo competitionManager)
        {
            _competitionRoundRepo = competitionRoundRepo;
            _competitionManagerRepo = competitionManager;
            _decodeToken = new DecodeToken();
        }

        private void CheckValidAuthorizedAsync(string token, int competitionId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            bool isValidUser = _competitionManagerRepo.CheckValidManagerByUser(competitionId, userId);
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
            if(competitionRound == null) throw new NullReferenceException();
            
            int userId = _decodeToken.Decode(token, "Id");
            bool isValidUser = _competitionManagerRepo.CheckValidManagerByUser(competitionRound.Id, userId);
            if (!isValidUser && competitionRound.Status.Equals(false)) throw new NullReferenceException();

            return competitionRound;
        }

        public async Task<List<ViewCompetitionRound>> Insert(string token, List<CompetitionRoundInsertModel> models)
        {
            if (models.Count == 0) throw new ArgumentException();
            int competitionId = models[0].CompetitionId;
            CheckValidAuthorizedAsync(token, competitionId);

            List<ViewCompetitionRound> viewCompetitionRounds = new List<ViewCompetitionRound>();

            foreach(var model in models)
            {
                if (model.CompetitionId.Equals(0) || !model.CompetitionId.Equals(competitionId) || string.IsNullOrEmpty(model.Title) 
                    || string.IsNullOrEmpty(model.Description) || model.StartTime.Equals(DateTime.MinValue) || model.EndTime.Equals(DateTime.MinValue) 
                    || model.NumberOfTeam.Equals(0) || model.SeedsPoint.Equals(0)) 
                        throw new ArgumentException();

                CompetitionRound competitionRound = new CompetitionRound()
                {
                    CompetitionId = competitionId,
                    Title = model.Title,
                    Description = model.Description,                    
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    NumberOfTeam = model.NumberOfTeam,
                    SeedsPoint = model.SeedsPoint,
                    Status = true // default status insert
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
                    NumberOfTeam = model.NumberOfTeam,
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

            CheckValidAuthorizedAsync(token, competitionRound.CompetitionId);

            if(!string.IsNullOrEmpty(model.Title)) competitionRound.Title = model.Title;

            if(!string.IsNullOrEmpty(model.Description)) competitionRound.Description = model.Description;

            if(model.StartTime.HasValue) competitionRound.StartTime = model.StartTime.Value;

            if(model.EndTime.HasValue) competitionRound.EndTime = model.EndTime.Value;

            if(model.NumberOfTeam.HasValue) competitionRound.NumberOfTeam = model.NumberOfTeam.Value;

            if(model.SeedsPoint.HasValue) competitionRound.SeedsPoint = model.SeedsPoint.Value;

            await _competitionRoundRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            CheckValidAuthorizedAsync(token, id);

            CompetitionRound competitionRound = await _competitionRoundRepo.Get(id);
            if (competitionRound == null || (competitionRound != null && !competitionRound.CompetitionId.Equals(id)))
                throw new NullReferenceException("Not found this competition round");

            competitionRound.Status = false;
            await _competitionRoundRepo.Update();
        }
    }
}
