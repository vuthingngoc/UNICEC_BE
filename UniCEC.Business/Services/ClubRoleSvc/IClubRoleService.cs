using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.Business.Services.ClubRoleSvc
{
    public interface IClubRoleService
    {
        public Task<List<ViewClubRole>> GetAll();
        public Task<ViewClubRole> GetById(int id);
        public Task<ViewClubRole> Insert(string name, string token);
        public Task Update(ViewClubRole model, string token);
        public Task Delete(int id, string token);
    }
}
