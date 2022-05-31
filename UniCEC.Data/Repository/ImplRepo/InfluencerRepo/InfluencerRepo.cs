using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace UniCEC.Data.Repository.ImplRepo.InfluencerRepo
{
    public class InfluencerRepo : Repository<Influencer>, IInfluencerRepo
    {

        public InfluencerRepo(UniCECContext context) : base(context)
        {
        }

       

        public async Task Delete(Influencer influencer)
        {
            context.Influencers.Remove(influencer);
            await Update();
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

        public async Task<ViewInfluencer> GetById(int id)
        {
            return await (from i in context.Influencers
                        where i.Id.Equals(id)
                        select new ViewInfluencer()
                        {
                            Id = i.Id,
                            Name = i.Name,
                            ImageUrl = i.ImageUrl
                        }).FirstOrDefaultAsync();
        }

        public Task<int> Insert(Influencer influencer, int competitionId)
        {
            throw new NotImplementedException();
        }

        //Tien Anh
        public async Task<bool> CheckInfluencerInSystem(List<int> listInfluencerId)
        {
            bool result = true;
            foreach (int InfluId in listInfluencerId)
            {
                var query = await(from influencer in context.Influencers
                                  where influencer.Id == InfluId
                                  select influencer).FirstOrDefaultAsync();

                if (query == null)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
