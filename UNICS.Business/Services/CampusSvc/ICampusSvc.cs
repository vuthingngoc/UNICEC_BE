using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Campus;

namespace UNICS.Business.Services.CampusSvc
{
    public interface ICampusSvc
    {
        Task<PagingResult<ViewCampus>> GetAll();
        Task<ViewCampus> GetById();
        Task<bool> Insert();
        Task<bool> Update();
        Task<bool> Delete(int id);
    }
}
