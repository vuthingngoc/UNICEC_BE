using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Business.Services.TeamInRoundSvc
{
    public interface ITeamInRoundService
    {
        public Task<ViewTeamInRound> GetById(string token, int id);
        public Task<PagingResult<ViewTeamInRound>> GetByCondition(string token, );
    }
}
