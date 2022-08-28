using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Data.Repository.ImplRepo.ClubRepo
{
    public interface IClubRepo : IRepository<Club>
    {
        public Task<ViewClub> GetById(int id, bool? status);
        public Task<PagingResult<ViewClub>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<List<int>> GetByCompetition(int competitionId);
        public Task<PagingResult<ViewClub>> GetByConditions(ClubRequestModel request);
        public Task<PagingResult<ViewClub>> GetByManager(ClubRequestByManagerModel request);
        public Task<List<ViewClub>> GetByUser(int userId);
        public Task<List<ViewClub>> GetByUni(int universityId);
        public Task<PagingResult<ViewClub>> GetByUniversity(int universityId, PagingRequest request);
        public Task<int> CheckExistedClubName(int universityId, string name);
        public Task<int> GetUniversityByClub(int clubId);
        public Task<bool> CheckExistedClubInUniversity(int universityId, int clubId);
        public Task<bool> CheckExistedClub(int clubId);
        //TA
        public Task<ViewActivityOfClubModel> GetActivityOfClubById(int clubId);

    }
}
