using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Club;

namespace UNICS.Business.Services.ClubSvc
{
    public interface IClubService
    {
        public Task<PagingResult<ViewClub>> GetAll(PagingRequest request);
        public Task<ViewClub> GetByClubId(int id);
        public Task<ViewClub> Insert(ClubInsertModel club);
        public Task<bool> Update(ClubUpdateModel club);
        public Task<bool> Delete(int id);
    }
}
