using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo
{
    public class SeedsWalletRepo : Repository<SeedsWallet>, ISeedsWalletRepo
    {
        public SeedsWalletRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<ViewSeedsWallet>> GetByConditions(SeedsWalletRequestModel request)
        {
            var query = from sw in context.SeedsWallets
                        join u in context.Users on sw.StudentId equals u.Id
                        join uni in context.Universities on u.UniversityId equals uni.Id
                        select new { sw, u, uni };

            if (request.UniversityId.HasValue) query = query.Where(selector => selector.uni.Id.Equals(request.UniversityId.Value));

            if (request.StudentId.HasValue) query = query.Where(selector => selector.sw.StudentId.Equals(request.StudentId.Value));

            if (request.Amount.HasValue) query = query.Where(selector => selector.sw.Amount.Equals(request.Amount.Value));

            if (request.Status.HasValue) query = query.Where(selector => selector.sw.Status.Equals(request.Status.Value));

            int totalCount = query.Count();

            List<ViewSeedsWallet> items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(selector => new ViewSeedsWallet()
                {
                    Id = selector.sw.Id,
                    Amount = selector.sw.Amount,
                    StudentId = selector.sw.StudentId,
                    Status = selector.sw.Status
                }).ToListAsync();

            return (items.Count() > 0)
                        ? new PagingResult<ViewSeedsWallet>(items, totalCount, request.CurrentPage, request.PageSize)
                        : null;
        }

        public async Task<ViewSeedsWallet> GetById(int id)
        {
            return await (from sw in context.SeedsWallets
                          where sw.Id.Equals(id)
                          select new ViewSeedsWallet()
                          {
                              Id = sw.Id,
                              StudentId = sw.StudentId,
                              Amount = sw.Amount,
                              Status = sw.Status
                          }).FirstOrDefaultAsync();
        }

        public async Task<SeedsWallet> GetByStudentId(int studentId)
        {
            var id =  await (from sw in context.SeedsWallets
                          where sw.StudentId.Equals(studentId)
                          select sw.Id).FirstOrDefaultAsync();

            return (id > 0) ? await Get(id) : null; // just for update amount
        }
    }
}
