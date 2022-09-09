using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.MatchType;
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.MatchTypeRepo
{
    public class MatchTypeRepo : Repository<MatchType>, IMatchTypeRepo
    {
        public MatchTypeRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<bool> CheckDuplicatedMatchType(string name)
        {
            return await context.MatchTypes.FirstOrDefaultAsync(type => type.Name.ToLower().Equals(name.ToLower())) != null;
        }

        public async Task<bool> CheckExistedType(int typeId)
        {
            return await context.MatchTypes.FirstOrDefaultAsync(type => type.Id.Equals(typeId)) != null;
        }

        public async Task<List<ViewMatchType>> GetByConditions(MatchTypeRequestModel request)
        {
            var query = from mt in context.MatchTypes
                        select mt;

            if (request.Status.HasValue) query = query.Where(mt => mt.Status.Equals(request.Status.Value));

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(mt => mt.Name.ToLower().Contains(request.Name.ToLower()));

            return (query.Any())
                ? await query.Select(mt => new ViewMatchType()
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    Status = mt.Status
                }).ToListAsync()
            : null;

        }

        public async Task<ViewMatchType> GetById(int id)
        {
            return await context.MatchTypes.Where(type => type.Id.Equals(id)).Select(type => new ViewMatchType()
            {
                Id = id,
                Name = type.Name,
                Status = type.Status
            }).FirstOrDefaultAsync();
        }
    }
}
