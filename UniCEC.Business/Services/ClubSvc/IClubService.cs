using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public interface IClubService
    {
        public Task<ViewClub> GetByClub(string token, int id, int universityId);  
        public Task<PagingResult<ViewClub>> GetByName(string token, int universityId, string name, PagingRequest request);
        public Task<PagingResult<ViewClub>> GetByCompetition(string token, int competitionId, PagingRequest request);
        public Task<PagingResult<ViewClub>> GetByUniversity(string token, int id, PagingRequest request);
        public Task<List<ViewClub>> GetByUser(string token);
        public Task<ViewClub> Insert(string token, ClubInsertModel model);
        public Task Update(string token, ClubUpdateModel model);
        public Task Delete(string token, int id);
    }
}
