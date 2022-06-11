using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public interface IClubService
    {
        public Task<ViewClub> GetById(string token, int id);  
        public Task<PagingResult<ViewClub>> GetByConditions(string token, ClubRequestModel request);
        public Task<PagingResult<ViewClub>> GetByCompetition(string token, int competitionId, PagingRequest request);
        //public Task<PagingResult<ViewClub>> GetByUniversity(string token, int id, PagingRequest request);
        public Task<List<ViewClub>> GetByUser(string token, int id);
        public Task<ViewClub> Insert(string token, ClubInsertModel model);
        public Task Update(string token, ClubUpdateModel model);
        public Task Update(string token,int clubId, bool status); // for university admin
        public Task Delete(string token, int id);
    }
}
