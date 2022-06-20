using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.Data.Repository.ImplRepo.TeamInRoundRepo
{
    public class TeamInRoundRepo : Repository<TeamInRound>, ITeamInRoundRepo
    {
        public TeamInRoundRepo(UniCECContext context) : base(context)
        {
        }

        public Task<PagingResult<ViewTeamInRound>> GetByConditions(TeamInRoundRequestModel request)
        {
            throw new NotImplementedException();
        }

        public async Task<ViewTeamInRound> GetById(int id)
        {
            return await (from tir in context.TeamInRounds
                          where tir.Id.Equals(id)
                          select new ViewTeamInRound()
                          {
                              Id = tir.Id,
                              TeamId = tir.TeamId,
                              RoundId = tir.RoundId,
                              Rank = tir.Rank,
                              Result = tir.Result,
                              Status = tir.Status
                          }).FirstOrDefaultAsync();
        }
    }
}
