using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubRoleRepo
{
    public interface IClubRoleRepo : IRepository<ClubRole>
    {
        public Task<bool> CheckExistedClubRole(int id);
    }
}
