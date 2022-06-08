using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.Data.Repository.ImplRepo.ClubRoleRepo
{
    public interface IClubRoleRepo : IRepository<ClubRole>
    {
        public Task<ViewClubRole> GetById(int id);
        public Task<List<ViewClubRole>> GetAll();
        public Task Delete(ClubRole clubRole);
        public Task<bool> CheckExistedClubRole(int id);
    }
}
