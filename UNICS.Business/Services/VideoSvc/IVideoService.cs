using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Video;

namespace UNICS.Business.Services.VideoSvc
{
    public interface IVideoService
    {
        Task<PagingResult<ViewVideo>> GetAll(PagingRequest request);
        Task<ViewVideo> GetById(int id);
        Task<bool> Insert(VideoInsertModel video);
        Task<bool> Update(VideoUpdateModel video);
        Task<bool> Delete(int id);
    }
}
