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

        public Task<PagingResult<ViewTeamInRound>> GetByCondition(string token, TeamInRoundRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeamInRound> GetById(string token, int id)
        {
            throw new NotImplementedException();
        }
    }
}
