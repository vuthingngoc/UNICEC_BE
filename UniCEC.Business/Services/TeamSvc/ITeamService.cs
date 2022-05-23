using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;

namespace UniCEC.Business.Services.TeamSvc
{
    public interface ITeamService
    {
        public Task<PagingResult<ViewTeam>> GetAllPaging(PagingRequest request);
        public Task<ViewTeam> GetByTeamId(int id);
        public Task<ViewTeam> InsertTeam(TeamInsertModel team, string token);
        public Task<bool> Update(TeamUpdateModel team);
        public Task<bool> Delete(int id);
    }
}
