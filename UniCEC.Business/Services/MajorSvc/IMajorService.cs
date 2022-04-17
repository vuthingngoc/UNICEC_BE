using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;
using System.Collections.Generic;

namespace UniCEC.Business.Services.MajorSvc
{
    public interface IMajorService
    {
        public Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request);
        public Task<List<ViewMajor>> GetByUniversity(int universityId);
        public Task<PagingResult<ViewMajor>> GetMajorByCondition(MajorRequestModel request);
        public Task<ViewMajor> Insert(MajorInsertModel major);
        public Task<bool> Update(ViewMajor major);
        public Task<bool> Delete(int id);
    }
}
