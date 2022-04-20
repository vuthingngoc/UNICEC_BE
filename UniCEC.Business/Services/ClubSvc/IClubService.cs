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
        public Task<List<ViewClub>> GetByName(string name);
        public Task<List<ViewClub>> GetByCompetition(int competitionId);
        public Task<ViewClub> Insert(ClubInsertModel club);
        public Task<bool> Update(ClubUpdateModel club);
        public Task<bool> Delete(int id);
    }
}
