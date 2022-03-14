using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Album;

namespace UNICS.Business.Services.AlbumSvc
{
    public interface IAlbumService
    {
        Task<ViewAlbum> GetById(int id);
        Task<PagingResult<ViewAlbum>> GetAlbumByCompetitionId(int competitionId);
        Task<PagingResult<ViewAlbum>> GetAll(PagingRequest request);
        Task<bool> Insert(AlbumInsertModel album);
        Task<bool> Update(AlbumUpdateModel album);
        Task<bool> Delete(int id);
    }
}
