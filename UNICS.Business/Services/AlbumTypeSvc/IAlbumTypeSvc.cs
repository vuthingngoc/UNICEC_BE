using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.AlbumType;

namespace UNICS.Business.Services.AlbumTypeSvc
{
    public interface IAlbumTypeSvc
    {
        Task<PagingResult<ViewAlbumType>> GetAll(PagingRequest request);
        Task<ViewAlbumType> GetById(int id);
        Task<bool> Insert(AlbumTypeInsertModel albumType);
        Task<bool> Update(ViewAlbumType albumType);
        Task<bool> Delete(int id);
    }
}
