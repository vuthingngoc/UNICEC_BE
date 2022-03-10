using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.AlbumType;

namespace UNICS.Business.Services.AlbumTypeSvc
{
    public interface IAlbumTypeSvc
    {
        Task<ViewAlbumType> GetAll(PagingRequest request);
        Task<ViewAlbumType> GetById(int id);
        Task<bool> Insert();
        Task<bool> Update();
        Task<bool> Delete(int id);
    }
}
