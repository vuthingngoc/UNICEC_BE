using System;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Enum;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo;
using UniCEC.Data.Repository.ImplRepo.MatchRepo;
using UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;
using System.Linq;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Common;

namespace UniCEC.Business.Services.MatchSvc
{
    public class MatchService : IMatchService
    {
        private IMatchRepo _matchRepo;
        private IMemberInCompetitionRepo _memberInCompetitionRepo;
        private ICompetitionRoundRepo _competitionRoundRepo;
        private DecodeToken _decodeToken;

        public MatchService(IMatchRepo matchRepo, IMemberInCompetitionRepo memberInCompetitionRepo
                                , ICompetitionRoundRepo competitionRoundRepo)
        {
            _matchRepo = matchRepo;
            _memberInCompetitionRepo = memberInCompetitionRepo;
            _competitionRoundRepo = competitionRoundRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<PagingResult<ViewMatch>> GetByConditions(MatchRequestModel request, string token)
        {
            PagingResult<ViewMatch> matches = await _matchRepo.GetByConditions(request);
            if (matches == null) throw new NullReferenceException("Not found any matches");

            if (!string.IsNullOrEmpty(token))
            { // manager
                int roundId = matches.Items[0].RoundId;
                bool isManager = await CheckValidManager(roundId, token);
                if (isManager) return matches;
            }

            // user
            matches.Items = matches.Items.Where(match => !match.Status.Equals(MatchStatus.IsDeleted)).ToList();
            if (matches.Items.Count.Equals(0)) throw new NullReferenceException("Not found any matches");
            // processing paging result
            matches.PageSize = matches.Items.Count;

            return matches;
        }

        public async Task<ViewMatch> GetById(int id, string token)
        {
            ViewMatch match = await _matchRepo.GetById(id);
            if (match == null) throw new NullReferenceException("Not found this match");

            if (match.Status.Equals(MatchStatus.IsDeleted))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    bool isManager = await CheckValidManager(match.RoundId, token);
                    if (isManager) return match;
                }

                throw new NullReferenceException("Not found any matches");
            }

            return match;
        }

        private async Task<bool> CheckValidManager(int roundId, string token)
        {
            int competitionId = await _competitionRoundRepo.GetCompetitionIdByRound(roundId);
            return _memberInCompetitionRepo.CheckValidManagerByUser(competitionId, _decodeToken.Decode(token, "Id"), null);
        }

        public async Task<ViewMatch> Insert(MatchInsertModel model, string token)
        {
            // check model
            if (model.RoundId.Equals(0) || string.IsNullOrEmpty(model.Address) || string.IsNullOrEmpty(model.Title)
                || string.IsNullOrEmpty(model.Description) || model.StartTime.Equals(DateTime.MinValue) || model.EndTime.Equals(DateTime.MinValue)
                || model.NumberOfTeam < 0)
                throw new ArgumentException("RoudId Null ||  Address Null || Title Null || Description Null || StartTime Null || EndTime Null || NumberOfTeam < 0");
            
            // check authorize
            bool isManager = await CheckValidManager(model.RoundId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            // validation data
            bool isDuplicated = await _matchRepo.CheckDuplicatedMatch(model.Title, model.RoundId);
            if (isDuplicated) throw new ArgumentException("Duplicated title of match in the round");

            CompetitionRound round = await _competitionRoundRepo.Get(model.RoundId);
            if (round == null) throw new ArgumentException("Not found this round");

            if (model.StartTime < round.StartTime || model.StartTime > round.EndTime
                || model.EndTime < round.StartTime || model.EndTime > round.EndTime
                || model.EndTime <= model.StartTime)
                throw new ArgumentException("The time of match must in the range time of round && StartTime < EndTime");

            // insert
            Match match = new Match()
            {
                Address = model.Address,
                CreateTime = new LocalTime().GetLocalTime().DateTime,
                Description = model.Description,
                EndTime = model.EndTime,
                IsLoseMatch = model.IsLoseMatch,
                NumberOfTeam = model.NumberOfTeam,
                RoundId = model.RoundId,
                StartTime = model.StartTime,
                Status = MatchStatus.Ready, // default status when insert
                Title = model.Title,                
            };

            int matchId = await _matchRepo.Insert(match);
            return await _matchRepo.GetById(matchId);
        }

        public async Task Update(MatchUpdateModel model, string token)
        {
            // check model
            if (model.Id.Equals(0) || model.RoundId.Equals(0)) throw new ArgumentException("Id Null || RoundId Null");

            // check authorize
            bool isManager = await CheckValidManager(model.RoundId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Match match = await _matchRepo.Get(model.Id);
            if (match == null) throw new NullReferenceException("Not found this match");

            CompetitionRound round = await _competitionRoundRepo.Get(model.RoundId);
            if (round == null) throw new NullReferenceException("Not found this round");

            DateTime currentTime = new LocalTime().GetLocalTime().DateTime;
            TimeSpan timeSpan = match.StartTime - currentTime;
            if (timeSpan.TotalMinutes < 10) throw new ArgumentException("Can not update match < 10 mins before round start");

            if (!string.IsNullOrEmpty(model.Address)) match.Address = model.Address;

            match.RoundId = model.RoundId;

            if (model.IsLoseMatch.HasValue) match.IsLoseMatch = model.IsLoseMatch.Value;

            if(!string.IsNullOrEmpty(model.Title)) match.Title = model.Title;
            
            if(!string.IsNullOrEmpty(model.Description)) match.Description = model.Description;

            if(model.StartTime.HasValue)
            {
                DateTime endTime = model.EndTime ?? match.EndTime;
                if (model.StartTime.Value < round.StartTime || model.StartTime.Value > round.EndTime
                    || model.StartTime.Value >= endTime)
                    throw new ArgumentException("The time of match must in the range time of round");
                
                match.StartTime = model.StartTime.Value;
            }

            if (model.EndTime.HasValue)
            {
                DateTime startTime = model.StartTime ?? match.StartTime;
                if (model.EndTime.Value < round.StartTime || model.EndTime.Value > round.EndTime
                    || model.EndTime.Value <= startTime)
                    throw new ArgumentException("The time of match must in the range time of round");

                match.EndTime = model.EndTime.Value;
            }

            if(model.NumberOfTeam.HasValue) match.NumberOfTeam = model.NumberOfTeam.Value;

            if (model.Status.HasValue)
            {
                // check valid status
                if (!Enum.IsDefined(typeof(MatchStatus), model.Status.Value)) 
                    throw new ArgumentException("Invalid match status");

                match.Status = model.Status.Value;
            }

            await _matchRepo.Update();
        }

        public async Task Delete(int id, string token)
        {
            Match match = await _matchRepo.Get(id);
            if (match == null) throw new NullReferenceException("Not found this match");

            bool isManager = await CheckValidManager(match.RoundId, token);
            if (!isManager) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            match.Status = MatchStatus.IsDeleted; // delete status
            await _matchRepo.Update();
        }
    }
}
