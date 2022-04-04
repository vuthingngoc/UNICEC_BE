using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TeamRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
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

        public Task<PagingResult<ViewTeam>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeam> GetByTeamId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTeam> Insert(TeamInsertModel team)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(TeamUpdateModel team)
        {
            throw new NotImplementedException();
        }
    }
}
