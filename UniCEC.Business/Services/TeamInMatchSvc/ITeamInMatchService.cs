using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.Business.Services.TeamInMatchSvc
{
    public interface ITeamInMatchService
    {
        public Task<ViewTeamInMatch> GetById(int id, string token);
        public Task<PagingResult<ViewTeamInMatch>> GetByConditions(TeamInMatchRequestModel request, string token);
        public Task<List<ViewTeamInMatch>> Insert(List<TeamInMatchInsertModel> models, string token);
        public Task Update(List<TeamInMatchUpdateModel> models, string token);
        public Task Delete(int id, string token);
    }
}
