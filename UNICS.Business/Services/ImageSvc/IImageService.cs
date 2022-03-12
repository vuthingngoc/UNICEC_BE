using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Image;

namespace UNICS.Business.Services.ImageSvc
{
    public interface IImageService
    {
        Task<PagingResult<ViewImage>> GetAll(PagingRequest request);
        Task<PagingResult<ViewImage>> GetByAlbumId(int albumId);
        Task<ViewImage> GetById(int id);
        Task<bool> Insert(ImageInsertModel image);
        Task<bool> Update(ImageUpdateModel image);
        Task<bool> Delete(int id);
    }
}
