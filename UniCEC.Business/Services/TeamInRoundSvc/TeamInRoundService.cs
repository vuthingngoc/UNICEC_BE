using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo;

namespace UniCEC.Business.Services.TeamInRoundSvc
{
    public class TeamInRoundService : ITeamInRoundService
    {
        private ITeamInRoundRepo _teamInRoundRepo;

        public TeamInRoundService(ITeamInRoundRepo teamInRoundRepo)
        {
            _teamInRoundRepo = teamInRoundRepo;
        }


    }
}
