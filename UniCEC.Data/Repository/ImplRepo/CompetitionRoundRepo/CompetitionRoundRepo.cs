using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundRepo
{
    public class CompetitionRoundRepo : Repository<CompetitionRound>, ICompetitionRoundRepo
    {
        public CompetitionRoundRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<PagingResult<ViewCompetitionRound>> GetByConditions(CompetitionRoundRequestModel request)
        {
            var query = from cr in context.CompetitionRounds
                        join crt in context.CompetitionRoundTypes on cr.CompetitionRoundTypeId equals crt.Id
                        where cr.CompetitionId.Equals(request.CompetitionId)
                        select new { cr, crt };

            if (!string.IsNullOrEmpty(request.Title)) query = query.Where(selector => selector.cr.Title.ToLower().Contains(request.Title.ToLower()));

            if (request.RoundTypeId.HasValue) query = query.Where(selector => selector.cr.CompetitionRoundTypeId.Equals(request.RoundTypeId.Value));

            if (request.StartTime.HasValue) query = query.Where(selector => selector.cr.StartTime.Year.Equals(request.StartTime.Value.Year)
                                                            && selector.cr.StartTime.Month.Equals(request.StartTime.Value.Month)
                                                            && selector.cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.StartTime.HasValue && request.StartTime.Value.Hour > 0) query = query.Where(selector => selector.cr.StartTime.Hour.Equals(request.StartTime.Value.Hour));

            if (request.EndTime.HasValue) query = query.Where(selector => selector.cr.EndTime.Year.Equals(request.EndTime.Value.Year)
                                                                && selector.cr.EndTime.Month.Equals(request.EndTime.Value.Month)
                                                                && selector.cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.EndTime.HasValue && request.EndTime.Value.Hour > 0)
                query = query.Where(selector => selector.cr.EndTime.Hour.Equals(request.EndTime.Value.Hour));

            if (request.Statuses != null) query = query.Where(selector => request.Statuses.Contains((int)selector.cr.Status));

            query = query.OrderBy(selector => selector.cr.StartTime);

            int totalCount = query.Count();

            List<ViewCompetitionRound> competitionRounds = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(selector => new ViewCompetitionRound()
                {
                    Id = selector.cr.Id,
                    CompetitionId = selector.cr.CompetitionId,
                    RoundTypeId = selector.cr.CompetitionRoundTypeId,
                    RoundTypeName = selector.crt.Name,
                    Title = selector.cr.Title,
                    Description = selector.cr.Description,
                    StartTime = selector.cr.StartTime,
                    EndTime = selector.cr.EndTime,
                    NumberOfTeam = selector.cr.NumberOfTeam,
                    SeedsPoint = selector.cr.SeedsPoint,
                    Status = selector.cr.Status,
                    Order = selector.cr.Order
                }).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewCompetitionRound>(competitionRounds, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewCompetitionRound> GetById(int id, CompetitionRoundStatus? status)
        {
            var query = from cr in context.CompetitionRounds
                        join crt in context.CompetitionRoundTypes on cr.CompetitionRoundTypeId equals crt.Id
                        where cr.Id.Equals(id)
                        select new { cr, crt };

            if (status.HasValue) query = query.Where(selector => selector.cr.Status.Equals(status.Value));

            return await query.Select(selector => new ViewCompetitionRound()
            {
                Id = selector.cr.Id,
                CompetitionId = selector.cr.CompetitionId,
                RoundTypeId = selector.cr.CompetitionRoundTypeId,
                RoundTypeName = selector.crt.Name,
                Title = selector.cr.Title,
                Description = selector.cr.Description,
                StartTime = selector.cr.StartTime,
                EndTime = selector.cr.EndTime,
                NumberOfTeam = selector.cr.NumberOfTeam,
                SeedsPoint = selector.cr.SeedsPoint,
                Status = selector.cr.Status,
                Order = selector.cr.Order
            }).FirstOrDefaultAsync();
        }

        private async Task<int> CheckDuplicatedTitle(int competitionId, string title)
        {
            return await (from cr in context.CompetitionRounds
                          where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                                  && cr.Title.ToLower().Equals(title.ToLower())
                          select cr.Id).FirstOrDefaultAsync();
        }

        private async Task<int> CheckInvalidStartTime(int competitionId, DateTime time)
        {
            return await (from cr in context.CompetitionRounds
                          where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                                  && cr.EndTime.CompareTo(time) > 0
                          select cr.Id).FirstOrDefaultAsync();
        }

        private async Task<int> CheckInvalidEndtime(int competitionId, DateTime time, int order)
        {
            return await (from cr in context.CompetitionRounds
                          where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                                  && cr.StartTime.CompareTo(time) < 0 && cr.Order > order
                          select cr.Id).FirstOrDefaultAsync();
        }

        public async Task<int> CheckInvalidRound(int competitionId, string title, DateTime? startTime, DateTime? endTime, int? order)
        {
            int roundId = 0;
            if (!string.IsNullOrEmpty(title))
            {
                roundId = await CheckDuplicatedTitle(competitionId, title);
                if (roundId != 0) return roundId;
            }

            if (startTime.HasValue)
            {
                roundId = await CheckInvalidStartTime(competitionId, startTime.Value);
                if (roundId != 0) return roundId;
            }

            if (endTime.HasValue)
            {
                roundId = await CheckInvalidEndtime(competitionId, endTime.Value, order.Value);
                if (roundId != 0) return roundId;
            }

            return roundId;
        }

        public async Task UpdateNumberOfTeam(int roundId, int numberOfTeam)
        {
            CompetitionRound round = await Get(roundId);
            if (round != null && numberOfTeam > 0)
            {
                round.NumberOfTeam = numberOfTeam;
                await Update();
            }
        }

        public async Task<int> GetCompetitionIdByRound(int competitionRoundId)
        {
            return await (from cr in context.CompetitionRounds
                          where cr.Id.Equals(competitionRoundId)
                          select cr.CompetitionId).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckExistedRound(int roundId)
        {
            return await context.CompetitionRounds.FirstOrDefaultAsync(round => round.Id.Equals(roundId)
                                                                                && !round.Status.Equals(CompetitionRoundStatus.Cancel)
                                                                                && !round.Status.Equals(CompetitionRoundStatus.IsDeleted)) != null;
        }

        public async Task UpdateOrderRoundsByCompe(int competitionId)
        {
            var rounds = await (from cr in context.CompetitionRounds
                                where cr.CompetitionId.Equals(competitionId)
                                      && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                                      && !cr.Status.Equals(CompetitionRoundStatus.IsDeleted)
                                orderby cr.StartTime ascending
                                select cr).ToListAsync();

            if (rounds.Count > 0)
            {
                int count = 1;
                foreach (var round in rounds)
                {
                    round.Order = count;
                    ++count;
                }
            }

            List<CompetitionRoundStatus> invalidStatuses = new List<CompetitionRoundStatus>();
            invalidStatuses.Add(CompetitionRoundStatus.Cancel);
            invalidStatuses.Add(CompetitionRoundStatus.IsDeleted);

            (from cr in context.CompetitionRounds
             where cr.CompetitionId.Equals(competitionId)
                    && invalidStatuses.Contains(cr.Status)
             select cr).ToList().ForEach(cr => cr.Order = 0);

            await Update();
        }

        public async Task<CompetitionRound> GetRoundAtOrder(int competitionId, int order)
        {
            return await (from cr in context.CompetitionRounds
                          where cr.CompetitionId.Equals(competitionId) && cr.Order.Equals(order)
                          select cr).FirstOrDefaultAsync();
        }

        public async Task UpdateStatusRoundByCompe(int competitionId, CompetitionRoundStatus status)
        {
            (from cr in context.CompetitionRounds
             where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.IsDeleted)
             select cr).ToList().ForEach(cr => cr.Status = status);

            await Update();
        }

        public async Task<int> GetNumberOfRoundsByCompetition(int competitionId)
        {
            var query = from cr in context.CompetitionRounds
                        where cr.CompetitionId.Equals(competitionId)
                            && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                            && !cr.Status.Equals(CompetitionRoundStatus.IsDeleted)
                        select cr;

            return await query.CountAsync();
        }

        public async Task<int> GetRoundTypeByMatch(int matchId)
        {
            return await (from cr in context.CompetitionRounds
                          join m in context.Matches on cr.Id equals m.RoundId
                          where m.Id.Equals(matchId)
                          select cr.CompetitionRoundTypeId).FirstOrDefaultAsync();
        }

        ////TA
        //public async Task<CompetitionRound> GetLastRound(int competitionId)
        //{
        //    return await (from cr in context.CompetitionRounds
        //                  where cr.CompetitionId.Equals(competitionId)
        //                  select cr).Max;
        //}
    }
}
