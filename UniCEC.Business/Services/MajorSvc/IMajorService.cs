using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        public Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request);
        public Task<PagingResult<ViewMajor>> GetByUniversity(int universityId, PagingRequest request);
        public Task<PagingResult<ViewMajor>> GetMajorByCondition(MajorRequestModel request);
        public Task<ViewMajor> Insert(MajorInsertModel major);
        public Task<bool> Update(ViewMajor major);
        public Task<bool> Delete(int id);
    }
}
