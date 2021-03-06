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
                        where cr.CompetitionId.Equals(request.CompetitionId)
                        select cr;            

            if (!string.IsNullOrEmpty(request.Title)) query = query.Where(cr => cr.Title.ToLower().Contains(request.Title.ToLower()));

            if (request.StartTime.HasValue) query = query.Where(cr => cr.StartTime.Year.Equals(request.StartTime.Value.Year) && cr.StartTime.Month.Equals(request.StartTime.Value.Month)
                && cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.StartTime.HasValue && request.StartTime.Value.Hour > 0) query = query.Where(cr => cr.StartTime.Hour.Equals(request.StartTime.Value.Hour));

            if (request.EndTime.HasValue) query = query.Where(cr => cr.EndTime.Year.Equals(request.EndTime.Value.Year) && cr.EndTime.Month.Equals(request.EndTime.Value.Month)
                && cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.EndTime.HasValue && request.EndTime.Value.Hour > 0) query = query.Where(cr => cr.EndTime.Hour.Equals(request.EndTime.Value.Hour));

            if (request.Statuses != null) query = query.Where(cr => request.Statuses.Contains((int)cr.Status));

            query = query.OrderBy(cr => cr.StartTime);

            int totalCount = query.Count();

            List<ViewCompetitionRound> competitionRounds = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(cr => new ViewCompetitionRound()
                {
                    Id = cr.Id,
                    CompetitionId = cr.CompetitionId,
                    Title = cr.Title,
                    Description = cr.Description,
                    StartTime = cr.StartTime,
                    EndTime = cr.EndTime,
                    NumberOfTeam = cr.NumberOfTeam,
                    SeedsPoint = cr.SeedsPoint,
                    Status = cr.Status
                }).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewCompetitionRound>(competitionRounds, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewCompetitionRound> GetById(int id, bool? status)
        {
            var query = from cr in context.CompetitionRounds
                        where cr.Id.Equals(id)
                        select cr;

            if (status.HasValue) query = query.Where(cr => cr.Status.Equals(status.Value));

            return await query.Select(cr => new ViewCompetitionRound()
            {
                Id = cr.Id,
                CompetitionId = cr.CompetitionId,
                Title = cr.Title,
                Description = cr.Description,
                StartTime = cr.StartTime,
                EndTime = cr.EndTime,
                NumberOfTeam = cr.NumberOfTeam,
                SeedsPoint = cr.SeedsPoint,
                Status = cr.Status
            }).FirstOrDefaultAsync();
        }

        private async Task<int> CheckDuplicatedTitle(int competitionId, string title)
        {
            return await (from cr in context.CompetitionRounds
                        where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.Cancel) 
                                && cr.Title.ToLower().Equals(title.ToLower())
                        select cr.Id).FirstOrDefaultAsync();
        }

        private async Task<int> CheckDuplicatedTime(int competitionId, DateTime time)
        {
            return await (from cr in context.CompetitionRounds
                        where cr.CompetitionId.Equals(competitionId) && !cr.Status.Equals(CompetitionRoundStatus.Cancel)
                                && cr.StartTime.CompareTo(time) < 0 && cr.EndTime.CompareTo(time) > 0
                        select cr.Id).FirstOrDefaultAsync();
        }

        public async Task<int> CheckInvalidRound(int competitionId, string title, DateTime? startTime, DateTime? endTime)
        {
            int roundId = 0;
            if (!string.IsNullOrEmpty(title))
            {
                roundId = await CheckDuplicatedTitle(competitionId, title);
                if (roundId != 0) return roundId;
            }

            if (startTime.HasValue) 
            {
                roundId = await CheckDuplicatedTime(competitionId, startTime.Value);
                if (roundId != 0) return roundId;
            }

            if (endTime.HasValue) 
            {
                roundId = await CheckDuplicatedTime(competitionId, endTime.Value);
                if (roundId != 0) return roundId;
            } 

            return roundId;
        }

        public async Task UpdateNumberOfTeam(int roundId, int numberOfTeam)
        {
            CompetitionRound round = await Get(roundId);
            if(round != null && numberOfTeam > 0)
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
            return await context.CompetitionRounds.FirstOrDefaultAsync(round => round.Id.Equals(roundId)) != null;
        }
    }
}
