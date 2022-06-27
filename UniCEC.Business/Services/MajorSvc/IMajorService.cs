using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        public Task<ViewMajor> GetById(string token, int id);
        public Task<PagingResult<ViewMajor>> GetByConditions(string token, MajorRequestModel request);
        public Task<PagingResult<ViewMajor>> GetByCompetition(int competitionId, PagingRequest request);
        public Task<ViewMajor> Insert(string token, string name);
        public Task Update(string token, MajorUpdateModel model);
        public Task Delete(string token, int id);
    }
}
