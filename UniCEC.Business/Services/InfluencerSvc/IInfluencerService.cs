using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Business.Services.InfluencerSvc
{
    public interface IInfluencerService
    {
        public Task<PagingResult<ViewInfluencer>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<ViewInfluencer> Insert(InfluencerInsertModel model, string token);
        public Task Update(InfluencerUpdateModel model, string token);
        //public Task Update(int id, IFormFile imageFile, string token);
        public Task Delete(int id);
    }
}
