using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.Business.Services.InfluencerSvc
{
    public interface IInfluencerService
    {
        public Task<PagingResult<ViewInfluencer>> GetByCompetition(int competition);
        public Task<ViewInfluencer> Insert(InfluencerInsertModel model);
        public Task Update(InfluencerInsertModel model);
        public Task Delete(int id);
    }
}
