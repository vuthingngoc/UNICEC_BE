using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Entities.Term;
using UniCEC.Data.Enum;
using System;

namespace UniCEC.Data.Repository.ImplRepo.TermRepo
{
    public class TermRepo : Repository<Term>, ITermRepo
    {
        public TermRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<bool> CloseOldTermByClub(int clubId)
        {
            var term = await (from t in context.Terms
                              join ch in context.ClubHistories on t.Id equals ch.TermId
                              where ch.ClubId.Equals(clubId) && ch.Status.Equals(ClubHistoryStatus.Active)
                              select t).FirstOrDefaultAsync();

            if (term != null)
            {
                term.EndTime = DateTime.Now;
                term.Status = false;
                await Update();
                return true;
            }

            return false;
        }

        public async Task<ViewTerm> GetCurrentTermByClub(int clubId)
        {
            return await (from ch in context.ClubHistories
                                      join t in context.Terms on ch.TermId equals t.Id
                                      where ch.ClubId.Equals(clubId) && t.Status.Equals(true) // current term
                                      select new ViewTerm()
                                      {
                                          Id = t.Id,
                                          Name = t.Name,
                                          CreateTime = t.CreateTime,
                                          EndTime = t.EndTime
                                      }).FirstOrDefaultAsync();     
        }

        public async Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request)
        {
            var query = (from ch in context.ClubHistories
                         join t in context.Terms on ch.TermId equals t.Id
                         where ch.ClubId.Equals(clubId)
                         select t).Distinct();

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(t => t.Name.Contains(request.Name));

            if (request.CreateTime.HasValue) query = query.Where(t => t.CreateTime.Year.Equals(request.CreateTime.Value.Year));

            if (request.EndTime.HasValue) query = query.Where(t => t.EndTime.Year.Equals(request.EndTime.Value.Year));


            int totalCount = query.Count();
            List<ViewTerm> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                            .Select(t => new ViewTerm()
                                            {
                                                Id = t.Id,
                                                Name = t.Name,
                                                CreateTime = t.CreateTime,
                                                EndTime = t.EndTime
                                            }).ToListAsync();

            return (totalCount > 0) ? new PagingResult<ViewTerm>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<ViewTerm> GetById(int clubId, int id)
        {
            var query = from ch in context.ClubHistories
                        join t in context.Terms on ch.TermId equals t.Id
                        where ch.ClubId.Equals(clubId) && t.Id.Equals(id)
                        select new ViewTerm()
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
