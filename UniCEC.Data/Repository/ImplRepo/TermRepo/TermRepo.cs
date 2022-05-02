using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.TermRepo
{
    public class TermRepo : Repository<Term>, ITermRepo
    {
        public TermRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<Term>> GetByClub(int clubId, PagingRequest request)
        {
            var query = from ch in context.ClubHistories
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId)
                        select new { t };

            int totalCount = query.Count();
            List<Term> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                            .Select(x => new Term()
                                            {
                                                Id = x.t.Id,
                                                Name = x.t.Name,
                                                CreateTime = x.t.CreateTime,
                                                EndTime = x.t.EndTime
                                            }).Distinct().ToListAsync();

            return (totalCount > 0) ? new PagingResult<Term>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<Term>> GetByConditions(int clubId, TermRequestModel request)
        {
            var query = from ch in context.ClubHistories
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId)
                        select new { t };

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.t.Name.Contains(request.Name));
            if (!string.IsNullOrEmpty(request.CreateYear)) query = query.Where(x => x.t.CreateTime.Year.ToString().Equals(request.CreateYear));
            if (!string.IsNullOrEmpty(request.EndYear)) query = query.Where(x => x.t.EndTime.Year.ToString().Equals(request.EndYear));


            int totalCount = query.Count();
            List<Term> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                            .Select(x => new Term()
                                            {
                                                Id = x.t.Id,
                                                Name = x.t.Name,
                                                CreateTime = x.t.CreateTime,
                                                EndTime = x.t.EndTime
                                            }).Distinct().ToListAsync();

            return (totalCount > 0) ? new PagingResult<Term>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<Term> GetById(int clubId, int id)
        {
            var query = from ch in context.ClubHistories
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId) && t.Id.Equals(id)
                        select new Term()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            CreateTime = t.CreateTime,
                            EndTime = t.EndTime
                        };

            return await query.FirstOrDefaultAsync();
        }
    }
}
