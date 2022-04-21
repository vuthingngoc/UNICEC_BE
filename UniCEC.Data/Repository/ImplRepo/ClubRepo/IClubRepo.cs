using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public interface IClubRepo : IRepository<Club>
    {
        Task<List<Club>> GetByCompetition(int competitionId);
        Task<List<Club>> GetByName(string name);
        Task<bool> CheckExistedClubName(int universityId, string name);
    }
}
