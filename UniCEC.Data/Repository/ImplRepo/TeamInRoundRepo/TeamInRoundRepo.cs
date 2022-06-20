using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo
{
    public class TeamInRoundRepo : Repository<TeamInRound>, ITeamInRoundRepo
    {
        public TeamInRoundRepo(UniCECContext context) : base(context)
        {
        }
    }
}
