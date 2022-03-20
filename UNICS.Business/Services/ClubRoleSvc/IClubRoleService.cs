using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ClubRole;

namespace UNICS.Business.Services.ClubRoleSvc
{
    public interface IClubRoleService
    {
        public Task<PagingResult<ViewClubRole>> GetAll(PagingRequest request);
        public Task<ViewClubRole> GetByClubRoleId(int id);
        public Task<ViewClubRole> Insert(ClubRoleInsertModel clubRole);
        public Task<bool> Update(ViewClubRole clubRole);
        public Task<bool> Delete(int id);
    }
}
