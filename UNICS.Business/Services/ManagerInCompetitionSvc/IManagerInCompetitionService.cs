using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ManagerInCompetition;

namespace UNICS.Business.Services.ManagerInCompetitionSvc
{
    public interface IManagerInCompetitionService
    {
        Task<PagingResult<ViewManagerInCompetition>> GetAll(PagingRequest request);
        Task<ViewManagerInCompetition> GetById(int id);
        Task<bool> Insert(ManagerInCompetitionInsertModel managerInCompetition);
        Task<bool> Update(ViewManagerInCompetition managerInCompetition);
        Task<bool> Delete(int id);
    }
}
