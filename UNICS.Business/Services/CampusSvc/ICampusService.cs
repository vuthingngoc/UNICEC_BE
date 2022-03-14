using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Campus;

namespace UNICS.Business.Services.CampusSvc
{
    public interface ICampusService
    {
        Task<PagingResult<ViewCampus>> GetAll(PagingRequest request);
        Task<ViewCampus> GetById(int id);
        Task<bool> Insert(CampusInsertModel campus);
        Task<bool> Update(CampusUpdateModel campus);
        Task<bool> Delete(int id);
    }
}
