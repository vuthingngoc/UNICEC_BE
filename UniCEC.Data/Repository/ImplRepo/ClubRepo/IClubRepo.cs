using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public interface IClubRepo : IRepository<Club>
    {
        Task<PagingResult<Club>> GetByCompetition(int competitionId, PagingRequest request);
        Task<PagingResult<Club>> GetByName(string name, PagingRequest request);
        Task<List<Club>> GetByUser(int userId);
        Task<int> CheckExistedClubName(int universityId, string name);
    }
}
