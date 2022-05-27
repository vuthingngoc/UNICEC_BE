using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerRepo
{
    public class InfluencerRepo : Repository<Influencer>, IInfluencerRepo
    {

        public InfluencerRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<PagingResult<ViewInfluencer>> GetByCompetition(int competitioId, PagingRequest request)
        {
            var query = from i in context.Influencers
                        join iic in context.InfluencerInCompetitions on i.Id equals iic.InfluencerId
                        where iic.CompetitionId.Equals(competitioId)
                        select i;

            int totalCount = query.Count();
            List<ViewInfluencer> influencers = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(influencer => new ViewInfluencer()
            {
                Id = influencer.Id,
                Name = influencer.Name,
                ImageUrl = influencer.ImageUrl
            }).ToListAsync();

            return (influencers.Any()) ? new PagingResult<ViewInfluencer>(influencers, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
