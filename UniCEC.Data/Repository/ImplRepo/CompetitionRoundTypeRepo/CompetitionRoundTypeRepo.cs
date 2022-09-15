using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.CompetitionRoundType;
using UniCEC.Data.RequestModels;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoundTypeRepo
{
    public class CompetitionRoundTypeRepo : Repository<CompetitionRoundType>, ICompetitionRoundTypeRepo
    {
        public CompetitionRoundTypeRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<bool> CheckDuplicatedCompetitionRoundType(string name)
        {
            return await context.CompetitionRoundTypes.FirstOrDefaultAsync(type => type.Name.ToLower().Equals(name.ToLower())) != null;
        }

        public async Task<bool> CheckExistedType(int typeId)
        {
            return await context.CompetitionRoundTypes.FirstOrDefaultAsync(type => type.Id.Equals(typeId)) != null;
        }

        public async Task<List<ViewCompetitionRoundType>> GetByConditions(CompetitionRoundTypeRequestModel request)
        {
            var query = from crt in context.CompetitionRoundTypes
                        select crt;

            if (request.Status.HasValue) query = query.Where(mt => mt.Status.Equals(request.Status.Value));

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(crt => crt.Name.ToLower().Contains(request.Name.ToLower()));

            return (query.Any())
                ? await query.Select(mt => new ViewCompetitionRoundType()
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    Status = mt.Status
                }).ToListAsync()
            : null;

        }

        public async Task<ViewCompetitionRoundType> GetById(int id)
        {
            return await context.CompetitionRoundTypes.Where(type => type.Id.Equals(id)).Select(type => new ViewCompetitionRoundType()
            {
                Id = id,
                Name = type.Name,
                Status = type.Status
            }).FirstOrDefaultAsync();
        }
    }
}
