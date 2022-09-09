using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.Business.Services.TeamInMatchSvc
{
    public class TeamInMatchService : ITeamInMatchService
    {
        public Task Delete(int id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewTeamInMatch>> GetByConditions(TeamInMatchRequestModel request, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeamInMatch> GetById(int id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeamInMatch> Insert(TeamInMatchInsertModel model, string token)
        {
            throw new NotImplementedException();
        }

        public Task Update(TeamInMatchUpdateModel model, string token)
        {
            throw new NotImplementedException();
        }
    }
}
