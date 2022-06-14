using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        //public Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request);
        //public Task<PagingResult<ViewMajor>> GetByUniversity(int universityId, PagingRequest request);
        public Task<ViewMajor> GetById(string token, int id);
        public Task<ViewMajor> GetByCode(string token, string majorCode);
        public Task<PagingResult<ViewMajor>> GetMajorByConditions(string token, MajorRequestModel request);
        public Task<ViewMajor> Insert(string token, MajorInsertModel major);
        public Task Update(string token, ViewMajor major);
        public Task Delete(string token, int id);        
    }
}
