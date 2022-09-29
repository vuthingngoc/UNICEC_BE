using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo
{
    public interface ICompetitionRoundRepo : IRepository<CompetitionRound>
    {
        public Task<ViewCompetitionRound> GetById(int id, CompetitionRoundStatus? status);
        public Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel request);
        public Task<int> GetCompetitionIdByRound(int competitionRoundId);
        public Task<int> CheckInvalidRound(int competitionId, string title, DateTime? startTime, DateTime? endTime, int? order);
        public Task UpdateNumberOfTeam(int roundId, int numberOfTeam);
        public Task UpdateOrderRoundsByCompe(int competitionId);
        public Task UpdateStatusRoundByCompe(int competitionId, CompetitionRoundStatus status);
        public Task<bool> CheckExistedRound(int roundId);
        public Task<CompetitionRound> GetRoundAtOrder(int competitionId, int order);
        public Task<int> GetNumberOfRoundsByCompetition(int competitionId);
        public Task<int> GetRoundTypeByMatch(int matchId);        

        //TA
        //public Task<CompetitionRound> GetLastRound(int competitionId);       
    }
}
