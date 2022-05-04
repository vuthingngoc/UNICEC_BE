using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public interface IClubRepo : IRepository<Club>
    {
        public Task<PagingResult<ViewClub>> GetAll(PagingRequest request);
        public Task<ViewClub> GetById(int id);
        public Task<PagingResult<ViewClub>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<PagingResult<ViewClub>> GetByName(string name, PagingRequest request);
        public Task<List<ViewClub>> GetByUser(int userId);
        public Task<PagingResult<ViewClub>> GetByUniversity(int universityId, PagingRequest request);
        public Task<int> CheckExistedClubName(int universityId, string name);
       
    }
}
