using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.InfluencerRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Business.Services.InfluencerSvc
{
    public class InfluencerService : IInfluencerService
    {
        private readonly IInfluencerRepo _influencerRepo;
        
        public InfluencerService(IInfluencerRepo influencerRepo)
        {
            _influencerRepo = influencerRepo;  
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagingResult<ViewInfluencer>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<ViewInfluencer> result = await _influencerRepo.GetByCompetition(competitionId, request);
            if (result == null) throw new NullReferenceException();
            return result; 
        }

        public Task<ViewInfluencer> Insert(InfluencerInsertModel model)
        {
            throw new NotImplementedException();
        }

        public Task Update(ViewInfluencer model)
        {
            throw new NotImplementedException();
        }
    }
}
