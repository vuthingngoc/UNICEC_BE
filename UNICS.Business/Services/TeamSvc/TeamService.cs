using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.TeamRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Team;

namespace UNICS.Business.Services.TeamSvc
{
    public class TeamService : ITeamService
    {
        private ITeamRepo _teamRepo;

        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewTeam>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeam> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(TeamInsertModel team)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TeamUpdateModel team)
        {
            throw new NotImplementedException();
        }
    }
}
