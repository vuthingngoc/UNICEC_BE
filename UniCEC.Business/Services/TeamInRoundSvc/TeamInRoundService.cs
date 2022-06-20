using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Business.Services.TeamInRoundSvc
{
    public class TeamInRoundService : ITeamInRoundService
    {
        private ITeamInRoundRepo _teamInRoundRepo;

        public TeamInRoundService(ITeamInRoundRepo teamInRoundRepo)
        {
            _teamInRoundRepo = teamInRoundRepo;
        }

        public Task<PagingResult<ViewTeamInRound>> GetByConditions(string token, TeamInRoundRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeamInRound> GetById(string token, int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewTeamInRound>> Insert(string token, List<TeamInRoundInsertModel> models)
        {
            throw new NotImplementedException();
        }

        public Task Update(string token, TeamInRoundUpdateModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string token, int id)
        {
            throw new NotImplementedException();
        }
    }
}
