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

        public Task<PagingResult<ViewInfluencer>> GetByCompetition(int competition)
        {
            throw new NotImplementedException();
        }

        public Task<ViewInfluencer> Insert(InfluencerInsertModel model)
        {
            throw new NotImplementedException();
        }

        public Task Update(InfluencerInsertModel model)
        {
            throw new NotImplementedException();
        }
    }
}
