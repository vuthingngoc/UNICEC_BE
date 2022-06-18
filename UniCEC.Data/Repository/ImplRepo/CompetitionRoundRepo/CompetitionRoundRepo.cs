using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        select cr;

            if (request.CompetitionId.HasValue) query = query.Where(cr => cr.CompetitionId.Equals(request.CompetitionId));

            if (!string.IsNullOrEmpty(request.Title)) query = query.Where(cr => cr.Title.ToLower().Contains(request.Title.ToLower()));

            if (request.StartTime.HasValue) query = query.Where(cr => cr.StartTime.Year.Equals(request.StartTime.Value.Year) && cr.StartTime.Month.Equals(request.StartTime.Value.Month) 
                && cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.StartTime.HasValue && request.StartTime.Value.Hour > 0) query = query.Where(cr => cr.StartTime.Hour.Equals(request.StartTime.Value.Hour));

            if (request.EndTime.HasValue) query = query.Where(cr => cr.EndTime.Year.Equals(request.EndTime.Value.Year) && cr.EndTime.Month.Equals(request.EndTime.Value.Month)
                && cr.StartTime.Day.Equals(request.StartTime.Value.Day));

            if (request.EndTime.HasValue && request.EndTime.Value.Hour > 0) query = query.Where(cr => cr.EndTime.Hour.Equals(request.EndTime.Value.Hour));            

            if (request.Status.HasValue) query = query.Where(cr => cr.Status.Equals(request.Status.Value));

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
    }
}
