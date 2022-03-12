using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Team;

namespace UNICS.Business.Services.TeamSvc
{
    public interface ITeamSvc
    {
        Task<PagingResult<ViewTeam>> GetAll(PagingRequest request);
        Task<ViewTeam> GetById(int id);
        Task<bool> Insert(TeamInsertModel team);
        Task<bool> Update(TeamUpdateModel team);
        Task<bool> Delete(int id);
    }
}
