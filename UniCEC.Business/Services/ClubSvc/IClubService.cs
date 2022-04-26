using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public interface IClubService
    {
        public Task<ViewClub> GetByClub(int id);
        public Task<PagingResult<ViewClub>> GetAllPaging(PagingRequest request);        
        public Task<PagingResult<ViewClub>> GetByName(string name, PagingRequest request);
        public Task<PagingResult<ViewClub>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<List<ViewClub>> GetByUser(int userId);
        public Task<ViewClub> Insert(ClubInsertModel club);
        public Task Update(ClubUpdateModel club);
        public Task Delete(int id);
    }
}
