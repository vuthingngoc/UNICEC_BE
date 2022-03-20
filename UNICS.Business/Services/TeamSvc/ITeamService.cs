using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Team;

namespace UNICS.Business.Services.TeamSvc
{
    public interface ITeamService
    {
        public Task<PagingResult<ViewTeam>> GetAll(PagingRequest request);
        public Task<ViewTeam> GetByTeamId(int id);
        public Task<ViewTeam> Insert(TeamInsertModel team);
        public Task<bool> Update(TeamUpdateModel team);
        public Task<bool> Delete(int id);
    }
}
